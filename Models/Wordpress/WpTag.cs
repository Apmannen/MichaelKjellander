using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace MichaelKjellander.Models.Wordpress;

[Table("wp_tags")]
public class WpTag : WordpressModel
{
    public string? Name { get; set; }
    public string? Slug { get; set; }
    public IList<WpPostTag> PostTags { get; set; }
    
    public override WpTag ParseFromJson(JsonElement el)
    {
        Id = el.GetProperty("id").GetInt32();
        Name = el.GetProperty("name").GetString();
        Slug = el.GetProperty("slug").GetString();

        return this;
    }
}