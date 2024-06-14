using MichaelKjellander.SharedUtils;

namespace MichaelKjellander.Config;

public class AppConfig
{
    public AppEnvironment AppEnvironment { get; set; }
    public string? SiteUrl { get; set; }

    public bool IsAnyDev => AppEnvironment != AppEnvironment.Prod;

    public static bool IsAnyWww(AppEnvironment appEnvironment)
    {
        return appEnvironment is AppEnvironment.Prod or AppEnvironment.WwwDev;
    }
}