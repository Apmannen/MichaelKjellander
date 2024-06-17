using MichaelKjellander.Config;
using MichaelKjellander.Models;
using MichaelKjellander.Models.WebGames;
using MichaelKjellander.Models.Wordpress;
using MichaelKjellander.SharedUtils.Api;
using MichaelKjellander.SharedUtils.Routes;
using Microsoft.Extensions.Options;

namespace MichaelKjellander.Communicators;

public class InternalApiCommunicator
{
    private readonly HttpClient _client;
    private readonly ApiRoutes _apiRoutes;

    public InternalApiCommunicator(HttpClient client, IOptions<AppConfig> options)
    {
        this._client = client;
        this._apiRoutes = new ApiRoutes(options.Value.SiteUrl!);
    }

    //Publics
    public async Task<IList<WpCategory>> FetchCategories()
    {
        ApiResponse<WpCategory> response = await FetchModels<WpCategory>(_apiRoutes.Categories);
        return response.Items!;
    }

    public async Task<IList<WpTag>> FetchTags(string categorySlug)
    {
        ApiResponse<WpTag> response = await FetchModels<WpTag>(_apiRoutes.Tags(categorySlug));
        return response.Items!;
    }

    public async Task<ApiResponse<WpPage>> FetchPages(string slug = "")
    {
        return await FetchModels<WpPage>(_apiRoutes.Pages(slug));
    }

    public async Task<ApiResponse<WpPost>> FetchPosts(int pageNumber = 1, string? categorySlug = null,
        ICollection<int>? tagIds = null, ICollection<int>? metaRatings = null, string? postSlug = null)
    {
        return await FetchModels<WpPost>(_apiRoutes.Posts(pageNumber, categorySlug, tagIds, metaRatings,
            postSlug));
    }

    public async Task<WordGuessGameProgress> FetchInitWordGuessGame()
    {
        ApiResponse<WordGuessGameProgress>
            response = await FetchModels<WordGuessGameProgress>(_apiRoutes.WordGuessInit);
        return response.Items!.First();
    }

    public async Task<WordGuessGameProgress> FetchGuessResult(char letter, string gameId)
    {
        ApiResponse<WordGuessGameProgress> response =
            await FetchModels<WordGuessGameProgress>(_apiRoutes.WordGuessGuess(letter, gameId));
        return response.Items!.First();
    }

    //Privates
    private async Task<ApiResponse<T>> FetchModels<T>(string path)
    {
        JsonFetchResult result = await ApiUtil.FetchJson(path, _client);
        ApiResponse<T> response = new();
        response.ParseFromJson(result.Root);
        return response;
    }

    private async Task<ApiResponse<string>> FetchStrings(string path)
    {
        JsonFetchResult result = await ApiUtil.FetchJson(path, _client);
        ApiResponse<string> response = new();
        response.ParseStringsFromJson(result.Root);
        return response;
    }
}