using System.Text.Json;

namespace MichaelKjellander.SharedUtils;

public static class JsonUtil
{
    public static List<T> ParseList<T>(JsonElement root) where T : IParsableJson 
    {
        List<T> list = [];
        foreach (var el in root.EnumerateArray())
        {
            T parsableJson = Activator.CreateInstance<T>();
            parsableJson.ParseFromJson(el);
            list.Add(parsableJson);
        }
        return list;
    }
}

public interface IParsableJson
{
    public void ParseFromJson(JsonElement el);
}