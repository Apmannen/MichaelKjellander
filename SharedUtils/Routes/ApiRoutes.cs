using MichaelKjellander.SharedUtils.Api;

namespace MichaelKjellander.SharedUtils.Routes;

public class ApiRoutes
{
    private readonly string _baseUrl;
    public ApiRoutes(string baseUrl)
    {
        _baseUrl = baseUrl;
    }
    
    public string Pages(string slug) => $"{_baseUrl}/api/blog/pages?slug={slug}"; 
    public string Posts(int page, string? categorySlug, ICollection<int>? metaRatings, string? postSlug)
    {
        return new HttpQueryBuilder($"{_baseUrl}/api/blog/posts", QueryArrayMode.Multiple)
            .Add("categorySlug", categorySlug)
            .Add("metaRatings", metaRatings)
            .Add("page", page)
            .Add("slug", postSlug)
            .ToString();
    }
    
    public string WordGuessInit =>  $"{_baseUrl}/api/webgames/word-guess/init"; 
    public string WordGuessGuess(char letter, string uuid) => $"{_baseUrl}/api/webgames/word-guess/guess?letter={letter}&gameId={uuid}"; 
}