using Microsoft.Maui.Handlers;

namespace MAUIToolkit.Graphics.Core;

public partial class GraphicsControlHandler<TViewDrawable, TVirtualView> : ViewHandler<TVirtualView, NativeGraphicsControlView>
{
    protected override NativeGraphicsControlView CreatePlatformView() =>
        new NativeGraphicsControlView { GraphicsControl = this };

    public void Invalidate() =>
        base.PlatformView?.Invalidate();
}