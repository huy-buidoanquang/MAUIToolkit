﻿using Microsoft.Maui.Graphics.Platform;
using Microsoft.Maui.Handlers;

namespace MAUIToolkit.Graphics.Core;

public partial class GraphicsControlHandler<TViewDrawable, TVirtualView> : ViewHandler<TVirtualView, PlatformGraphicsView>
{
    protected override PlatformGraphicsView CreatePlatformView() =>
        new NativeGraphicsControlView { GraphicsControl = this };

    public void Invalidate() =>
        PlatformView?.InvalidateDrawable();
}