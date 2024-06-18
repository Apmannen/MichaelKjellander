using System.Text.Json;
using MichaelKjellander.IndependentUtils.Parsers.Json;

namespace MichaelKjellander.Models.Wordpress;

public abstract class WordpressModel : DbModel, IParsableJson
{
    public abstract IParsableJson ParseFromJson(JsonElement el);
    
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
        html = html.Replace("\n", "");
        return html;
    }
    
    protected static string? TryParseString(JsonElement parent, string key)
    {
        bool didSet = parent.TryGetProperty(key, out JsonElement child);
        if (!didSet)
        {
            return null;
        }

        return child.EnumerateArray().FirstOrDefault().GetString();
    }

    protected static List<string> TryParseStrings(JsonElement parent, string key)
    {
        bool didSet = parent.TryGetProperty(key, out JsonElement child);
        if (!didSet)
        {
            return [];
        }

        List<string> strings = [];
        foreach (JsonElement el in child.EnumerateArray())
        {
            strings.Add(el.GetString()!);
        }

        return strings;
    }

    protected static int? TryParseInt(JsonElement parent, string key)
    {
        string? parsedString = TryParseString(parent, key);
        if (parsedString == null)
        {
            return null;
        }

        return int.Parse(parsedString);
    }
}