namespace MichaelKjellander.Tools.Url;

public enum QueryArrayMode { Multiple, CommaSeparated }
public class UrlBuilder
{
    private readonly List<KeyValuePair<string,string>> _entries = new();
    private readonly QueryArrayMode _arrayMode;
    private readonly string _baseUrl;

    public UrlBuilder(string baseUrl, QueryArrayMode arrayMode)
    {
        this._baseUrl = baseUrl;
        this._arrayMode = arrayMode;
    }
    
    public UrlBuilder AddParam(string key, string? value)
    {
        return AddObject(key, value);
    }
    public UrlBuilder AddParam(string key, int? value)
    {
        return AddObject(key, value);
    }
    public UrlBuilder AddParam(string key, ICollection<int>? values)
    {
        return AddObjects(key, values);
    }
    public UrlBuilder AddParam(string key, ICollection<string>? values)
    {
        return AddObjects(key, values);
    }

    private UrlBuilder AddObject(string key, object? value)
    {
        string? stringValue;
        if (value != null && !string.IsNullOrEmpty(stringValue = value.ToString()))
        {
            _entries.Add(new KeyValuePair<string, string>(key, stringValue));
        }
        return this;
    }
    private UrlBuilder AddObjects<T>(string key, ICollection<T>? values)
    {
        if (values == null || values.Count == 0)
        {
            return this;
        }

        if (_arrayMode == QueryArrayMode.Multiple)
        {
            foreach (T obj in values)
            {
                AddObject(key, obj);
            }
        }
        else if (_arrayMode == QueryArrayMode.CommaSeparated)
        {
            AddObject(key, string.Join(',', values));
        }
        
        return this;
    }
    
    public override string ToString()
    {
        string fullUrl = _baseUrl;
        bool isFirst = true;
        foreach (KeyValuePair<string,string> pair in _entries)
        {
            fullUrl += isFirst ? '?' : '&';
            fullUrl += pair.Key + "=" + pair.Value;
            isFirst = false;
        }
        
        return fullUrl;
    }

    public Url ToUrl()
    {
        return new Url(ToString());
    }
}