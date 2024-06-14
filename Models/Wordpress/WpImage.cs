using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace MichaelKjellander.Models.Wordpress;

public class WpImage : WordpressModel
{
    [Key]
    [Required]
    public int InternalId { get; set; }
    [Required]
    [MaxLength(VarcharLength)]
    public string? ThumbnailUrl { get; set; }
    [Required]
    [MaxLength(VarcharLength)]
    public string? FullUrl { get; set; }

    public bool IsSet => ThumbnailUrl != null && FullUrl != null;
    
    public override WpImage ParseFromJson(JsonElement el)
    {
        ThumbnailUrl = el.GetProperty("thumbnail").GetString();
        FullUrl = el.GetProperty("full").GetString();

        return this;
    }
}