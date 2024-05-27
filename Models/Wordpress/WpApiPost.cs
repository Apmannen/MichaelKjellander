using System.Text.Json;

namespace MichaelKjellander.Models.Wordpress;

public class WpApiPost
{
    public int Id { get; set; }
    public string Title { get; set; }
}

public class Tmp
{
    //TODO: move
    //TODO: generalize
    public static async Task<List<WpApiPost>> ParsePosts(HttpResponseMessage responseMessage)
    {
        using var responseStream = await responseMessage.Content.ReadAsStreamAsync();
        using StreamReader reader = new StreamReader(responseStream);

        string content = reader.ReadToEnd();

        using JsonDocument doc = JsonDocument.Parse(content);
        JsonElement root = doc.RootElement;

        List<WpApiPost> posts = new();
        foreach (var el in root.EnumerateArray())
        {
            int id = el.GetProperty("id").GetInt32();
            string title = el.GetProperty("title").GetProperty("rendered").GetString();

            WpApiPost post = new WpApiPost { Id = id, Title = title };
            posts.Add(post);
        }

        return posts;
    }
}