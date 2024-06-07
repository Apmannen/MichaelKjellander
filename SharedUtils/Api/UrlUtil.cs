﻿namespace MichaelKjellander.SharedUtils.Api;

public class UrlUtil
{
    public static string GetQueryParameter(string url, string name)
    {
        var uriBuilder = new UriBuilder(url);
        var query = System.Web.HttpUtility.ParseQueryString(uriBuilder.Query);
        return query[name] ?? "";
    }

    public static ICollection<string> GetQueryParameters(string url, string name)
    {
        return GetQueryParameter(url, name).Split(",");
    }

    public static ICollection<int> GetQueryParametersInt(string url, string name)
    {
        ICollection<string> stringValues = GetQueryParameters(url, name);
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

    public static string GetQueryString(string url)
    {
        string[] splits = url.Split("?");
        return splits.Length == 2 ? "?" + splits[1] : "";
    }
}