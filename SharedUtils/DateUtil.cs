namespace MichaelKjellander.SharedUtils;

public static class DateUtil
{
    public static string FormatDate(DateOnly date)
    {
        return date.ToString("yyyy-MM-dd");
    }
}