using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace MichaelKjellander.Views.Services;

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

    public async Task AttachDraggable(ElementReference draggableParent)
    {
        //_js.InvokeVoidAsync("debugObject2", draggableParent);
        await _js.InvokeVoidAsync("attachDraggable", draggableParent);
    }
}