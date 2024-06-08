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

    public static ApiResponse<T> CreateApiResponse<T>(IList<T> items)
    {
        return CreateApiResponse(items, 1, 1);
    }
    public static ApiResponse<T> CreateApiResponse<T>(IList<T> items, int currentPage, int numPages)
    {
        return new ApiResponse<T>(items, new PaginationData(currentPage, numPages, items.Count));
    }
}