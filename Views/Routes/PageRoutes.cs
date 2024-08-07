using MichaelKjellander.Domains.Models.Wordpress;

namespace MichaelKjellander.Views.Routes;

public static class PageRoutes
{
    public const string Home = "/";

    public static string Category(string slug)
    {
        return "/kategori/" + slug;
    }

    public static string Category(CategoryType type)
    {
        return Category(WpCategory.GetSlugByType(type));
    }

    public static string Tag(CategoryType type, WpTag tag)
    {
        return $"{Category(type)}?tagg={tag.Id}";
    }

    public static string CategoryPattern(CategoryType type)
    {
        string path = Category(type);
        return $"{path}|{path}/[0-9]+";
    }

    public static string Page(string slug)
    {
        return "/sida/" + slug;
    }
    
    public static string Post(string slug)
    {
        return "/inlagg/" + slug;
    }
}