using System.Text.Json;

namespace MichaelKjellander.Models.Wordpress;

public class WpTag : WordpressModel
{
    public string? Name { get; set; }
    public string? Slug { get; set; }
    
    public override WpTag ParseFromJson(JsonElement el)
    {
        Id = el.GetProperty("term_id").GetInt32();
        Name = el.GetProperty("name").GetString();
        Slug = el.GetProperty("slug").GetString();

        return this;
    }
}