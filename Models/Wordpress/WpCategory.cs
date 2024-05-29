using System.Text.Json;
using MichaelKjellander.SharedUtils.Json;

namespace MichaelKjellander.Models.Wordpress;

public class WpCategory : IParsableJson
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public void ParseFromJson(JsonElement el)
    {
        this.Id = el.GetProperty("id").GetInt32();
        this.Name = el.GetProperty("name").GetString()!;
    }
}