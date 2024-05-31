using Microsoft.Extensions.Options;

namespace MichaelKjellander.SharedUtils.Routes;

public class ApiRoutes
{
    private readonly string _baseUrl;
    public ApiRoutes(string baseUrl)
    {
        _baseUrl = baseUrl;
    }
    
    public string Posts(int page) => $"{_baseUrl}/api/blog/posts?page={page}"; 
}