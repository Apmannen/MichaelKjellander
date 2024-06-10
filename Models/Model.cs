using System.Text.Json;

namespace MichaelKjellander.Models;

public abstract class Model
{
    /// <summary>
    /// Intended for external APIs, not all models need it. Since not all models need it,
    /// maybe use an interface or a subclass?
    /// </summary>
    /// <param name="el"></param>
    /// <exception cref="NotImplementedException"></exception>
    public virtual Model ParseFromJson(JsonElement el)
    {
        throw new NotImplementedException();
    }

    public static T ParseNewFromJson<T>(JsonElement el) where T : Model
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