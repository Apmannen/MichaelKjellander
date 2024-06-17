using System.Text.Json;
using MichaelKjellander.Models;
using MichaelKjellander.Models.Wordpress;
using MichaelKjellander.SharedUtils.Api;
using MichaelKjellander.Tools.Parsers;

namespace MichaelKjellander.Communicators;

public class WpApiCommunicator
{
    private readonly HttpClient _client;
    private const string WpApiBaseUrl = "https://michaelkjellander.se/wp-json";
    private const string NamespaceDefault = "wp/v2";
    private const string NamespacePlugin = "sgplugin/v1";

    public WpApiCommunicator(HttpClient client)
    {
        this._client = client;
    }

    public async Task<List<WpTranslationEntry>> GetTranslations()
    {
        var result = await ApiUtil.FetchJson(GetFullBaseUrl(NamespacePlugin, "translations"), _client);
        JsonElement root = result.Root;
        string translationFileContent = root.GetString()!;

        //TODO: should have a new directory for parsers. Include JSON parser and po/pot there.
        //It could have it's own DTO format to return.
        List<WpTranslationEntry> entries = new List<WpTranslationEntry>();
        string[] rows = translationFileContent.Split("\n");
        WpTranslationEntry? currentEntry = null;
        foreach (string row in rows)
        {
            string[] pieces = row.Split(' ', 2);
            if (pieces.Length != 2)
            {
                currentEntry = null;
                continue;
            }
            if (currentEntry == null)
            {
                currentEntry = new WpTranslationEntry();
                currentEntry.Language = Language.Swedish;
            }

            string fileKey = pieces[0];
            string fileValue = pieces[1].Substring(1, pieces[1].Length - 2);
            switch (fileKey)
            {
                case "msgid":
                    currentEntry.Key = fileValue;
                    break;
                case "msgstr":
                    currentEntry.Text = fileValue;
                    break;
            }

            if (currentEntry is { Key.Length: > 0, Text: not null })
            {
                entries.Add(currentEntry);
                currentEntry = null;
            }
        }
        
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

    private static List<T> ParseList<T>(JsonElement collectionElement) where T : IParsableJson
    {
        return JsonParser.ParseParsableJsonCollection<T>(collectionElement.EnumerateArray());
    }

    private static string GetFullBaseUrl(string nameSpace, string path)
    {
        return $"{WpApiBaseUrl}/{nameSpace}/{path}";
    }
    
    
}