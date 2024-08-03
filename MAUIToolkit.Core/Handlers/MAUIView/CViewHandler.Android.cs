using MAUIToolkit.Core.Platforms;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;

namespace MAUIToolkit.Core.Handlers;

internal partial class CViewHandler : LayoutHandler
{
    private LayoutViewGroupExt? layoutViewGroupExt;

    //
    // Exceptions:
    //   T:System.InvalidOperationException:
    protected override LayoutViewGroup CreatePlatformView()
    {
        if (base.VirtualView == null)
        {
            throw new InvalidOperationException("VirtualView must be set to create a LayoutViewGroup");
        }

        layoutViewGroupExt = new LayoutViewGroupExt(base.Context, (View)base.VirtualView)
        {
            CrossPlatformMeasure = base.VirtualView.CrossPlatformMeasure,
            CrossPlatformArrange = base.VirtualView.CrossPlatformArrange
        };
        layoutViewGroupExt.SetClipChildren(clipChildren: true);
        return layoutViewGroupExt;
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

        if (layoutViewGroupExt != null)
        {
            layoutViewGroupExt.CrossPlatformMeasure = base.VirtualView.CrossPlatformMeasure;
            layoutViewGroupExt.CrossPlatformArrange = base.VirtualView.CrossPlatformArrange;
        }
    }

    public void Invalidate()
    {
        base.PlatformView?.Invalidate();
    }

    //
    // Parameters:
    //   drawingOrder:
    public void SetDrawingOrder(DrawingOrder drawingOrder = DrawingOrder.NoDraw)
    {
        if (layoutViewGroupExt != null)
        {
            layoutViewGroupExt.DrawingOrder = drawingOrder;
        }
    }

    public void UpdateClipToBounds(bool clipToBounds)
    {
        if (layoutViewGroupExt != null)
        {
            layoutViewGroupExt.ClipsToBounds = clipToBounds;
        }
    }

    //
    // Summary:
    //     Invalidates the semantics nodes.
    internal void InvalidateSemantics()
    {
        layoutViewGroupExt?.InvalidateSemantics();
    }

    //
    // Parameters:
    //   platformView:
    protected override void DisconnectHandler(LayoutViewGroup platformView)
    {
        base.DisconnectHandler(platformView);
        foreach (IView item in base.VirtualView)
        {
            if (item.Handler != null)
            {
                item.Handler.DisconnectHandler();
            }
        }

        if (layoutViewGroupExt != null)
        {
            layoutViewGroupExt = null;
        }
    }

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
}
