using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using MichaelKjellander.Scripts.Startup.Generated;

namespace MichaelKjellander.Domains.Models.Wordpress;

[Table("wp_posts")]
public class WpPost : WordpressModel
{
    [Required] public string? Content { get; set; }
    [Required] [MaxLength(VarcharLength)] public string? Title { get; set; }
    [Required] [MaxLength(VarcharLength)] public string? Slug { get; set; }
    [Required] public DateOnly Date { get; set; }
    [Required] public int? CategoryId { get; set; }
    [Required] public virtual WpCategory? Category { get; set; }
    public int? FeaturedImageId { get; set; }
    public virtual WpImage? FeaturedImage { get; set; }
    [System.Obsolete("Replace with tags")] public IList<string>? MetaPlatforms { get; set; }
    [MaxLength(VarcharLength)] public string? MetaPlayAlso { get; set; }
    public int? MetaRating { get; set; }
    public IList<WpPostTag> PostTags { get; set; }
    

    public override WpPost ParseFromJson(JsonElement el)
    {
        int id = el.GetProperty("ID").GetInt32();
        string content = el.GetProperty("post_content").GetString()!;
        string title = el.GetProperty("post_title").GetString()!;
        string slug = el.GetProperty("post_name").GetString()!;
        string dateString = el.GetProperty("post_date").GetString()!;
        dateString = dateString.Replace(' ', 'T');
        DateOnly date = DateOnly.Parse(dateString);
        var metaElement = el.GetProperty("meta");
        WpImage featuredImage = new WpImage().ParseFromJson(el.GetProperty("featured_image"));
        WpCategory category = new WpCategory().ParseFromJson(el.GetProperty("category"));
        var tags = el.GetProperty("tags").EnumerateArray();
        //IList<int> tagIds = [];
        IList<WpPostTag> wpPostTags = [];
        foreach (JsonElement tagEl in tags)
        {
            WpTag tag = new WpTag().ParseFromJson(tagEl);
            //tagIds.Add(tag.Id);

            WpPostTag postTag = new WpPostTag
            {
                TagId = tag.Id,
                PostId = id
            };
            wpPostTags.Add(postTag);
        }


        this.Id = id;
        this.Content = HarmonizeHtmlContent(content);
        this.Title = title;
        this.Slug = slug;
        this.Date = date;
        this.Category = new WpCategory().ParseFromJson(el.GetProperty("category"));
        this.CategoryId = category.Id;
        this.FeaturedImage = featuredImage.IsSet ? featuredImage : null;
        this.FeaturedImageId = featuredImage.IsSet ? featuredImage.Id : null;
        this.MetaPlatforms = TryParseStrings(metaElement, "format");
        this.MetaPlayAlso = TryParseString(metaElement, "play_also");
        this.MetaRating = TryParseInt(metaElement, "rating");
        this.PostTags = wpPostTags;
        //this.TagIds = tagIds;

        return this;
    }
}