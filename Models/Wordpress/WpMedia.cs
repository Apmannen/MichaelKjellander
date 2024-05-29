using System.Text.Json;
using MichaelKjellander.Utils;
using Microsoft.AspNetCore.Authentication;

namespace MichaelKjellander.Models.Wordpress;

public class WpMedia : IParsableJson
{
    public int Id { get; private init; }
    public string? PostThumbnailUrl { get; private init; }

    public static IParsableJson ParseFromJson(JsonElement el)
    {
        var id = el.GetProperty("id").GetInt32();
        var postThumbnailUrl = el.GetProperty("media_details").GetProperty("sizes").GetProperty("post-thumbnail")
            .GetProperty("source_url")
            .GetString();

        return new WpMedia { Id = id, PostThumbnailUrl = postThumbnailUrl };
    }
}