namespace MichaelKjellander.Models.UtilModels;

public class Url
{
    private readonly string _urlString;
    
    public Url(string urlString)
    {
        _urlString = urlString;
    }
    
    public string GetQueryParameter(string name)
    {
        var uriBuilder = new UriBuilder(_urlString);
        var query = System.Web.HttpUtility.ParseQueryString(uriBuilder.Query);
        return query[name] ?? "";
    }

    public ICollection<string> GetQueryParameters(string name)
    {
        return GetQueryParameter(name).Split(",");
    }

    public ICollection<int> GetQueryParametersInt(string name)
    {
        ICollection<string> stringValues = GetQueryParameters(name);
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

    public string GetQueryString()
    {
        string[] splits = _urlString.Split("?");
        return splits.Length == 2 ? "?" + splits[1] : "";
    }
}