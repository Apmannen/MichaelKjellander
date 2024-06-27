using MichaelKjellander.Config;
using MichaelKjellander.Domains.ApiResponse;
using MichaelKjellander.Domains.Models.WebGames;
using Microsoft.Extensions.Options;

namespace MichaelKjellander.Views.Services.Api;

public class WebGamesApiService : InternalApiService
{
    public WebGamesApiService(HttpClient client, IOptions<AppConfig> options) : base(client, options) {}
    
    public async Task<WordGuessGameProgress> FetchInitWordGuessGame()
    {
        ApiResponse<WordGuessGameProgress>
            response = await FetchModels<WordGuessGameProgress>(ApiRoutes.WordGuessInit);
        return response.Items!.First();
    }

    public async Task<WordGuessGameProgress> FetchGuessResult(char letter, string gameId)
    {
        ApiResponse<WordGuessGameProgress> response =
            await FetchModels<WordGuessGameProgress>(ApiRoutes.WordGuessGuess(letter, gameId));
        return response.Items!.First();
    }
}