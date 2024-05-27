using System.Text.Json;
using MichaelKjellander.Utils;

namespace MichaelKjellander.Models.Wordpress;

public class WpApiPost : IParsableJson
{
    public int Id { get; set; }
    public string Title { get; set; }

    public static IParsableJson ParseFromJson(JsonElement el)
    {
        int id = el.GetProperty("id").GetInt32();
        string title = el.GetProperty("title").GetProperty("rendered").GetString();

        return new WpApiPost{Id = id, Title = title};
    }
}

