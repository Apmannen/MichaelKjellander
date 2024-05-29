namespace MichaelKjellander.SharedUtils.Routes;

public static class PageRoutes
{
    public const string Home = "/";

    public static string Category(string slug)
    {
        return "/kategori/" + slug;
    }

    public const string Contact = "/kontakt";
}