using System.Text.Json;

namespace MichaelKjellander.Models;

public abstract class Model
{
    /// <summary>
    /// Intended for external APIs, not all models need it
    /// </summary>
    /// <param name="el"></param>
    /// <exception cref="NotImplementedException"></exception>
    public virtual void ParseFromJson(JsonElement el)
    {
        throw new NotImplementedException();
    }
    
    public static List<T> ParseList<T>(JsonElement root) where T : Model 
    {
        List<T> list = [];
        foreach (var el in root.EnumerateArray())
        {
            T parsableJson = CreateModelInstance<T>();
            parsableJson.ParseFromJson(el);
            list.Add(parsableJson);
        }
        return list;
    }

    private static T CreateModelInstance<T>()
    {
        return Activator.CreateInstance<T>();
    }
}