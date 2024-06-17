using System.Text.Json;
using MichaelKjellander.IndependentUtils.Api;
using MichaelKjellander.IndependentUtils.Parsers.Json;
using MichaelKjellander.IndependentUtils.Parsers.TranslationFile;
using MichaelKjellander.Models.Wordpress;
using MichaelKjellander.Tools.Url;

namespace MichaelKjellander.Services;

public class WpApiService
{
    private readonly HttpClient _client;
    private const string WpApiBaseUrl = "https://michaelkjellander.se/wp-json";
    private const string NamespaceDefault = "wp/v2";
    private const string NamespacePlugin = "sgplugin/v1";

    public WpApiService(HttpClient client)
    {
        this._client = client;
    }

    public async Task<List<WpTranslationEntry>> GetTranslations()
    {
        var result = await ApiUtil.FetchJson(GetFullBaseUrl(NamespacePlugin, "translations"), _client);
        JsonElement root = result.Root;
        string translationFileContent = root.GetString()!;
        List<WpTranslationEntry> entries = TranslationFileParser.ParsePortableObjectFile(translationFileContent).Select(
            pair => new WpTranslationEntry()
            {
                Key = pair.Key,
                Text = pair.Value,
                Language = Language.Swedish
            }).ToList();

        return entries;
    }

    public async Task<IList<WpCategory>> GetCategories()
    {
        var result = await ApiUtil.FetchJson(GetFullBaseUrl(NamespaceDefault, "categories"), _client);
        JsonElement root = result.Root;
        IList<WpCategory> parsedItems = ParseList<WpCategory>(root);

        return parsedItems;
    }

    public async Task<IList<WpTag>> GetTags()
    {
        var result = await ApiUtil.FetchJson(GetFullBaseUrl(NamespaceDefault, "tags"), _client);
        JsonElement root = result.Root;
        IList<WpTag> parsedItems = ParseList<WpTag>(root);

        return parsedItems;
    }

    public async Task<IList<WpPage>> GetPages()
    {
        var pagesResult = await ApiUtil.FetchJson($"{GetFullBaseUrl(NamespaceDefault, "pages")}", _client);
        IList<WpPage> parsedPages = ParseList<WpPage>(pagesResult.Root);
        return parsedPages;
    }

    //TODO: multiple return values isn't the best
    public async Task<(IList<WpPost>, int)> GetPosts(int page = 1, string[]? metaPlatforms = null,
        int[]? metaRatings = null,
        string? categorySlug = null, string? postSlug = null)
    {
        string fullUrl = new UrlBuilder(GetFullBaseUrl(NamespacePlugin, "posts"), QueryArrayMode.CommaSeparated)
            .AddParam("page", page)
            .AddParam("formats", metaPlatforms)
            .AddParam("ratings", metaRatings)
            .AddParam("category_slug", categorySlug)
            .AddParam("slug", postSlug)
            .ToString();

        var postsResult = await ApiUtil.FetchJson(fullUrl, _client);
        JsonElement root = postsResult.Root;
        if (root.ValueKind != JsonValueKind.Object)
        {
            return ([], 1);
        }

        int numPages = root.GetProperty("num_pages").GetInt32();
        IList<WpPost> parsedPosts = ParseList<WpPost>(root.GetProperty("posts"));

        return (parsedPosts, numPages);
    }

    private static List<T> ParseList<T>(JsonElement collectionElement) where T : IParsableJson
    {
        return JsonParser.ParseParsableJsonCollection<T>(collectionElement.EnumerateArray());
    }

    private static string GetFullBaseUrl(string nameSpace, string path)
    {
        return $"{WpApiBaseUrl}/{nameSpace}/{path}";
    }
}