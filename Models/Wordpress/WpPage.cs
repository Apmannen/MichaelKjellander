using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace MichaelKjellander.Models.Wordpress;

public enum PageType
{
    Unknown,
    Welcome,
    Contact
}

[Table("wp_pages")]
public class WpPage : WordpressModel
{
    [Required]
    // ReSharper disable once EntityFramework.ModelValidation.UnlimitedStringLength
    public string? Content { get; set; }

    [Required] [MaxLength(VarcharLength)] public string? Title { get; set; }
    [Required] public DateOnly Date { get; set; }
    [Required] [MaxLength(VarcharLength)] public string? Slug { get; set; }
    [Required] [MaxLength(VarcharLength)] public string? MetaIdName { get; set; }

    public PageType PageType
    {
        get
        {
            return MetaIdName switch
            {
                "contact" => PageType.Contact,
                "welcome" => PageType.Welcome,
                _ => PageType.Unknown
            };
        }
    }

    public override WpPage ParseFromJson(JsonElement el)
    {
        int id = el.GetProperty("id").GetInt32();
        string content = el.GetProperty("content").GetProperty("rendered").GetString()!;
        string title = el.GetProperty("title").GetProperty("rendered").GetString()!;
        string slug = el.GetProperty("slug").GetString()!;
        JsonElement meta = el.GetProperty("metadata");
        string dateString = el.GetProperty("date").GetString()!;
        DateOnly date = DateOnly.Parse(dateString);

        this.Id = id;
        this.Content = content;
        this.Title = title;
        this.Date = date;
        this.Slug = slug;
        this.MetaIdName = TryParseString(meta, "id_name") ?? "unknown";

        return this;
    }
}