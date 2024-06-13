using Microsoft.AspNetCore.Components;
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

    public void ClientLog(object obj)
    {
        _js.InvokeVoidAsync("debugObject", obj);
    }

    public void ScrollToTop()
    {
        _js.InvokeVoidAsync("scrollToTop");
    }
}