using System.Text.Json;

namespace MichaelKjellander.Models.Wordpress;

public class WpTag : Model
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public override void ParseFromJson(JsonElement el)
    {
        this.Id = el.GetProperty("id").GetInt32();
        this.Name = el.GetProperty("name").GetString();
    }
}