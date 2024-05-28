using MichaelKjellander.Models.Wordpress;

namespace MichaelKjellander.Utils;

public static class ModelUtil
{
    [System.Obsolete("Replace with full get posts method")]
    public static void MapCategoriesToPosts(ICollection<WpApiCategory> categories, ICollection<WpApiPost> posts)
    {
        foreach (var post in posts)
        {
            post.FindAndSetCategory(categories);
        }
    }
}