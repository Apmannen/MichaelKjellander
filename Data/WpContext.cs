using System.Net.Http.Headers;
using System.Text.Json;
using MichaelKjellander.Models.Wordpress;
using MichaelKjellander.Utils;

namespace MichaelKjellander.Data;

public class WpContext
{
    private readonly HttpClient _client;

    //public ICollection<WpApiPost> Posts { get; private set; } //TODO: don't have it here
    //public int NumPages { get; private set; } //TODO: don't have it here

    //TODO: if needed, create instance of class and keep collections of fetches here (posts, categories etc.)
    public WpContext(HttpClient client)
    {
        this._client = client;
    }

    public async Task<(ICollection<WpPost>,int)> GetPosts()
    {
        var postsResult = await FetchAndParseFromApiWithHeaders<WpPost>("posts?per_page=10", _client);
        ICollection<WpPost> posts = postsResult.ParsedElements;
        int numPages = int.Parse(postsResult.Headers.GetValues("X-WP-TotalPages").First());
        ICollection<WpCategory> categories = await FetchAndParseFromApi<WpCategory>("categories", _client);
        

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
            await FetchAndParseFromApi<WpMedia>($"media?include={mediaIdsString}", _client);

        string tagIdsString = string.Join(",", tagIds);
        ICollection<WpTag> tags = await FetchAndParseFromApi<WpTag>($"tags?include={tagIdsString}", _client);

        foreach (WpPost post in posts)
        {
            post.FindAndSetCategory(categories);
            post.FindAndSetFeaturedMedia(medias);
            post.FindAndSetTags(tags);
        }

        return (posts, numPages);
    }

    private static async Task<ICollection<T>> FetchAndParseFromApi<T>(string uri, HttpClient client)
        where T : IParsableJson
    {
        JsonFetchElementsResult<T> result = await FetchAndParseFromApiWithHeaders<T>(uri, client);
        return result.ParsedElements;
    }
    private static async Task<JsonFetchElementsResult<T>> FetchAndParseFromApiWithHeaders<T>(string uri, HttpClient client)
        where T : IParsableJson
    {
        ApiUtil.JsonFetchResult result = await ApiUtil.FetchJson("https://michaelkjellander.se/wp-json/wp/v2/" + uri, client);
        ICollection<T> parsedElements = JsonUtil.ParseList<T>(result.Root);
        return new JsonFetchElementsResult<T>(parsedElements, result.Headers);
    }
    
    private struct JsonFetchElementsResult<T>(ICollection<T> parsedElements, HttpResponseHeaders headers) where T : IParsableJson
    {
        public readonly ICollection<T> ParsedElements = parsedElements;
        public readonly HttpResponseHeaders Headers = headers;
    }
}