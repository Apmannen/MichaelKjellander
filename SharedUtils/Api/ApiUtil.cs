using System.Net.Http.Headers;
using System.Text.Json;

namespace MichaelKjellander.SharedUtils.Api;

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

    public static ApiResponse<T> CreateApiResponse<T>(ICollection<T> items, int currentPage, int numPages) where T : IParsableJson
    {
        return new ApiResponse<T>(items, new PaginationData(currentPage, numPages));
    }
    
    public class JsonFetchResult(JsonElement root, HttpResponseHeaders headers)
    {
        public readonly JsonElement Root = root;
        public readonly HttpResponseHeaders Headers = headers;
    }
}