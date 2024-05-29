using System.Net.Http.Headers;
using MichaelKjellander.Models.Wordpress;
using MichaelKjellander.SharedUtils.Api;
using MichaelKjellander.SharedUtils.Json;

namespace MichaelKjellander.Data;

public class WpContext
{
    private readonly HttpClient _client;

    public WpContext(HttpClient client)
    {
        this._client = client;
    }

    public async Task<(ICollection<WpPost>,int)> GetPosts()
    {
        var postsResult = await FetchAndParseFromWpApiWithHeaders<WpPost>("posts?per_page=10", _client);
        ICollection<WpPost> posts = postsResult.ParsedElements;
        int numPages = int.Parse(postsResult.Headers.GetValues("X-WP-TotalPages").First());
        ICollection<WpCategory> categories = await FetchAndParseFromWpApi<WpCategory>("categories", _client);

        //Medias and tags
        var mediaIds = new HashSet<int>();
        var tagIds = new HashSet<int>();
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
        }

        string mediaIdsString = string.Join(",", mediaIds);
        ICollection<WpMedia> medias =
            await FetchAndParseFromWpApi<WpMedia>($"media?include={mediaIdsString}", _client);

        string tagIdsString = string.Join(",", tagIds);
        ICollection<WpTag> tags = await FetchAndParseFromWpApi<WpTag>($"tags?include={tagIdsString}", _client);

        foreach (WpPost post in posts)
        {
            post.FindAndSetCategory(categories);
            post.FindAndSetFeaturedMedia(medias);
            post.FindAndSetTags(tags);
        }

        return (posts, numPages);
    }

    private static async Task<ICollection<T>> FetchAndParseFromWpApi<T>(string uri, HttpClient client)
        where T : IParsableJson
    {
        JsonFetchElementsResult<T> result = await FetchAndParseFromWpApiWithHeaders<T>(uri, client);
        return result.ParsedElements;
    }
    private static async Task<JsonFetchElementsResult<T>> FetchAndParseFromWpApiWithHeaders<T>(string uri, HttpClient client)
        where T : IParsableJson
    {
        JsonFetchResult result = await ApiUtil.FetchJson("https://michaelkjellander.se/wp-json/wp/v2/" + uri, client);
        ICollection<T> parsedElements = JsonUtil.ParseList<T>(result.Root);
        return new JsonFetchElementsResult<T>(parsedElements, result.Headers);
    }
    
    private struct JsonFetchElementsResult<T>(ICollection<T> parsedElements, HttpResponseHeaders headers) where T : IParsableJson
    {
        public readonly ICollection<T> ParsedElements = parsedElements;
        public readonly HttpResponseHeaders Headers = headers;
    }
}