namespace MichaelKjellander.SharedUtils.Routes;

public static class PageRoutes
{
    public static readonly string Home = "/";

    public static string Category(string slug)
    {
        return "/kategori/" + slug;
    }

    public static readonly string Contact = "/kontakt";
}