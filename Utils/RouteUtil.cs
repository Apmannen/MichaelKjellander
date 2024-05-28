namespace MichaelKjellander.Utils;

public static class RouteUtil
{
    public static readonly string pageHome = "/";

    public static string pageCategory(string slug)
    {
        return "/katergori/" + slug;
    }

    public static readonly string pageContact = "/kontakt";
}