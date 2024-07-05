namespace MichaelKjellander.IndependentUtils.Collections;

public class Indexer<T, TV> where T : struct
{
    private readonly Dictionary<T, TV> _map = new Dictionary<T, TV>();
    private readonly Dictionary<string, Action> _changeListeners = new Dictionary<string, Action>();

    public ICollection<T> Keys => _map.Keys;

    public void Clear()
    {
        _map.Clear();
        NotifyChange();
    }

    public void AddChangeListener(string name, Action action)
    {
        _changeListeners[name] = action;
    }

    private void NotifyChange()
    {

        foreach(Action action in _changeListeners.Values)
        {
            action();
        }
    }

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

            NotifyChange();
        }
    }
}