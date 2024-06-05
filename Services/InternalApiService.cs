using MichaelKjellander.Models;
using MichaelKjellander.Models.WebGames;
using MichaelKjellander.Models.Wordpress;
using MichaelKjellander.SharedUtils;
using MichaelKjellander.SharedUtils.Api;
using MichaelKjellander.SharedUtils.Routes;
using Microsoft.Extensions.Options;

namespace MichaelKjellander.Services;

public class InternalApiService
{
    private readonly HttpClient _client;
    private readonly ApiRoutes _apiRoutes;

    public InternalApiService(HttpClient client, IOptions<AppConfig> options)
    {
        this._client = client;
        this._apiRoutes = new ApiRoutes(options.Value.SiteUrl!);
    }

    public async Task<ApiResponse<WpPage>> FetchPages(string slug = "")
    {
        return await Fetch<WpPage>(_apiRoutes.Pages(slug));
    }
    public async Task<ApiResponse<WpPost>> FetchPosts(int pageNumber = 1, string? categorySlug = null, string? postSlug = null)
    {
        return await Fetch<WpPost>(_apiRoutes.Posts(pageNumber, categorySlug, postSlug));
    }

    /*public async Task<string> FetchRandomWord()
    {
        ApiResponse<Word> response = await Fetch<Word>(_apiRoutes.RandomWord);
        return response.Items.FirstOrDefault().WordString;
    }*/

    public async Task<WordGuessGameProgress> FetchInitWordGuessGame()
    {
        ApiResponse<WordGuessGameProgress> response = await Fetch<WordGuessGameProgress>(_apiRoutes.WordGuessInit);
        return response.Items!.FirstOrDefault()!;
    }
    public async Task<WordGuessGameProgress> FetchGuessResult(char letter, string gameId)
    {
        ApiResponse<WordGuessGameProgress> response = await Fetch<WordGuessGameProgress>(_apiRoutes.WordGuessGuess(letter, gameId));
        return response.Items!.FirstOrDefault()!;
    }

    private async Task<ApiResponse<T>> Fetch<T>(string path) where T : Model
    {
        JsonFetchResult result = await ApiUtil.FetchJson(path, _client);
        ApiResponse<T> response = new ApiResponse<T>();
        response.ParseFromJson(result.Root);
        return response;
    }
}