using System.Text.Json;

namespace MichaelKjellander.Models.Wordpress;

public enum CategoryType
{
    Unknown,
    GameReview,
    Game,
    Other
}

public class WpCategory : WordpressModel
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Slug { get; set; }
    
    public CategoryType Type
    {
        get
        {
            return Enum.GetValues<CategoryType>().FirstOrDefault(aType => GetSlugByType(aType) == Slug);
        }
    }
    
    // Some categories need special treatment, just need to identify them in the way that it's currently possible.
    public static string GetSlugByType(CategoryType categoryType)
    {
        return categoryType switch
        {
            CategoryType.GameReview => "tv-spelrecensioner",
            CategoryType.Game => "spel",
            CategoryType.Other => "okategoriserade",
            _ => ""
        };
    }

    
    public override WpCategory ParseFromJson(JsonElement el)
    {
        this.Id = el.GetProperty("term_id").GetInt32();
        this.Name = el.GetProperty("name").GetString();
        this.Description = HarmonizeHtmlContent(el.GetProperty("category_description").GetString()!);
        this.Slug = el.GetProperty("slug").GetString();
        return this;
    }
}