using System.Text.Json;
using MichaelKjellander.SharedUtils.Json;

namespace MichaelKjellander.Models.Wordpress;

public enum CategoryType
{
    Unknown,
    GameReview,
    Game
}

public class WpCategory : IParsableJson
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Slug { get; set; }
    
    public CategoryType Type
    {
        get
        {
            return Enum.GetValues<CategoryType>().FirstOrDefault(aType => GetSlug(aType) == Slug);
        }
    }
    
    // Some categories need special treatment, just need to identify them in the way that it's currently possible.
    public static string GetSlug(CategoryType categoryType)
    {
        return categoryType switch
        {
            CategoryType.GameReview => "tv-spelrecensioner",
            CategoryType.Game => "spel",
            _ => ""
        };
    }

    
    public void ParseFromJson(JsonElement el)
    {
        this.Id = el.GetProperty("id").GetInt32();
        this.Name = el.GetProperty("name").GetString()!;
        this.Slug = el.GetProperty("slug").GetString();
    }

    
}