using System.Text.Json;
using MichaelKjellander.Utils;

namespace MichaelKjellander.Models.Wordpress;

public class WpApiPost : IParsableJson
{
    public int Id { get; private init; }
    public string? Title { get; private init; }

    public static IParsableJson ParseFromJson(JsonElement el)
    {
        var id = el.GetProperty("id").GetInt32();
        var title = el.GetProperty("title").GetProperty("rendered").GetString();

        return new WpApiPost{Id = id, Title = title};
    }
}

