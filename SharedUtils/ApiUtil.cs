using System.Net.Http.Headers;
using System.Text.Json;
using MichaelKjellander.Models.Wordpress;

namespace MichaelKjellander.SharedUtils;

public static class ApiUtil
{
    /*public static async Task<JsonElement> FetchWp(string uri, HttpClient client)
    {
        return await FetchJson("https://michaelkjellander.se/wp-json/wp/v2/" + uri, client);
    }*/
    
    public static async Task<JsonFetchResult> FetchJson(string url, HttpClient client)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, url);

        var response = await client.SendAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Couldn't fetch: " + url);
        }

        await using var responseStream = await response.Content.ReadAsStreamAsync();
        using var reader = new StreamReader(responseStream);
        string content = await reader.ReadToEndAsync();
        JsonDocument doc = JsonDocument.Parse(content);

        List<IParsableJson> apa = [];

        return new JsonFetchResult(root: doc.RootElement, headers: response.Headers);
    }
    
    /*public interface IInterface
    {
        public static void Test();
    }*/

    public static ApiResponse<T> CreateApiResponse<T>(ICollection<T> items, int currentPage, int numPages) where T : IParsableJson
    {
        return new ApiResponse<T>(items, new PaginationData(currentPage, numPages));
    }

    public struct JsonFetchResult(JsonElement root, HttpResponseHeaders headers)
    {
        public readonly JsonElement Root = root;
        public readonly HttpResponseHeaders Headers = headers;
    }

    public class ApiResponse<T>(ICollection<T> items, PaginationData paginationData) where T : IParsableJson
    {
        public readonly ICollection<T> Items = items;
        public readonly PaginationData PaginationData = paginationData;
    }

    public struct PaginationData(int currentPage, int numPages)
    {
        public readonly int CurrentPage = currentPage;
        public readonly int NumPages = numPages;
    }
}