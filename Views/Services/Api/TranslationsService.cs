using MichaelKjellander.Config;
using MichaelKjellander.Domains.Generated;
using MichaelKjellander.Domains.Models.Wordpress;
using Microsoft.Extensions.Options;

namespace MichaelKjellander.Views.Services.Api;

/// <summary>
/// Only supports one language for now.
/// </summary>
public class TranslationsService : InternalApiService
{
    private readonly Dictionary<string, WpTranslationEntry> _translationsByKey = new();

    public TranslationsService(HttpClient client, IOptions<AppConfig> options) : base(client, options)
    {
        Console.WriteLine("***** INIT TS!!!!!!!!!");
        var task = Task.Run(async () => await this.FetchModels<WpTranslationEntry>(this.ApiRoutes.Translations));
        
        foreach (WpTranslationEntry entry in task.Result.Items!)
        {
            _translationsByKey[entry.Key!] = entry;
        }
    }
    public string Get(TKey translationKey)
    {
        string stringKey = translationKey.ToString();
        if (!_translationsByKey.TryGetValue(stringKey, out WpTranslationEntry? entry))
        {
            return "";
        }
        return entry.Text!;
    }
    public string Format(TKey key, params string[] replace) 
    {
        return string.Format(Get(key), replace);
    }

    public string GetRatingTranslation(int? rating)
    {
        TKey tKey = Enum.Parse<TKey>("Rating_" + rating);
        if (rating == null)
        {
            tKey = TKey.Illegal;
        }
        return Get(tKey);
    }
    
    public string FormatPageTitle(TKey tKey)
    {
        return FormatPageTitle(Get(tKey));
    }
    
    public string FormatPageTitle(string? pageName, int? pagingPage = null)
    {
        string title = "";
        if (pageName != null)
        {
            title += $"{pageName} - ";
        }
        if (pagingPage != null)
        {
            title += $"{Get(TKey.Blog_Page)} {pagingPage} - ";
        }

        title += "Michael Kjellander";
        
        return title;
    }
    
    public string FormatDate(DateOnly date)
    {
        return date.ToString("yyyy-MM-dd");
    }
}