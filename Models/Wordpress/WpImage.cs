using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace MichaelKjellander.Models.Wordpress;

[Table("wp_images")]
public class WpImage : WordpressModel
{
    [Required]
    [MaxLength(VarcharLength)]
    public string? ThumbnailUrl { get; set; }
    [Required]
    [MaxLength(VarcharLength)]
    public string? FullUrl { get; set; }

    public bool IsSet => Id != 0;
    
    public override WpImage ParseFromJson(JsonElement el)
    {
        bool isSet = el.GetProperty("id").ValueKind != JsonValueKind.Null;
        if (!isSet)
        {
            return this;
        }
        Id = el.GetProperty("id").GetInt32();
        ThumbnailUrl = el.GetProperty("thumbnail").GetString();
        FullUrl = el.GetProperty("full").GetString();

        return this;
    }
}