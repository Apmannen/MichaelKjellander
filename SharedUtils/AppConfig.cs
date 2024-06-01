namespace MichaelKjellander.SharedUtils;

public enum AppEnvironment { Unknown, Local, WwwDev, Prod }

public class AppConfig
{
    
    public AppEnvironment AppEnvironment { get; set; }
    public string? SiteUrl { get; set; }

    public bool IsAnyDev => AppEnvironment != AppEnvironment.Prod;

    public void ParseAndSetSiteUrl(string urls)
    {
        string[] urlsArray = urls.Split(";");
        foreach (string aUrl in urlsArray)
        {
            if (aUrl.StartsWith("https"))
            {
                this.SiteUrl = aUrl;
                return;
            }
        }

        this.SiteUrl = urlsArray[0];
    }
    // public static AppEnvironment ParseAppEnvironment(string appEnvironmentString)
    // {
    //     return Enum.Parse<AppEnvironment>(appEnvironmentString);
    // }

    public static bool IsAnyWww(AppEnvironment appEnvironment)
    {
        return appEnvironment is AppEnvironment.Prod or AppEnvironment.WwwDev;
    }
    
    
}