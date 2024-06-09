namespace MichaelKjellander.SharedUtils.Components;

public static class ComponentUtil
{
    public static string FormatPageTitle(string? pageName, int? pagingPage = null)
    {
        string title = "";
        if (pageName != null)
        {
            title += $"{pageName} - ";
        }
        if (pagingPage != null)
        {
            title += $"Sida {pagingPage} - ";
        }

        title += "Michael Kjellander";
        
        return title;
    }
}