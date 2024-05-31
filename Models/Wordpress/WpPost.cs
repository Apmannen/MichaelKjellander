using System.Text.Json;
using MichaelKjellander.SharedUtils.Json;

namespace MichaelKjellander.Models.Wordpress;

public class WpPost : IParsableJson
{
    public int Id { get; set; }
    public string? Content { get; set; }
    public string? Title { get; set; }
    public DateOnly Date { get; set; }
    public int CategoryId { get; set; }
    public int FeaturedMediaId { get; set; }
    public WpCategory? Category { get; set; }
    public WpMedia? FeaturedMedia { get; set; }
    public ICollection<int>? TagIds { get; set;  }
    public ICollection<WpTag>? Tags { get; set;  }
    public string? MetaPlatform { get; set; }
    public string? MetaPlayAlso { get; set; }
    public int? MetaRating { get; set; }

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
    
    public void ParseFromJson(JsonElement el)
    {
        int id = el.GetProperty("id").GetInt32();
        string content = el.GetProperty("content").GetProperty("rendered").GetString()!;
        string title = el.GetProperty("title").GetProperty("rendered").GetString()!;
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
        this.Date = date;
        this.CategoryId = categoryId;
        this.FeaturedMediaId = featuredMediaId;
        this.TagIds = tagIds;
        this.MetaPlatform = TryParseString(metaElement, "format");
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