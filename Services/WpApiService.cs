using System.Text.Json;
using MichaelKjellander.Models;
using MichaelKjellander.Models.Wordpress;
using MichaelKjellander.SharedUtils.Api;

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
    
    public async Task<IList<WpCategory>> GetCategories()
    {
        var result = await ApiUtil.FetchJson(GetFullBaseUrl(NamespaceDefault, "categories"), _client);
        JsonElement root = result.Root;
        IList<WpCategory> parsedItems = ParseList<WpCategory>(root);

        return parsedItems;
    }

    public async Task<IList<string>> GetMetas()
    {
        var pagesResult = await ApiUtil.FetchJson($"{GetFullBaseUrl(NamespacePlugin, "metas")}", _client);
        var platformsEnumerator = pagesResult.Root.GetProperty("formats").EnumerateArray();
        List<string> platforms = [];
        foreach (JsonElement el in platformsEnumerator)
        {
            platforms.Add(el.GetString()!);
        }

        return platforms;
    }

    public async Task<WpPage?> GetPage(string slug = "")
    {
        var pagesResult = await ApiUtil.FetchJson($"{GetFullBaseUrl(NamespaceDefault, "pages")}?slug={slug}", _client);
        IList<WpPage> parsedPages = ParseList<WpPage>(pagesResult.Root);
        return parsedPages.FirstOrDefault();
    }

    //TODO: multiple return values isn't the best
    public async Task<(IList<WpPost>, int)> GetPosts(int page = 1, string[]? metaPlatforms = null,
        int[]? metaRatings = null,
        string? categorySlug = null, string? postSlug = null)
    {
        string fullUrl = new HttpQueryBuilder(GetFullBaseUrl(NamespacePlugin, "posts"), QueryArrayMode.CommaSeparated)
            .Add("page", page)
            .Add("formats", metaPlatforms)
            .Add("ratings", metaRatings)
            .Add("category_slug", categorySlug)
            .Add("slug", postSlug)
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

    private static string GetFullBaseUrl(string nameSpace, string path)
    {
        return $"{WpApiBaseUrl}/{nameSpace}/{path}";
    }
    
    private static List<T> ParseList<T>(JsonElement root) where T : Model 
    {
        List<T> list = [];
        foreach (JsonElement el in root.EnumerateArray())
        {
            list.Add(Model.ParseNewFromJson<T>(el));
        }
        return list;
    }
}