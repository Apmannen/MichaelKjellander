using System.Text.Json;
using MichaelKjellander.Utils;

namespace MichaelKjellander.Models.Wordpress;

public class WpApiPost : IParsableJson
{
    public int Id { get; private init; }
    public string Content { get; private init; }
    public string? Title { get; private init; }
    public DateOnly Date { get; private init; }
    public int CategoryId { get; private init; }
    public int FeaturedMediaId { get; private init; }
    public WpApiCategory? Category { get; private set; }
    public WpApiMedia? FeaturedMedia { get; private set; }

    public void FindAndSetCategory(ICollection<WpApiCategory> categories)
    {
        this.Category = categories.First(c => c.Id == this.CategoryId);
    }
    public void FindAndSetFeaturedMedia(ICollection<WpApiMedia> medias)
    {
        this.FeaturedMedia = medias.FirstOrDefault(m => m.Id == this.FeaturedMediaId);
    }


    public static IParsableJson ParseFromJson(JsonElement el)
    {
        var id = el.GetProperty("id").GetInt32();
        var content = el.GetProperty("content").GetProperty("rendered").GetString();
        var title = el.GetProperty("title").GetProperty("rendered").GetString();
        var categoryId = el.GetProperty("categories").EnumerateArray().FirstOrDefault().GetInt32();
        var dateString = el.GetProperty("date").GetString();
        var date = DateOnly.Parse(dateString);
        var featuredMediaId = el.GetProperty("featured_media").GetInt32();

        return new WpApiPost
        {
            Id = id, Content = content, Title = title, CategoryId = categoryId, Date = date,
            FeaturedMediaId = featuredMediaId
        };
    }
}