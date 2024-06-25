namespace MichaelKjellander.Views.Utils;

[Obsolete("Use EventCallback")]
public class ComponentCallbacker<T>
{
    public Action<T>? Action { get; set; }
}