namespace MichaelKjellander.IndependentUtils.Parsers.PortableObject;

public static class PortableObjectsParser
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
                    currentEntry.Key = fileValue;
                    break;
                case "msgstr":
                    currentEntry.Value = fileValue;
                    break;
            }

            if (!string.IsNullOrEmpty(currentEntry.Key) && !string.IsNullOrEmpty(currentEntry.Value))
            {
                translationsByKey[currentEntry.Key] = currentEntry.Value; 
                currentEntry = null;
            }
        }

        return translationsByKey;
    }

    private class Pair
    {
        public string? Key;
        public string? Value;
    }
}