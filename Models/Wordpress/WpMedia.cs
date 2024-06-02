using System.Text.Json;

namespace MichaelKjellander.Models.Wordpress;

public class WpMedia : Model
{
    public int Id { get; set; }
    public string? PostThumbnailUrl { get; set; }

    public override void ParseFromJson(JsonElement el)
    {
        this.Id = el.GetProperty("id").GetInt32();
        this.PostThumbnailUrl = el.GetProperty("media_details").GetProperty("sizes").GetProperty("post-thumbnail")
            .GetProperty("source_url")
            .GetString();
    }
}