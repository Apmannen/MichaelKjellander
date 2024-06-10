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

    public void ListenToClicksForActivatingTarget(ElementReference rootElement, ElementReference targetElement)
    {
        _js.InvokeVoidAsync("listenToClicksForActivatingTarget", targetElement);
    }
    
    public void TrackClickAndActivateTarget(IList<string> classes, ElementReference target)
    {
        Console.WriteLine("*** TRACK:"+classes.Count);
        _js.InvokeVoidAsync("debugObject", classes);
        _js.InvokeVoidAsync("trackClickAndActivateTarget", classes, target);
    }
    
    [Obsolete("Might be replaced")]
    public void AttachExclusiveListener(ElementReference element)
    {
        _js.InvokeVoidAsync("attachExclusiveListener", element);
    }
    [Obsolete("Might be replace")]
    public async Task<bool> CheckIfTargetHasBeenClicked(ElementReference element)
    {
        return await _js.InvokeAsync<bool>("checkIfTargetHasBeenClicked", element);
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