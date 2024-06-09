using Microsoft.AspNetCore.Components.Web;

namespace MichaelKjellander.Services;

using Microsoft.JSInterop;

public class JsService
{
    private readonly IJSRuntime _js;

    public JsService(IJSRuntime js)
    {
        _js = js;
    }

    public void DebugArgs(MouseEventArgs args)
    {
        _js.InvokeVoidAsync("debugObject", args);
    }

    public void ScrollToTop()
    {
        _js.InvokeVoidAsync("scrollToTop");
    }
}