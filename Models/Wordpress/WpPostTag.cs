using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace MichaelKjellander.Models.Wordpress;

[Table("wp_post_tags")]
public class WpPostTag : WordpressModel
{
    [Required]
    public int PostId { get; set; }
    public WpPost? Post { get; set; }
    
    [Required]
    public int TagId { get; set; }
    public WpTag Tag { get; set; }
    
    public override IParsableJson ParseFromJson(JsonElement el)
    {
        throw new NotImplementedException();
    }
}