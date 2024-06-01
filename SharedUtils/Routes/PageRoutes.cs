using MichaelKjellander.Models.Wordpress;

namespace MichaelKjellander.SharedUtils.Routes;

public static class PageRoutes
{
    public const string Home = "/";

    public static string Category(string slug)
    {
        return "/kategori/" + slug;
    }

    public static string Category(CategoryType type)
    {
        return Category(WpCategory.GetSlug(type));
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
}