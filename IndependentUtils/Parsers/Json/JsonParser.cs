using System.Text.Json;
using Newtonsoft.Json;

namespace MichaelKjellander.IndependentUtils.Parsers.Json;

public static class JsonParser
{
    private static T DeserializeObject<T>(JsonElement jsonElement)
    {
        T deserialized = JsonConvert.DeserializeObject<T>(jsonElement.ToString())!;
        return deserialized;
    }

    public static List<T> DeserializeObjectCollection<T>(JsonElement.ArrayEnumerator collectionElement)
    {
        return collectionElement.Select(item => DeserializeObject<T>(item)).ToList();
    }
    
    public static T ParseParsableJson<T>(JsonElement jsonElement) where T : IParsableJson
    {
        T parsableJson = CreateParsableJson<T>();
        parsableJson.ParseFromJson(jsonElement);
        return parsableJson;
    }

    public static List<T> ParseParsableJsonCollection<T>(JsonElement.ArrayEnumerator collectionElement) where T : IParsableJson
    {
        return collectionElement.Select(el => ParseParsableJson<T>(el)).ToList();
    }
    
    private static T CreateParsableJson<T>() where T : IParsableJson
    {
        return Activator.CreateInstance<T>();
    }
}