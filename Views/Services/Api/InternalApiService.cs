using MichaelKjellander.Config;
using MichaelKjellander.Domains.ApiResponse;
using MichaelKjellander.Domains.Models;
using MichaelKjellander.IndependentUtils.Api;
using MichaelKjellander.IndependentUtils.Parsers.Json;
using MichaelKjellander.Views.Routes;
using Microsoft.Extensions.Options;

namespace MichaelKjellander.Views.Services.Api;

public abstract class InternalApiService
{
    private readonly HttpClient _client;
    protected ApiRoutes ApiRoutes { get; private init; }

    public InternalApiService(HttpClient client, IOptions<AppConfig> options)
    {
        this._client = client;
        this.ApiRoutes = new ApiRoutes(options.Value.SiteUrl!);
    }

    protected async Task<ApiResponse<T>> FetchModels<T>(string path) where T : DbModel
    {
        JsonFetchResult result = await ApiUtil.FetchJson(path, _client);
        ApiResponse<T> response = JsonParser.ParseParsableJson<ApiResponse<T>>(result.Root);
        return response;
    }

    protected async Task<ApiResponse<T>> PostFetchGeneric<T>(string path, object payload)
    {
        JsonFetchResult result = await ApiUtil.PostFetchJson(path, _client, payload);
        ApiResponse<T> response = JsonParser.ParseParsableJson<ApiResponse<T>>(result.Root);
        return response;
    }
}