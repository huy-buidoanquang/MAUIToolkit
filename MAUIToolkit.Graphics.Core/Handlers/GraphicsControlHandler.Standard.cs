using Microsoft.Maui.Graphics.Platform;
using Microsoft.Maui.Handlers;

namespace MAUIToolkit.Graphics.Core;
public partial class GraphicsControlHandler<TViewDrawable, TVirtualView> : ViewHandler<TVirtualView, object>
{
    protected override object CreatePlatformView() => throw new NotImplementedException();
    public void Invalidate() => throw new NotImplementedException();
}
