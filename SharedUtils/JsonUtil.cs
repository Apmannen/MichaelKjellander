using System.Text.Json;

namespace MichaelKjellander.Utils;

public static class JsonUtil
{
    public static List<T> ParseList<T>(JsonElement root) where T : IParsableJson 
    {
        List<T> list = [];
        foreach (var el in root.EnumerateArray())
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