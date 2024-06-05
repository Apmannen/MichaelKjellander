namespace MichaelKjellander.SharedUtils.Routes;

public class ApiRoutes
{
    private readonly string _baseUrl;
    public ApiRoutes(string baseUrl)
    {
        _baseUrl = baseUrl;
    }
    
    public string Pages(string slug) => $"{_baseUrl}/api/blog/pages?slug={slug}"; 
    //TODO: use some kind of query builder, #16
    public string Posts(int page, string? categorySlug, string? postSlug)
    {
        string url = $"{_baseUrl}/api/blog/posts?page={page}";
        if (!string.IsNullOrEmpty(categorySlug))
        {
            url += $"&categorySlug={categorySlug}"; 
        }
        if (!string.IsNullOrEmpty(postSlug))
        {
            url += $"&slug={postSlug}"; 
        }
        return url;
    }
    
    public string WordGuessInit =>  $"{_baseUrl}/api/webgames/word-guess/init"; 
    public string WordGuessGuess(char letter, string uuid) => $"{_baseUrl}/api/webgames/word-guess/guess?letter={letter}&gameId={uuid}"; 
    
    //public string RandomWord =>  $"{_baseUrl}/api/webgames/random-word"; 
}