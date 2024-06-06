using System.Net.Http.Headers;
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
    
    public async Task<WpPage?> GetPage(string slug = "")
    {
        var pagesResult = await ApiUtil.FetchJson($"{GetFullBaseUrl(NamespaceDefault, "pages")}?slug={slug}", _client);
        IList<WpPage> parsedPages = Model.ParseList<WpPage>(pagesResult.Root);
        return parsedPages.FirstOrDefault();
    }
    
    //TODO: multiple return values isn't the best
    public async Task<(IList<WpPost>,int)> GetPosts(int page = 1, int[]? metaRatings = null, string? categorySlug = null, string? postSlug = null)
    {
        string fullUrl = new HttpQueryBuilder()
            .Add("page", page)
            .Add("ratings", metaRatings)
            .Add("category_slug", categorySlug)
            .Add("post_slug", postSlug)
            .Build(GetFullBaseUrl(NamespacePlugin, "posts"));
        
        var postsResult = await ApiUtil.FetchJson(fullUrl, _client);
        JsonElement root = postsResult.Root;
        int numPages = root.GetProperty("num_pages").GetInt32();
        IList<WpPost> parsedPosts = Model.ParseList<WpPost>(root.GetProperty("posts"));

        return (parsedPosts, numPages);
    }

    private static string GetFullBaseUrl(string nameSpace, string path)
    {
        return $"{WpApiBaseUrl}/{nameSpace}/{path}";
    }
}