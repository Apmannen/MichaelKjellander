using MichaelKjellander.Config;
using MichaelKjellander.Models.Wordpress;
using MichaelKjellander.Scripts.Startup.Generated;
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
}