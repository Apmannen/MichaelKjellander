using MichaelKjellander.Models.Wordpress;
using MichaelKjellander.SharedUtils.Api;
using MichaelKjellander.SharedUtils.Json;
using MichaelKjellander.SharedUtils.Routes;

namespace MichaelKjellander.Data;

public class InternalApiContext
{
    private readonly HttpClient _client;

    public InternalApiContext(HttpClient client)
    {
        this._client = client;
    }

    public async Task<ApiResponse<WpPost>> FetchPosts()
    {
        return await Fetch<WpPost>(ApiRoutes.Posts);
    }

    private async Task<ApiResponse<T>> Fetch<T>(string path) where T : IParsableJson
    {
        JsonFetchResult result = await ApiUtil.FetchJson(path, _client);
        ApiResponse<T> response = new ApiResponse<T>();
        response.ParseFromJson(result.Root);
        return response;
    }
}