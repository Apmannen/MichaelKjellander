using Microsoft.Extensions.Options;

namespace MichaelKjellander.SharedUtils.Routes;

public class ApiRoutes
{
    private readonly string _baseUrl;
    public ApiRoutes(string baseUrl)
    {
        _baseUrl = baseUrl;
    }
    
    public string Pages(string slug) => $"{_baseUrl}/api/blog/pages?slug={slug}"; 
    //TODO: use some kind of query builder
    public string Posts(int page, string? categorySlug)
    {
        string url = $"{_baseUrl}/api/blog/posts?page={page}";
        if (!string.IsNullOrEmpty(categorySlug))
        {
            url += $"&category_slug={categorySlug}"; 
        }

        return url;
    }
}