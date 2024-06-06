namespace MichaelKjellander.SharedUtils.Api;

public class HttpQueryBuilder
{
    private readonly List<KeyValuePair<string,string>> _entries = new();
    
    public HttpQueryBuilder Add(string key, string? value)
    {
        return AddObject(key, value);
    }
    public HttpQueryBuilder Add(string key, int? value)
    {
        return AddObject(key, value);
    }
    public HttpQueryBuilder Add(string key, int[]? values)
    {
        return AddObjects(key, values);
    }

    private HttpQueryBuilder AddObject(string key, object? value)
    {
        string? stringValue;
        if (value != null && !string.IsNullOrEmpty(stringValue = value.ToString()))
        {
            _entries.Add(new KeyValuePair<string, string>(key, stringValue));
        }
        return this;
    }
    private HttpQueryBuilder AddObjects<T>(string key, T[]? values)
    {
        if (values == null || values.Length == 0)
        {
            return this;
        }
        foreach (T obj in values)
        {
            AddObject(key, obj);
        }
        return this;
    }
    
    public string Build(string baseUrl)
    {
        string fullUrl = baseUrl;
        bool isFirst = true;
        foreach (KeyValuePair<string,string> pair in _entries)
        {
            char seperatorChar = isFirst ? '?' : '&';
            fullUrl += seperatorChar;
            fullUrl += pair.Key + "=" + pair.Value;
            isFirst = false;
        }
        
        return fullUrl;
    }
}