namespace MichaelKjellander.Models.Wordpress;

public abstract class WordpressModel : Model
{
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
}