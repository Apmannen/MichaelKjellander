namespace MichaelKjellander.Api.Features.Posts;

public class PostsRequest
{
    public string? CategorySlug { get; set; }
    public string[]? MetaPlatforms { get; set; }
    public List<int>? MetaRatings { get; set; }
    public int? Page { get; set; }
    public string? Slug { get; set; }
    public List<int>? TagIds { get; set; }
}