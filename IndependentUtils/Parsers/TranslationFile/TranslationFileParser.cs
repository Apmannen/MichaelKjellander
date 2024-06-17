namespace MichaelKjellander.IndependentUtils.Parsers.TranslationFile;

public static class TranslationFileParser
{
    public static Dictionary<string,string> ParsePortableObjectFile(string poFileContent)
    {
        Dictionary<string, string> translationsByKey = new();
        string[] rows = poFileContent.Split("\n");
        Pair? currentEntry = null;
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
                currentEntry = new Pair();
            }

            string fileKey = pieces[0];
            string fileValue = pieces[1].Substring(1, pieces[1].Length - 2);
            switch (fileKey)
            {
                case "msgid":
                    currentEntry.key = fileValue;
                    break;
                case "msgstr":
                    currentEntry.value = fileValue;
                    break;
            }

            if (!string.IsNullOrEmpty(currentEntry.key) && !string.IsNullOrEmpty(currentEntry.value))
            {
                translationsByKey[currentEntry.key] = currentEntry.value; 
                currentEntry = null;
            }
        }

        return translationsByKey;
    }

    private class Pair
    {
        public string? key;
        public string? value;
    }
}