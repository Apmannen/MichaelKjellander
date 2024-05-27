using System.Text.Json;

namespace MichaelKjellander.Utils;

public static class ApiUtil
{
    public static async Task<JsonElement> FetchWp(string uri, HttpClient client)
    {
        return await FetchJson("https://michaelkjellander.se/wp-json/wp/v2/" + uri, client);
    }

    public static async Task<JsonElement> FetchJson(string url, HttpClient client)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, url);

        var response = await client.SendAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Couldn't fetch: " + url);
        }

        await using var responseStream = await response.Content.ReadAsStreamAsync();
        using var reader = new StreamReader(responseStream);
        var content = await reader.ReadToEndAsync();
        var doc = JsonDocument.Parse(content);

        return doc.RootElement;
    }
}