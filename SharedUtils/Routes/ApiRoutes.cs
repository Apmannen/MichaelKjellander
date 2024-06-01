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
    public string Posts(int page) => $"{_baseUrl}/api/blog/posts?page={page}"; 
}