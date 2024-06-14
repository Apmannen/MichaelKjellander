using MichaelKjellander.Config;

namespace MichaelKjellander.SharedUtils;



public static class EnvironmentUtil
{
    private enum EnvVariable
    {
        ASPNETCORE_URLS, 
        SG_APPENVIRONMENT, 
        //Example:
        //Server=127.0.0.1;Database=kjelle_db;User=root;Password=test;Port=3306;SslMode=none;
        SG_MYSQLCONNSTRING,
        SG_CLEAN_INTERNAL_WP_DB
    }
    public static void SetupAppConfig(AppConfig config)
    {
        config.AppEnvironment = GetAppEnvironment();
        config.SiteUrl = GetSiteUrl();
    }
    
    //General
    private static string ParseString(EnvVariable variable)
    {
        return Environment.GetEnvironmentVariable(variable.ToString())!;
    }

    private static string[] ParseArray(EnvVariable variable)
    {
        return ParseString(variable).Split(";");
    }
    private static TEnum ParseEnum<TEnum>(EnvVariable variable) where TEnum : struct,Enum
    {
        string value = ParseString(variable);
        return Enum.Parse<TEnum>(value);
    }
    private static TEnum ParseEnum<TEnum>(EnvVariable variable, TEnum defaultValue) where TEnum : struct,Enum
    {
        string value = ParseString(variable);
        bool successful = Enum.TryParse(value, out TEnum enumValue);
        return successful ? enumValue : defaultValue;
    }

    //Specific
    public static AppEnvironment GetAppEnvironment()
    {
        return ParseEnum(EnvVariable.SG_APPENVIRONMENT, AppEnvironment.Unknown);
    }

    public static string GetMysqlConnectionString()
    {
        return ParseString(EnvVariable.SG_MYSQLCONNSTRING);
    }
    private static string GetSiteUrl()
    {
        string[] urlsArray = ParseArray(EnvVariable.ASPNETCORE_URLS);
        foreach (string aUrl in urlsArray)
        {
            if (aUrl.StartsWith("https"))
            {
                return aUrl;
            }
        }
        return urlsArray[0];
    }
    
}