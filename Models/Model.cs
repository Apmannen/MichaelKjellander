using System.Text.Json;

namespace MichaelKjellander.Models;

public abstract class Model
{
    /// <summary>
    /// Intended for external APIs, not all models need it
    /// </summary>
    /// <param name="el"></param>
    /// <exception cref="NotImplementedException"></exception>
    public virtual Model ParseFromJson(JsonElement el)
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
    
    /// <summary>
    /// Wordpress seems to be returning different HTML formatting.
    /// </summary>
    /// <returns></returns>
    protected static string HarmonizeHtmlContent(string html)
    {
        if (!html.Contains("<p>"))
        {
            html = "<p>" + html;
            html = html.Replace("\n", "</p><p>");
            html += "</p>";
        }
        return html;
    }

    private static T CreateModelInstance<T>()
    {
        return Activator.CreateInstance<T>();
    }
}