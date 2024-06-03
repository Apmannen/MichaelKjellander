namespace MichaelKjellander.SharedUtils;

public enum EnvVariable
{
    ASPNETCORE_URLS, 
    SG_APPENVIRONMENT, 
    //Example:
    //Server=127.0.0.1;Database=kjelle_db;User=root;Password=test;Port=3306;SslMode=none;
    SG_MYSQLCONNSTRING,
}

public static class EnvironmentUtil
{
    public static string Get(EnvVariable variable)
    {
        return Environment.GetEnvironmentVariable(variable.ToString())!;
    }

    public static TEnum ParseEnum<TEnum>(EnvVariable variable) where TEnum : struct,Enum
    {
        string value = Get(variable);
        return Enum.Parse<TEnum>(value);
    }
}