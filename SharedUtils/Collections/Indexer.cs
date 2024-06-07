namespace MichaelKjellander.SharedUtils.Collections;

public class Indexer<T>
{
    public readonly HashSet<T> Values = new();

    public void ReplaceWithRange(ICollection<T> indexes)
    {
        Values.Clear();
        Values.UnionWith(indexes);
    }
    
    public bool this[T index]
    {
        get => Values.Contains(index);
        set
        {
            if (value)
            {
                Values.Add(index);
            }
            else
            {
                Values.Remove(index);
            }
        }
    }
}