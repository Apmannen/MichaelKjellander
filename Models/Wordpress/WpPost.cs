using System.Text.Json;

namespace MichaelKjellander.Models.Wordpress;

public class WpPost : Model
{
    public int Id { get; set; }
    public string? Content { get; set; }
    public string? Title { get; set; }
    public string? Slug { get; set; }
    public DateOnly Date { get; set; }
    public int CategoryId { get; set; }
    public int FeaturedMediaId { get; set; }
    public WpCategory? Category { get; set; }
    public WpMedia? FeaturedMedia { get; set; }
    public IList<int>? TagIds { get; set;  }
    public IList<WpTag>? Tags { get; set;  }
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

    public void FindAndSetCategory(ICollection<WpCategory> categories)
    {
        this.Category = categories.First(c => c.Id == this.CategoryId);
    }
    public void FindAndSetFeaturedMedia(ICollection<WpMedia> medias)
    {
        this.FeaturedMedia = medias.FirstOrDefault(m => m.Id == this.FeaturedMediaId);
    }
    public void FindAndSetTags(ICollection<WpTag> tags)
    {
        this.Tags = tags.Where(t => this.TagIds!.Contains(t.Id)).ToArray();
    }
    
    public override void ParseFromJson(JsonElement el)
    {
        int id = el.GetProperty("id").GetInt32();
        string content = el.GetProperty("content").GetProperty("rendered").GetString()!;
        string title = el.GetProperty("title").GetProperty("rendered").GetString()!;
        string slug = el.GetProperty("slug").GetString()!;
        int categoryId = el.GetProperty("categories").EnumerateArray().FirstOrDefault().GetInt32();
        string dateString = el.GetProperty("date").GetString()!;
        DateOnly date = DateOnly.Parse(dateString);
        int featuredMediaId = el.GetProperty("featured_media").GetInt32();
        var tagIdsEnumerator = el.GetProperty("tags").EnumerateArray();
        List<int> tagIds = new List<int>();
        foreach(JsonElement tagEl in tagIdsEnumerator)
        {
            tagIds.Add(tagEl.GetInt32());
        }
        var metaElement = el.GetProperty("metadata");
        
        this.Id = id;
        this.Content = content;
        this.Title = title;
        this.Slug = slug;
        this.Date = date;
        this.CategoryId = categoryId;
        this.FeaturedMediaId = featuredMediaId;
        this.TagIds = tagIds;
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
