using System.Text.Json;
using MichaelKjellander.SharedUtils;

namespace MichaelKjellander.Models.Wordpress;

public class WpPost : IParsableJson
{
    public int Id { get; private set; }
    public string? Content { get; private set; }
    public string? Title { get; private set; }
    public DateOnly Date { get; private set; }
    public int CategoryId { get; private set; }
    public int FeaturedMediaId { get; private set; }
    public WpCategory? Category { get; private set; }
    public WpMedia? FeaturedMedia { get; private set; }
    public ICollection<int>? TagIds { get; private set;  }
    public ICollection<WpTag>? Tags { get; private set;  }

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

        this.Id = id;
        this.Content = content;
        this.Title = title;
        this.Date = date;
        this.CategoryId = categoryId;
        this.FeaturedMediaId = featuredMediaId;
        this.TagIds = tagIds;
    }
}