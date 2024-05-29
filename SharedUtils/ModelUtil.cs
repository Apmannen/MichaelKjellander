using System.Text.Json;

namespace MichaelKjellander.SharedUtils;

public static class ModelUtil
{
    /*[System.Obsolete("Replace with full get posts method")]
    public static void MapCategoriesToPosts(ICollection<WpCategory> categories, ICollection<WpPost> posts)
    {
        foreach (var post in posts)
        {
            post.FindAndSetCategory(categories);
        }
    }*/
    
    /*public abstract class Model : IParsableJson
    {
        public abstract IParsableJson ParseFromJson(JsonElement el);
    }*/
    
    /*public interface IParsableJson2
    {
        public abstract IParsableJson ParseFromJson(JsonElement el);
    }

    public class ModelImpl : Model
    {
        public override IParsableJson ParseFromJson(JsonElement el)
        {
            throw new NotImplementedException();
        }
    }*/
}