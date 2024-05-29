using System.Text.Json;
using MichaelKjellander.Utils;

namespace MichaelKjellander.Models.Wordpress;

public class WpPost : IParsableJson
{
    public int Id { get; private init; }
    public string? Content { get; private init; }
    public string? Title { get; private init; }
    public DateOnly Date { get; private init; }
    public int CategoryId { get; private init; }
    public int FeaturedMediaId { get; private init; }
    public WpCategory? Category { get; private set; }
    public WpMedia? FeaturedMedia { get; private set; }
    public ICollection<int>? TagIds { get; private init;  }
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


    public static IParsableJson ParseFromJson(JsonElement el)
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
        
        return new WpPost
        {
            Id = id, Content = content, Title = title, CategoryId = categoryId, Date = date,
            FeaturedMediaId = featuredMediaId, TagIds = tagIds
        };
    }
}