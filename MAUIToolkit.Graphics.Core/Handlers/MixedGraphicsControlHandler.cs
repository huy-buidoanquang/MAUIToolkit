﻿#nullable enable
#if __IOS__ || MACCATALYST
using PlatformView = UIKit.UIView;
#elif MONOANDROID || ANDROID
using PlatformView = Android.Views.View;
#elif WINDOWS
using PlatformView = Microsoft.UI.Xaml.FrameworkElement;
//#elif TIZEN
//using PlatformView = Tizen.UIExtensions.NUI.GraphicsView.ProgressBar;
#elif (NETSTANDARD || !PLATFORM) || (NET6_0_OR_GREATER && !IOS && !ANDROID && !TIZEN)
using PlatformView = System.Object;
#endif
using Microsoft.Maui.Handlers;

namespace MAUIToolkit.Graphics.Core;

public abstract partial class MixedGraphicsControlHandler<TViewDrawable, TVirtualView, TPlatformView> : ViewHandler<TVirtualView, TPlatformView>, IViewHandler, IMixedGraphicsHandler
        where TVirtualView : class, IView
        where TViewDrawable : class, IViewDrawable
#if !NETSTANDARD || IOS || ANDROID || WINDOWS
        where TPlatformView : PlatformView, IMixedPlatformView
#else
		where TPlatformView : class
#endif
{
    TViewDrawable? _drawable;
    protected readonly DrawMapper _drawMapper;
    ControlState _currentState = ControlState.Default;

    protected MixedGraphicsControlHandler() : base(ViewHandler.Mapper)
    {
        _drawMapper = ViewHandler.DrawMapper;
    }

    protected MixedGraphicsControlHandler(DrawMapper? drawMapper, PropertyMapper mapper) : base(mapper ?? ViewHandler.Mapper)
    {
        _drawMapper = drawMapper ?? new DrawMapper<TViewDrawable, TVirtualView>(ViewHandler.DrawMapper);
    }

    DrawMapper IMixedGraphicsHandler.DrawMapper => _drawMapper;

    public RectF Bounds { get; private set; }

    public bool TouchEnabled { get; set; } = true;

    protected PointF CurrentTouchPoint { get; set; }

    public bool PointsContained(PointF[] points) => points.Any(p => Bounds.BoundsContains(p));

    public ControlState CurrentState
    {
        get => base.VirtualView!.IsEnabled ? _currentState : ControlState.Disabled;
        set
        {
            if (_currentState == value)
                return;

            _currentState = value;
            Drawable.CurrentState = value;

            ControlStateChanged();
            Invalidate();
        }
    }

    protected TViewDrawable Drawable
    {
        get => _drawable ??= CreateDrawable();
        set => _drawable = value;
    }

    protected abstract TViewDrawable CreateDrawable();

    public virtual void StartHoverInteraction(PointF[] points)
    {
        CurrentTouchPoint = points.FirstOrDefault();
        CurrentState = ControlState.Hovered;
    }

    public virtual void HoverInteraction(PointF[] points)
    {
    }

    public virtual void EndHoverInteraction()
    {
    }

    public virtual bool StartInteraction(PointF[] points)
    {
        CurrentTouchPoint = points.FirstOrDefault();
        CurrentState = ControlState.Pressed;

        return true;
    }

    public virtual void DragInteraction(PointF[] points)
    {
        CurrentTouchPoint = points.FirstOrDefault();
    }

    public virtual void EndInteraction(PointF[] points, bool inside)
    {
        CurrentState = ControlState.Default;
    }

    public virtual void CancelInteraction()
    {
        CurrentState = ControlState.Default;
    }

    protected virtual void ControlStateChanged()
    {
    }

    public void Invalidate()
    {
        if (base.PlatformView is IInvalidatable invalidatableView)
            invalidatableView?.Invalidate();
    }

    public virtual void Resized(RectF bounds)
    {
        Bounds = bounds;
    }

    public override Size GetDesiredSize(double widthConstraint, double heightConstraint) =>
        Drawable.GetDesiredSize(base.VirtualView!, widthConstraint, heightConstraint);

    public override void SetVirtualView(IView view)
    {
        base.SetVirtualView(view);

        Drawable.View = base.VirtualView!;
        if (base.PlatformView is IMixedPlatformView mnv)
            mnv.Drawable = this;

        Invalidate();
    }

    public virtual void Draw(ICanvas canvas, RectF dirtyRect)
    {
        if (base.VirtualView == null || _drawMapper == null)
            return;

        canvas.SaveState();

        var layers = LayerDrawingOrder();
        var rect = dirtyRect;
        bool hasDrawnBase = false;
        var mixedPlatformView = base.PlatformView as IMixedPlatformView;
        var platformLayers = mixedPlatformView?.PlatformLayers;

        foreach (var layer in layers)
        {
            //This will allow the native layer to handle the layers it can,
            //i.e: For Entry, the Text layer and Caret is handled by the base drawing.
            if (platformLayers != null && platformLayers.Contains(layer))
            {
                if (hasDrawnBase)
                    continue;

                hasDrawnBase = true;
                mixedPlatformView?.DrawBaseLayer(dirtyRect);
            }
            else
                _drawMapper?.DrawLayer(canvas, rect, Drawable, base.VirtualView, layer);
        }

        canvas.ResetState();
    }

    public abstract string[] LayerDrawingOrder();
}