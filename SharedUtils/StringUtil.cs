namespace MichaelKjellander.SharedUtils;

[System.Obsolete("Perhaps merge with CollectionUtil")]
public static class StringUtil
{
    public static IList<char> StringToCharList(string s)
    {
        List<char> chars = new List<char>();
        for (int i = 0; i < s.Length; i++)
        {
            chars.Add(s[i]);
        }
        return chars;
    }
}