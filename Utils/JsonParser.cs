using System.Text.Json;
using MichaelKjellander.Models.Wordpress;

namespace MichaelKjellander.Utils;

public class JsonParser
{
    public static async Task<List<T>> ParseResponseList<T>(HttpResponseMessage responseMessage) where T : IParsableJson 
    {
        using var responseStream = await responseMessage.Content.ReadAsStreamAsync();
        using StreamReader reader = new StreamReader(responseStream);

        string content = reader.ReadToEnd();

        using JsonDocument doc = JsonDocument.Parse(content);
        JsonElement root = doc.RootElement;

        List<T> list = new();
        foreach (JsonElement el in root.EnumerateArray())
        {
            list.Add((T) T.ParseFromJson(el));
        }

        return list;
    }
}

public interface IParsableJson
{
    public static abstract IParsableJson ParseFromJson(JsonElement el);
}