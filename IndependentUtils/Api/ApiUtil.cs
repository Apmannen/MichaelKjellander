using System.Text;
using System.Text.Json;

namespace MichaelKjellander.IndependentUtils.Api;

//TODO: methods will probably be moved to an abstract class that API services use
public static class ApiUtil
{
    public static async Task<JsonFetchResult> FetchJson(string url, HttpClient client)
    {
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);
        return await FetchJsonInternal(request, url, client);
    }

    public static async Task<JsonFetchResult> PostFetchJson(string url, HttpClient client, object payload)
    {
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url);
        request.Content = JsonContent.Create(payload);
        return await FetchJsonInternal(request, url, client);
    }

    private static async Task<JsonFetchResult> FetchJsonInternal(HttpRequestMessage request, string url,
        HttpClient client)
    {
        var response = await client.SendAsync(request);
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException("Couldn't fetch: " + url);
        }

        await using var responseStream = await response.Content.ReadAsStreamAsync();
        using var reader = new StreamReader(responseStream);
        string content = await reader.ReadToEndAsync();
        JsonDocument doc = JsonDocument.Parse(content);

        return new JsonFetchResult(root: doc.RootElement, headers: response.Headers);
    }
}