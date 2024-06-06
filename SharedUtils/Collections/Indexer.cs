namespace MichaelKjellander.SharedUtils.Collections;

public class Indexer
{
    public readonly HashSet<int> Values = new();
    public bool this[int rating]
    {
        get => Values.Contains(rating);
        set
        {
            if (value)
            {
                Values.Add(rating);
            }
            else
            {
                Values.Remove(rating);
            }
        }
    }
}