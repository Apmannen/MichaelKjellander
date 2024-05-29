using System.Text.Json;
using MichaelKjellander.SharedUtils;

namespace MichaelKjellander.Models.Wordpress;

public class WpTag : IParsableJson
{
    public int Id { get; private set; }
    public string? Name { get; private set; }
    public void ParseFromJson(JsonElement el)
    {
        this.Id = el.GetProperty("id").GetInt32();
        this.Name = el.GetProperty("name").GetString();
    }
}