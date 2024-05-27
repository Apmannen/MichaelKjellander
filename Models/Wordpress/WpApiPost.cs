namespace MichaelKjellander.Models.Wordpress;

public class WpApiPost
{
    public int id;
    public RenderedProp title;

    public int Id => id;
    public string Title => title.rendered;
    
}

public class RenderedProp
{
    public string rendered;
}
