namespace MichaelKjellander.Utils;

public static class RouteUtil
{
    public static readonly string PageHome = "/";

    public static string PageCategory(string slug)
    {
        return "/kategori/" + slug;
    }

    public static readonly string PageContact = "/kontakt";
}