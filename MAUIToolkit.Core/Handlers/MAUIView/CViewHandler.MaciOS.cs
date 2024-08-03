using MAUIToolkit.Core.Platform;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;

namespace MAUIToolkit.Core.Handlers;

internal partial class CViewHandler : LayoutHandler
{
    private LayoutViewExt? layoutViewExt;

    public CViewHandler()
    : base(Microsoft.Maui.Handlers.ViewHandler.ViewMapper)
    {
    }

    //
    // Parameters:
    //   mapper:
    public CViewHandler(PropertyMapper mapper)
        : base(mapper)
    {
    }

    //
    // Exceptions:
    //   T:System.InvalidOperationException:
    protected override LayoutView CreatePlatformView()
    {
        if (base.VirtualView == null)
        {
            throw new InvalidOperationException("VirtualView must be set to create a LayoutViewGroup");
        }

        layoutViewExt = new LayoutViewExt((IDrawable)base.VirtualView)
        {
            CrossPlatformMeasure = base.VirtualView.CrossPlatformMeasure,
            CrossPlatformArrange = base.VirtualView.CrossPlatformArrange
        };
        return layoutViewExt;
    }

    //
    // Parameters:
    //   view:
    //
    // Exceptions:
    //   T:System.InvalidOperationException:
    public override void SetVirtualView(IView view)
    {
        base.SetVirtualView(view);
        if (base.VirtualView == null)
        {
            throw new InvalidOperationException("VirtualView should have been set by base class.");
        }

        if (layoutViewExt != null)
        {
            layoutViewExt.CrossPlatformMeasure = base.VirtualView.CrossPlatformMeasure;
            layoutViewExt.CrossPlatformArrange = base.VirtualView.CrossPlatformArrange;
        }
    }

    public void Invalidate()
    {
        if (layoutViewExt != null)
        {
            layoutViewExt.InvalidateDrawable();
        }
    }

    //
    // Parameters:
    //   drawingOrder:
    public void SetDrawingOrder(DrawingOrder drawingOrder = DrawingOrder.NoDraw)
    {
        if (layoutViewExt != null)
        {
            layoutViewExt.DrawingOrder = drawingOrder;
        }
    }

    public void UpdateClipToBounds(bool clipToBounds)
    {
        if (layoutViewExt != null)
        {
            layoutViewExt.ClipsToBounds = clipToBounds;
        }
    }

    //
    // Parameters:
    //   child:
    //
    // Exceptions:
    //   T:System.InvalidOperationException:
    public new void Add(IView child)
    {
        if (base.PlatformView == null)
        {
            throw new InvalidOperationException("PlatformView should have been set by base class.");
        }

        if (base.VirtualView == null)
        {
            throw new InvalidOperationException("VirtualView should have been set by base class.");
        }

        if (base.MauiContext == null)
        {
            throw new InvalidOperationException("MauiContext should have been set by base class.");
        }

        int num = base.PlatformView.Subviews.Length;
        if (layoutViewExt == null)
        {
            return;
        }

        if (layoutViewExt.DrawingOrder == DrawingOrder.AboveContent)
        {
            if (num > 0)
            {
                base.PlatformView.InsertSubview(child.ToPlatform(base.MauiContext), num - 1);
            }
            else
            {
                base.PlatformView.InsertSubview(child.ToPlatform(base.MauiContext), num);
            }
        }
        else
        {
            base.PlatformView.InsertSubview(child.ToPlatform(base.MauiContext), num);
        }
    }

    //
    // Summary:
    //     Invalidates the semantics nodes.
    internal void InvalidateSemantics()
    {
        layoutViewExt?.InvalidateSemantics();
    }

    //
    // Parameters:
    //   platformView:
    protected override void DisconnectHandler(LayoutView platformView)
    {
        base.DisconnectHandler(platformView);
        foreach (IView item in base.VirtualView)
        {
            if (item.Handler != null)
            {
                item.Handler.DisconnectHandler();
            }
        }

        if (layoutViewExt != null)
        {
            layoutViewExt = null;
        }
    }
}
