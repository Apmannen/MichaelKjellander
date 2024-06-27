using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace MichaelKjellander.Domains.Models.Wordpress;

public enum CategoryType
{
    Unknown,
    GameReview,
    Game,
    Other
}

[Table("wp_categories")]
public class WpCategory : WordpressModel
{
    [Required]
    [MaxLength(VarcharLength)]
    public string? Name { get; set; }
    [Required]
    public string? Description { get; set; }
    [Required]
    [MaxLength(VarcharLength)]
    public string? Slug { get; set; }
    
    public CategoryType Type => Enum.GetValues<CategoryType>().FirstOrDefault(aType => GetSlugByType(aType) == Slug);
    public string TypeString => Type.ToString();
    
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
        this.Id = el.GetProperty("id").GetInt32();
        this.Name = el.GetProperty("name").GetString();
        this.Description = HarmonizeHtmlContent(el.GetProperty("description").GetString()!);
        this.Slug = el.GetProperty("slug").GetString();
        return this;
    }
}