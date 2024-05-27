using MichaelKjellander.Models.Wordpress;

namespace MichaelKjellander.Utils;

public class ModelUtil
{
    public static void MapCategoriesToPosts(ICollection<WpApiCategory> categories, ICollection<WpApiPost> posts)
    {
        foreach (var post in posts)
        {
            post.FindAndSetCategory(categories);
        }
    }
}