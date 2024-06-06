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
    
    public static string GetQueryParameter(NavigationManager navigationManager, string name)
    {
        var uriBuilder = new UriBuilder(navigationManager.Uri);
        var query = System.Web.HttpUtility.ParseQueryString(uriBuilder.Query);
        return query[name] ?? "";
    }
    
    public static ICollection<string> GetQueryParameters(NavigationManager navigationManager, string name)
    {
        return GetQueryParameter(navigationManager, name).Split(",");
    }
    public static ICollection<int> GetQueryParametersInt(NavigationManager navigationManager, string name)
    {
        ICollection<string> stringValues = GetQueryParameter(navigationManager, name).Split(",");
        List<int> intValues = new();
        foreach (string stringValue in stringValues)
        {
            bool isValid = int.TryParse(stringValue, out int parsedValue);
            if (isValid)
            {
                intValues.Add(parsedValue);
            }
        }
        return intValues;
    }
}