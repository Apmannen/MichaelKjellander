using System.Text.Json;

namespace MichaelKjellander.Models.Wordpress;

public class WpImage : WordpressModel
{
    public string? ThumbnailUrl { get; set; }
    public string? FullUrl { get; set; }

    public bool IsSet => ThumbnailUrl != null && FullUrl != null;
    
    public override WpImage ParseFromJson(JsonElement el)
    {
        ThumbnailUrl = el.GetProperty("thumbnail").GetString();
        FullUrl = el.GetProperty("full").GetString();

        return this;
    }
}