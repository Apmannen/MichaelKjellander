using MichaelKjellander.Domains.Models.Wordpress;
using MichaelKjellander.Tools.Url;

namespace MichaelKjellander.Views.Routes;

public class ApiRoutes
{
    private readonly string _baseUrl;

    public ApiRoutes(string baseUrl)
    {
        _baseUrl = baseUrl;
    }

    public string Categories => $"{_baseUrl}/api/blog/categories";
    
    public string Pages(PageIdentifier? identifier, string? slug)
    {
        return new UrlBuilder($"{_baseUrl}/api/blog/pages", QueryArrayMode.Multiple)
            .AddParam("slug", slug)
            .AddParam("pageIdentifier", identifier != null ? identifier.ToString() : null)
            .ToString();
    }

    public string Posts(int page, string? categorySlug, ICollection<int>? tagIds,
        ICollection<int>? metaRatings, string? postSlug)
    {
        return new UrlBuilder($"{_baseUrl}/api/blog/posts", QueryArrayMode.Multiple)
            .AddParam("categorySlug", categorySlug)
            .AddParam("tagIds", tagIds)
            .AddParam("metaRatings", metaRatings)
            .AddParam("page", page)
            .AddParam("slug", postSlug)
            .ToString();
    }
    
    public string SudokuSolve => $"{_baseUrl}/api/tools/sudoku/solve";

    public string Tags(string categorySlug) => $"{_baseUrl}/api/blog/tags?categorySlug={categorySlug}";
    public string Translations => $"{_baseUrl}/api/blog/translations";
    public string WordGuessInit => $"{_baseUrl}/api/webgames/word-guess/init";

    public string WordGuessGuess(char letter, string uuid) =>
        $"{_baseUrl}/api/webgames/word-guess/guess?letter={letter}&gameId={uuid}";
}