using System.Net.Http.Headers;
using System.Text.Json;
using MichaelKjellander.Models;
using MichaelKjellander.Models.Wordpress;
using MichaelKjellander.SharedUtils.Api;

namespace MichaelKjellander.Services;

public class WpApiService
{
    private readonly HttpClient _client;

    public WpApiService(HttpClient client)
    {
        this._client = client;
    }
    
    public async Task<WpPage?> GetPage(string slug = "")
    {
        var pagesResult = await ApiUtil.FetchJson(CreateRequestUrl($"pages?slug={slug}"), _client);
        IList<WpPage> parsedPages = Model.ParseList<WpPage>(pagesResult.Root);
        return parsedPages.FirstOrDefault();
    }
    
    //TODO: use some query builder, #16
    //TODO: multiple return values isn't the best
    public async Task<(IList<WpPost>,int)> GetPosts(int page = 1, string? categorySlug = null, string? postSlug = null)
    {
        string postsFetchString = $"posts?page={page}";
        if (postSlug != null)
        {
            postsFetchString += $"&slug={postSlug}";
        }
        if (categorySlug != null)
        {
            postsFetchString += $"&category_slug={categorySlug}";
        }
        string fullUrl = CreateRequestUrl(postsFetchString, "sgplugin/v1");
        var postsResult = await ApiUtil.FetchJson(fullUrl, _client);
        JsonElement root = postsResult.Root;
        int numPages = root.GetProperty("num_pages").GetInt32();
        IList<WpPost> parsedPosts = Model.ParseList<WpPost>(root.GetProperty("posts"));

        return (parsedPosts, numPages);
    }

    private static string CreateRequestUrl(string uri, string nameSpace = "wp/v2")
    {
        return $"https://michaelkjellander.se/wp-json/{nameSpace}/{uri}";
    }
}