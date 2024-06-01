namespace MichaelKjellander.SharedUtils;

public enum EnvVariable { ASPNETCORE_URLS, SG_APPENVIRONMENT,  }

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