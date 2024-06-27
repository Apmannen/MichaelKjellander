namespace MichaelKjellander.IndependentUtils.Collections;

public class Indexer<T>
{
    public readonly HashSet<T> Values = new();

    public void Clear()
    {
        Values.Clear();
    }

    public void ReplaceWithRange(ICollection<T> indexes)
    {
        Clear();
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