namespace MichaelKjellander.IndependentUtils.Collections;

public class Indexer<T, TV> where T : struct
{
    private readonly Dictionary<T, TV> _map = new Dictionary<T, TV>();

    public ICollection<T> Keys => _map.Keys;
    //public ICollection<TV> Values => _map.Values;

    public void Clear()
    {
        _map.Clear();
    }

    /*public void ReplaceWithRange(ICollection<T> indexes)
    {
        _values.Clear();
        _values.UnionWith(indexes);
        OnChange();
    }*/

    public TV this[T index]
    {
        get => _map.TryGetValue(index, out TV value) ? value : default(TV);
        set
        {
            if (EqualityComparer<TV>.Default.Equals(value,default(TV)))
            {
                _map.Remove(index);
            }
            else
            {
                _map[index] = value;
            }
        }
    }
}