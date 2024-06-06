using Microsoft.AspNetCore.Components;

namespace MichaelKjellander.SharedUtils;

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