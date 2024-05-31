namespace MichaelKjellander.SharedUtils.Routes;

public static class PageRoutes
{
    public const string Home = "/";

    public static string Category(string slug)
    {
        return "/kategori/" + slug;
    }

    //public static string LatestPosts(int page)
    //{
    //    return "/" + (page > 1 ? $"/sida/{page}");
    //}

    public const string Contact = "/kontakt";
}