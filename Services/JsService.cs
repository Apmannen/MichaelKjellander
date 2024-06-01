namespace MichaelKjellander.Services;

using Microsoft.JSInterop;
using System.Threading.Tasks;

public class JsService
{
    private readonly IJSRuntime _js;

    public JsService(IJSRuntime js)
    {
        _js = js;
    }

    public void ScrollToTop()
    {
        _js.InvokeVoidAsync("scrollToTop");
    }
}