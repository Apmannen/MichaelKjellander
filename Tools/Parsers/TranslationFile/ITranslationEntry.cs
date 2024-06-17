namespace MichaelKjellander.Tools.Parsers.TranslationFile;

public interface ITranslationEntry
{
    public void SetKey(string key);
    public void SetText(string text);
    public string GetKey();
    public string GetText();
}