using System.Text.Json;
using MichaelKjellander.SharedUtils.Json;

namespace MichaelKjellander.Models.Wordpress;

public enum CategoryType {Unknown, GameReview, Game }
public class WpCategory : IParsableJson
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Slug { get; set; }

    public CategoryType Type
    {
        get
        {
            if (Id == 7 || Slug == "tv-spelrecensioner")
            {
                return CategoryType.GameReview;
            }

            if (Id == 2 || Slug == "mina-spel")
            {
                return CategoryType.Game;
            }

            return CategoryType.Unknown;
        }
    }
    public void ParseFromJson(JsonElement el)
    {
        this.Id = el.GetProperty("id").GetInt32();
        this.Name = el.GetProperty("name").GetString()!;
        this.Slug = el.GetProperty("slug").GetString();
    }
}