using MichaelKjellander.Data;
using MichaelKjellander.Models.Wordpress;

namespace MichaelKjellander.TmpName;

/// <summary>
/// Only supports one language for now.
/// TODO: I want to generate a class with translation keys as enums.
/// TODO: add a API endpoint for getting translations
/// TODO: The idea is that it should only be created once, but now it's added on every page change.
/// </summary>
public class TranslationService
{
    private readonly Dictionary<string, WpTranslationEntry> _translationsByKey = new();

    public TranslationService()
    {
        Console.WriteLine("***** INIT TS!!!!!!!!!");
        using BlogDataContext context = new();
        List<WpTranslationEntry> entries = context.TranslationEntries.ToList();

        foreach (WpTranslationEntry entry in entries)
        {
            _translationsByKey[entry.Key!] = entry;
        }
    }

    public string Get(string translationKey)
    {
        return _translationsByKey[translationKey].Text!;
    }
}