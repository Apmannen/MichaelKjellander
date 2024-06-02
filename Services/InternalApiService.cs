using MichaelKjellander.Models;
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
    public async Task<ApiResponse<WpPost>> FetchPosts(int pageNumber = 1, string? categorySlug = null)
    {
        return await Fetch<WpPost>(_apiRoutes.Posts(pageNumber, categorySlug));
    }

    public async Task<string> FetchRandomWord()
    {
        return null;
    }

    private async Task<ApiResponse<T>> Fetch<T>(string path) where T : Model
    {
        JsonFetchResult result = await ApiUtil.FetchJson(path, _client);
        ApiResponse<T> response = new ApiResponse<T>();
        response.ParseFromJson(result.Root);
        return response;
    }
}