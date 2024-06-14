using System.Text.Json;

namespace MichaelKjellander.Models;

public interface IParsableJson
{
    public IParsableJson ParseFromJson(JsonElement el);

    public static T ParseNewFromJson<T>(JsonElement el) where T : IParsableJson
    {
        T parsableJson = CreateModelInstance<T>();
        parsableJson.ParseFromJson(el);
        return parsableJson;
    }

    private static T CreateModelInstance<T>()
    {
        return Activator.CreateInstance<T>();
    }
}