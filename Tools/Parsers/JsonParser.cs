using System.Text.Json;
using MichaelKjellander.Models;
using Newtonsoft.Json;

namespace MichaelKjellander.Tools.Parsers;

public static class JsonParser
{
    private static T DeserializeObject<T>(JsonElement jsonElement)
    {
        T deserialized = JsonConvert.DeserializeObject<T>(jsonElement.ToString())!;
        return deserialized;
    }

    public static List<T> DeserializeObjectCollection<T>(JsonElement.ArrayEnumerator collectionElement)
    {
        List<T> deserializedList = new List<T>();
        foreach (JsonElement item in collectionElement)
        {
            T deserialized = DeserializeObject<T>(item);
            deserializedList.Add(deserialized);
        }
        return deserializedList;
    }
    
    private static T ParseParsableJson<T>(JsonElement jsonElement) where T : IParsableJson
    {
        T parsableJson = CreateModelInstance<T>();
        parsableJson.ParseFromJson(jsonElement);
        return parsableJson;
    }

    public static List<T> ParseParsableJsonCollection<T>(JsonElement.ArrayEnumerator collectionElement) where T : IParsableJson
    {
        List<T> list = [];
        foreach (JsonElement el in collectionElement)
        {
            list.Add(ParseParsableJson<T>(el));
        }
        return list;
    }
    
    private static T CreateModelInstance<T>()
    {
        return Activator.CreateInstance<T>();
    }
}