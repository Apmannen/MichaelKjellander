namespace MichaelKjellander.IndependentUtils.Collections;

public class Indexer<T>
{
    private readonly HashSet<T> _values = [];
    private readonly Dictionary<string, Action> _changeListeners = new Dictionary<string, Action>();

    public ICollection<T> Values => _values.ToArray();

    private void OnChange()
    {
        foreach (Action action in _changeListeners.Values)
        {
            action.Invoke();
        }
    }

    public void AddChangeListener(string name, Action action)
    {
        _changeListeners[name] = action;
    }

    public void Clear()
    {
        _values.Clear();
        OnChange();
    }

    public void ReplaceWithRange(ICollection<T> indexes)
    {
        _values.Clear();
        _values.UnionWith(indexes);
        OnChange();
    }

    public bool this[T index]
    {
        get => _values.Contains(index);
        set
        {
            if (value)
            {
                _values.Add(index);
            }
            else
            {
                _values.Remove(index);
            }
            OnChange();
        }
    }
}