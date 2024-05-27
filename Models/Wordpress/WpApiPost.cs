using System.Text.Json;
using MichaelKjellander.Utils;

namespace MichaelKjellander.Models.Wordpress;

public class WpApiPost : IParsableJson
{
    public int Id { get; private init; }
    public string Content { get; private init; }
    public string? Title { get; private init; }
    public int CategoryId { get; private init; }
    public WpApiCategory? Category { get; private set; }

    public void FindAndSetCategory(ICollection<WpApiCategory> categories)
    {
        this.Category = categories.First(c => c.Id == this.CategoryId);
    }
    

    public static IParsableJson ParseFromJson(JsonElement el)
    {
        var id = el.GetProperty("id").GetInt32();
        var content = el.GetProperty("content").GetProperty("rendered").GetString();
        var title = el.GetProperty("title").GetProperty("rendered").GetString();
        var categoryId = el.GetProperty("categories").EnumerateArray().FirstOrDefault().GetInt32();

        return new WpApiPost{Id = id, Content = content, Title = title, CategoryId = categoryId};
    }
}

