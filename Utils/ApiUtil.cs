using System.Text.Json;

namespace MichaelKjellander.Utils;

public static class ApiUtil
{
    public static async Task<JsonElement> FetchWp(string uri, HttpClient client)
    {
        return await FetchJson("https://michaelkjellander.se/wp-json/wp/v2/"+uri, client);
    }
    
    public static async Task<JsonElement> FetchJson(string url, HttpClient client)
    {
        var request = new HttpRequestMessage(HttpMethod.Get,
            "https://michaelkjellander.se/wp-json/wp/v2/posts?per_page=1");

        var response = await client.SendAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Couldn't fetch: " + url);
        }
        await using var responseStream = await response.Content.ReadAsStreamAsync();
        using var reader = new StreamReader(responseStream);
        var content = await reader.ReadToEndAsync();
        using var doc = JsonDocument.Parse(content);
        var root = doc.RootElement;

        return root;

    }
}