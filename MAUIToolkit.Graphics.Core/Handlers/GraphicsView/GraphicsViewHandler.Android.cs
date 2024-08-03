﻿using Android.Views;
using Microsoft.Maui.Graphics.Platform;
using Microsoft.Maui.Handlers;
using APointF = Android.Graphics.PointF;

namespace MAUIToolkit.Graphics.Core;

public partial class GraphicsViewHandler : ViewHandler<IGraphicsView, PlatformGraphicsView>
{
    protected override PlatformGraphicsView CreatePlatformView()
    {
        var nativeGraphicsView = new PlatformGraphicsView(Context)
        {
            Drawable = VirtualView
        };

        return nativeGraphicsView;
    }

    protected override void ConnectHandler(PlatformGraphicsView platformView)
    {
        base.ConnectHandler(platformView);

        platformView.ViewAttachedToWindow += OnViewAttachedToWindow;
        platformView.ViewDetachedFromWindow += OnViewDetachedFromWindow;
        platformView.Touch += OnTouch;
    }

    protected override void DisconnectHandler(PlatformGraphicsView platformView)
    {
        base.DisconnectHandler(platformView);

        platformView.ViewAttachedToWindow -= OnViewAttachedToWindow;
        platformView.ViewDetachedFromWindow -= OnViewDetachedFromWindow;
        platformView.Touch -= OnTouch;
    }
    
    public static void MapInvalidate(GraphicsViewHandler handler, IGraphicsView graphicsView, object? arg)
    {
        handler.PlatformView?.Invalidate();
    }

    void OnViewAttachedToWindow(object? sender,  Android.Views.View.ViewAttachedToWindowEventArgs e)
    {
        VirtualView?.Load();
    }

    void OnViewDetachedFromWindow(object? sender, Android.Views.View.ViewDetachedFromWindowEventArgs e)
    {
        VirtualView?.Unload();
    }

    void OnTouch(object? sender, Android.Views.View.TouchEventArgs e)
    {
        if (e.Event == null)
            return;

        float density = Context?.Resources?.DisplayMetrics?.Density ?? 1.0f;
        APointF point = new APointF(e.Event.GetX() / density, e.Event.GetY() / density);

        switch (e.Event.Action)
        {
            case MotionEventActions.Down:
                VirtualView?.OnTouchDown(new Point(point.X, point.Y));
                break;
            case MotionEventActions.Move:
                VirtualView?.OnTouchMove(new Point(point.X, point.Y));
                break;
            case MotionEventActions.Up:
            case MotionEventActions.Cancel:
                VirtualView?.OnTouchUp(new Point(point.X, point.Y));
                break;
            default:
                break;
        }
    }
}
