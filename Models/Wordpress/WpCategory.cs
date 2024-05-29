using System.Text.Json;
using MichaelKjellander.Utils;

namespace MichaelKjellander.Models.Wordpress;

public class WpCategory : IParsableJson
{
    public int Id { get; private init; }
    public string? Name { get; private init; }
    public static IParsableJson ParseFromJson(JsonElement el)
    {
        int id = el.GetProperty("id").GetInt32();
        string name = el.GetProperty("name").GetString()!;

        return new WpCategory{Id = id, Name = name};
    }
}