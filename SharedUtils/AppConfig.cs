namespace MichaelKjellander.SharedUtils;

public enum AppEnvironment { Unknown, Local, WwwDev, Prod }

public class AppConfig
{
    
    public AppEnvironment AppEnvironment { get; set; }
    public string? SiteUrl { get; set; }

    public bool IsAnyDev => AppEnvironment != AppEnvironment.Prod;

    public void SetAppEnvironment(string appEnvironmentString)
    {
        AppEnvironment = Enum.Parse<AppEnvironment>(appEnvironmentString);
    }
    
    
}