namespace MichaelKjellander.IndependentUtils.Parsers.TranslationFile;

public static class TranslationFileParser
{
    public static List<T> ParsePortableObjectFile<T>(string poFileContent, Func<T> createTranslationEntry) where T : class,ITranslationEntry
    {
        List<T> entries = [];
        string[] rows = poFileContent.Split("\n");
        T? currentEntry = null;
        foreach (string row in rows)
        {
            string[] pieces = row.Split(' ', 2);
            if (pieces.Length != 2)
            {
                currentEntry = null;
                continue;
            }
            if (currentEntry == null)
            {
                currentEntry = createTranslationEntry();
            }

            string fileKey = pieces[0];
            string fileValue = pieces[1].Substring(1, pieces[1].Length - 2);
            switch (fileKey)
            {
                case "msgid":
                    currentEntry.SetKey(fileValue);
                    break;
                case "msgstr":
                    currentEntry.SetText(fileValue);
                    break;
            }

            if (currentEntry.GetKey().Length > 0 && currentEntry.GetText().Length > 0)
            {
                entries.Add(currentEntry);
                currentEntry = null;
            }
        }

        return entries;
    }
}