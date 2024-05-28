using System.Text.Json;
using MichaelKjellander.Models.Wordpress;
using MichaelKjellander.Utils;

namespace MichaelKjellander.Data;

public class WpContext
{
    private readonly HttpClient _client;

    //TODO: if needed, create instance of class and keep collections of fetches here (posts, categories etc.)
    public WpContext(HttpClient client)
    {
        this._client = client;
    }

    public async Task<ICollection<WpApiPost>> GetPosts()
    {
        ICollection<WpApiPost> posts = await FetchAndParseFromApi<WpApiPost>("posts?per_page=10", _client);
        ICollection<WpApiCategory> categories = await FetchAndParseFromApi<WpApiCategory>("categories", _client);

        //Medias and tags
        var mediaIds = new HashSet<int>();
        var tagIds = new HashSet<int>();
        foreach (WpApiPost post in posts)
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
        ICollection<WpApiMedia> medias =
            await FetchAndParseFromApi<WpApiMedia>($"media?include={mediaIdsString}", _client);

        string tagIdsString = string.Join(",", tagIds);
        ICollection<WpApiTag> tags = await FetchAndParseFromApi<WpApiTag>($"tags?include={tagIdsString}", _client);

        foreach (WpApiPost post in posts)
        {
            post.FindAndSetCategory(categories);
            post.FindAndSetFeaturedMedia(medias);
            post.FindAndSetTags(tags);
        }

        return posts;
    }

    private static async Task<ICollection<T>> FetchAndParseFromApi<T>(string uri, HttpClient client)
        where T : IParsableJson
    {
        JsonElement element = await ApiUtil.FetchJson("https://michaelkjellander.se/wp-json/wp/v2/" + uri, client);
        return JsonUtil.ParseList<T>(element);
    }
}