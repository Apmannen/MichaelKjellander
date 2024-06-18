using MichaelKjellander.Views.Services.Api;

namespace MichaelKjellander.Views.Utils;

public class ComponentUtil
{
    public static string FormatDate(DateOnly date)
    {
        return date.ToString("yyyy-MM-dd");
    }
}