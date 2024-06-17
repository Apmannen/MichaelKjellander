using System.Text.Json;
using MichaelKjellander.Models;

namespace MichaelKjellander.SharedUtils.Api;

//TODO: should be a class that's instansiated probably, with client as parameter. See internal api.
public static class ApiUtil
{
    public static async Task<JsonFetchResult> FetchJson(string url, HttpClient client)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        var response = await client.SendAsync(request);
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException ("Couldn't fetch: " + url);
        }

        await using var responseStream = await response.Content.ReadAsStreamAsync();
        using var reader = new StreamReader(responseStream);
        string content = await reader.ReadToEndAsync();
        JsonDocument doc = JsonDocument.Parse(content);

        return new JsonFetchResult(root: doc.RootElement, headers: response.Headers);
    }

    public static ApiResponse<T> CreateSimpleApiResponse<T>(IList<T> items) where T : DbModel
    {
        return CreateApiResponse(items, currentPage:1, totalCount: items.Count, perPage: -1);
    }
    public static ApiResponse<T> CreateApiResponse<T>(IList<T> items, int currentPage = 0, int totalCount = 0, int perPage = 0) where T : DbModel
    {
        int numPages = 1;
        if (perPage >= 0)
        {
            numPages = (int)Math.Ceiling((float)totalCount / (float)perPage);
        }
        return new ApiResponse<T>(items, new PaginationData(currentPage, numPages, items.Count, totalCount));
    }
}