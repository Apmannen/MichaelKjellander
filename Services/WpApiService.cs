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
        //var pagesResult = await FetchAndParseFromWpApiWithHeaders<WpPage>($"pages?slug={slug}");
        var pagesResult = await ApiUtil.FetchJson(CreateRequestUrl($"pages?slug={slug}"), _client);
        IList<WpPage> parsedPages = Model.ParseList<WpPage>(pagesResult.Root);
        //ICollection<WpPage> pages = pagesResult.ParsedElements;
        return parsedPages.FirstOrDefault();
    }
    
    //TODO: use some query builder, #16
    public async Task<(IList<WpPost>,int)> GetPosts(int page = 1, string? categorySlug = null, string? postSlug = null)
    {
        //ICollection<WpCategory>? categories = null;
        string postsFetchString = $"posts?page={page}";

        if (postSlug != null)
        {
            postsFetchString += $"&slug={postSlug}";
        }

        if (categorySlug != null)
        {
            postsFetchString += $"&category_slug={categorySlug}";
        }
        // if (categorySlug != null)
        // {
        //     var categoriesResult = await FetchAndParseFromWpApiWithHeaders<WpCategory>($"categories?slug={categorySlug}");
        //     categories = categoriesResult.ParsedElements;
        //     WpCategory? category = categories.FirstOrDefault();
        //     if (category == null)
        //     {
        //         return ([], 0);
        //     }
        //     postsFetchString += $"&categories={category.Id}";
        // }
        
        //var postsResult = await FetchAndParseFromWpApiWithHeaders<WpPost>(postsFetchString);
        //IList<WpPost> posts = postsResult.ParsedElements;
        //int numPages = int.Parse(postsResult.Headers.GetValues("X-WP-TotalPages").First());
        
        // var mediaIds = new HashSet<int>();
        // var tagIds = new HashSet<int>();
        // var categoryIds = new HashSet<int>();
        // foreach (WpPost post in posts)
        // {
        //     if (post.FeaturedMediaId != 0)
        //     {
        //         mediaIds.Add(post.FeaturedMediaId);
        //     }
        //     foreach (int tagId in post.TagIds!)
        //     {
        //         tagIds.Add(tagId);
        //     }
        //     categoryIds.Add(post.CategoryId);
        // }
        //
        // ICollection<WpMedia> medias = await FetchAndParseExtras<WpMedia>("media", mediaIds);
        // ICollection<WpTag> tags = await FetchAndParseExtras<WpTag>("tags", tagIds);
        // categories = categories ?? await FetchAndParseExtras<WpCategory>("categories", categoryIds);

        // foreach (WpPost post in posts)
        // {
        //     post.FindAndSetCategory(categories);
        //     post.FindAndSetFeaturedMedia(medias);
        //     post.FindAndSetTags(tags);
        // }

        string fullUrl = CreateRequestUrl(postsFetchString, "sgplugin/v1");
        var postsResult = await ApiUtil.FetchJson(fullUrl, _client);
        JsonElement root = postsResult.Root;
        int numPages = root.GetProperty("num_pages").GetInt32();
        IList<WpPost> parsedPosts = Model.ParseList<WpPost>(root.GetProperty("posts"));

        return (parsedPosts, numPages);
    }

    private string CreateRequestUrl(string uri, string nameSpace = "wp/v2")
    {
        return $"https://michaelkjellander.se/wp-json/{nameSpace}/{uri}";
    }

    // private async Task<ICollection<T>> FetchAndParseExtras<T>(string type, ICollection<int> ids) where T : Model
    // {
    //     if (ids.Count == 0)
    //     {
    //         return [];
    //     }
    //     string idsString = string.Join(",", ids);
    //     ICollection<T> result = await FetchAndParseFromWpApi<T>($"{type}?include={idsString}");
    //     return result;
    // }

    // private async Task<ICollection<T>> FetchAndParseFromWpApi<T>(string uri)
    //     where T : Model
    // {
    //     JsonFetchElementsResult<T> result = await FetchAndParseFromWpApiWithHeaders<T>(uri);
    //     return result.ParsedElements;
    // }
    //
    // private async Task<JsonFetchElementsResult<T>> FetchAndParseFromWpApiWithHeaders<T>(string uri, string nameSpace = "wp/v2")
    //     where T : Model
    // {
    //     JsonFetchResult result = await ApiUtil.FetchJson($"https://michaelkjellander.se/wp-json/{nameSpace}/{uri}", _client);
    //     IList<T> parsedElements = Model.ParseList<T>(result.Root);
    //     return new JsonFetchElementsResult<T>(parsedElements, result.Headers);
    // }
    //
    // //TODO: record?
    // private struct JsonFetchElementsResult<T>(IList<T> parsedElements, HttpResponseHeaders headers) where T : Model
    // {
    //     public readonly IList<T> ParsedElements = parsedElements;
    //     public readonly HttpResponseHeaders Headers = headers;
    // }
}