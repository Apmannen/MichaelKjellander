using System.Net.Http.Headers;
using MichaelKjellander.Models.Wordpress;
using MichaelKjellander.SharedUtils.Api;
using MichaelKjellander.SharedUtils.Json;

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
        var pagesResult = await FetchAndParseFromWpApiWithHeaders<WpPage>($"pages?slug={slug}");
        ICollection<WpPage> pages = pagesResult.ParsedElements;
        return pages.FirstOrDefault();
    }
    
    //TODO: use some query builder
    public async Task<(ICollection<WpPost>,int)> GetPosts(int page = 1, string? categorySlug = null)
    {
        ICollection<WpCategory>? categories = null;
        string postsFetchString = $"posts?per_page=10&page={page}";
        if (categorySlug != null)
        {
            var categoriesResult = await FetchAndParseFromWpApiWithHeaders<WpCategory>($"categories?slug={categorySlug}");
            categories = categoriesResult.ParsedElements;
            WpCategory? category = categories.FirstOrDefault();
            if (category == null)
            {
                return ([], 0);
            }
            postsFetchString += $"&categories={category.Id}";
        }
        
        var postsResult = await FetchAndParseFromWpApiWithHeaders<WpPost>(postsFetchString);
        ICollection<WpPost> posts = postsResult.ParsedElements;
        int numPages = int.Parse(postsResult.Headers.GetValues("X-WP-TotalPages").First());
        
        var mediaIds = new HashSet<int>();
        var tagIds = new HashSet<int>();
        var categoryIds = new HashSet<int>();
        foreach (WpPost post in posts)
        {
            if (post.FeaturedMediaId != 0)
            {
                mediaIds.Add(post.FeaturedMediaId);
            }
            foreach (int tagId in post.TagIds!)
            {
                tagIds.Add(tagId);
            }
            categoryIds.Add(post.CategoryId);
        }

        ICollection<WpMedia> medias = await FetchAndParseExtras<WpMedia>("media", mediaIds);
        ICollection<WpTag> tags = await FetchAndParseExtras<WpTag>("tags", tagIds);
        categories = categories ?? await FetchAndParseExtras<WpCategory>("categories", categoryIds);

        foreach (WpPost post in posts)
        {
            post.FindAndSetCategory(categories);
            post.FindAndSetFeaturedMedia(medias);
            post.FindAndSetTags(tags);
        }

        return (posts, numPages);
    }

    private async Task<ICollection<T>> FetchAndParseExtras<T>(string type, ICollection<int> ids) where T : IParsableJson
    {
        if (ids.Count == 0)
        {
            return [];
        }
        string idsString = string.Join(",", ids);
        ICollection<T> result = await FetchAndParseFromWpApi<T>($"{type}?include={idsString}");
        return result;
    }

    private async Task<ICollection<T>> FetchAndParseFromWpApi<T>(string uri)
        where T : IParsableJson
    {
        JsonFetchElementsResult<T> result = await FetchAndParseFromWpApiWithHeaders<T>(uri);
        return result.ParsedElements;
    }
    private async Task<JsonFetchElementsResult<T>> FetchAndParseFromWpApiWithHeaders<T>(string uri)
        where T : IParsableJson
    {
        JsonFetchResult result = await ApiUtil.FetchJson("https://michaelkjellander.se/wp-json/wp/v2/" + uri, _client);
        ICollection<T> parsedElements = JsonUtil.ParseList<T>(result.Root);
        return new JsonFetchElementsResult<T>(parsedElements, result.Headers);
    }
    
    //TODO: record?
    private struct JsonFetchElementsResult<T>(ICollection<T> parsedElements, HttpResponseHeaders headers) where T : IParsableJson
    {
        public readonly ICollection<T> ParsedElements = parsedElements;
        public readonly HttpResponseHeaders Headers = headers;
    }
}