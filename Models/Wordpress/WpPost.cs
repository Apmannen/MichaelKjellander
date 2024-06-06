using System.Text.Json;

namespace MichaelKjellander.Models.Wordpress;

public class WpPost : Model
{
    public int Id { get; set; }
    public string? Content { get; set; }
    public string? Title { get; set; }
    public string? Slug { get; set; }
    public DateOnly Date { get; set; }
    public WpCategory? Category { get; set; }
    public string? FeaturedMediaUrl { get; set; }
    public IList<string>? MetaPlatforms { get; set; }
    public string? MetaPlayAlso { get; set; }
    public int? MetaRating { get; set; }
    
    //Yep, I think it's fine to keep texts like these in a single language application.
    //They could easily be swapped otherwise.
    public string RatingText
    {
        get
        {
            return MetaRating switch
            {
                1 => "Ospelbart",
                2 => "Mycket dåligt",
                3 => "Dåligt",
                4 => "Ganska dåligt",
                5 => "Medelmåttigt",
                6 => "Ganska bra",
                7 => "Bra",
                8 => "Mycket bra",
                9 => "Fantastiskt",
                10 => "Mästerverk",
                _ => ""
            };
        }
    }
    
    public override void ParseFromJson(JsonElement el)
    {
        int id = el.GetProperty("ID").GetInt32();
        string content = el.GetProperty("post_content").GetString()!;
        string title = el.GetProperty("post_title").GetString()!;
        string slug = el.GetProperty("post_name").GetString()!;
        //int categoryId = el.GetProperty("category_id").GetInt32();
        string dateString = el.GetProperty("post_date").GetString()!;
        dateString = dateString.Replace(' ', 'T');
        DateOnly date = DateOnly.Parse(dateString);
        string? featuredMediaUrl = el.GetProperty("featured_image").GetString();
        var metaElement = el.GetProperty("meta");
        
        this.Id = id;
        this.Content = content;
        this.Title = title;
        this.Slug = slug;
        this.Date = date;
        this.Category = new WpCategory();
        this.Category.ParseFromJson(el.GetProperty("category"));
        this.FeaturedMediaUrl = featuredMediaUrl;
        this.MetaPlatforms = TryParseStrings(metaElement, "format");
        this.MetaPlayAlso = TryParseString(metaElement, "play_also");
        this.MetaRating = TryParseInt(metaElement, "rating");
    }

    private static string? TryParseString(JsonElement parent, string key)
    {
        bool didSet = parent.TryGetProperty(key, out JsonElement child);
        if (!didSet)
        {
            return null;
        }
        return child.EnumerateArray().FirstOrDefault().GetString();
    }
    private static List<string> TryParseStrings(JsonElement parent, string key)
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
    private static int? TryParseInt(JsonElement parent, string key)
    {
        string? parsedString = TryParseString(parent, key);
        if (parsedString == null)
        {
            return null;
        }

        return int.Parse(parsedString);
    }
}
