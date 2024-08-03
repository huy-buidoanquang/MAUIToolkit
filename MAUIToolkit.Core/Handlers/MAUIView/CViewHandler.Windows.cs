using MAUIToolkit.Core.Controls;
using MAUIToolkit.Core.Platforms;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;

namespace MAUIToolkit.Core.Handlers;

internal partial class CViewHandler : LayoutHandler
{
    private LayoutPanelExt? layoutPanelExt;

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
    protected override LayoutPanel CreatePlatformView()
    {
        if (base.VirtualView == null)
        {
            throw new InvalidOperationException("VirtualView must be set to create a LayoutViewGroup");
        }

        layoutPanelExt = new LayoutPanelExt((CView)base.VirtualView)
        {
            CrossPlatformMeasure = base.VirtualView.CrossPlatformMeasure,
            CrossPlatformArrange = base.VirtualView.CrossPlatformArrange
        };
        return layoutPanelExt;
    }

    //
    // Parameters:
    //   view:
    public override void SetVirtualView(IView view)
    {
        base.SetVirtualView(view);
        if (base.VirtualView == null)
        {
            throw new InvalidOperationException("VirtualView should have been set by base class.");
        }

        if (layoutPanelExt != null)
        {
            layoutPanelExt.CrossPlatformMeasure = base.VirtualView.CrossPlatformMeasure;
            layoutPanelExt.CrossPlatformArrange = base.VirtualView.CrossPlatformArrange;
        }
    }

    public void Invalidate()
    {
        if (layoutPanelExt != null)
        {
            layoutPanelExt.Invalidate();
        }
    }

    //
    // Parameters:
    //   drawingOrder:
    public void SetDrawingOrder(DrawingOrder drawingOrder = DrawingOrder.NoDraw)
    {
        if (layoutPanelExt != null)
        {
            layoutPanelExt.DrawingOrder = drawingOrder;
        }
    }

    public void UpdateClipToBounds(bool clipToBounds)
    {
        if (layoutPanelExt != null)
        {
            layoutPanelExt.ClipsToBounds = clipToBounds;
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
        if ((object)base.PlatformView == null)
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

        int count = base.PlatformView.Children.Count;
        if (!(layoutPanelExt != null))
        {
            return;
        }

        LayoutPanelExt? obj = layoutPanelExt;
        if ((object)obj != null && obj.DrawingOrder == DrawingOrder.AboveContent)
        {
            if (count > 0)
            {
                base.PlatformView.Children.Insert(count - 1, child.ToPlatform(base.MauiContext));
            }
            else
            {
                base.PlatformView.Children.Insert(count, child.ToPlatform(base.MauiContext));
            }
        }
        else
        {
            base.PlatformView.Children.Insert(count, child.ToPlatform(base.MauiContext));
        }
    }

    //
    // Parameters:
    //   index:
    //
    //   child:
    //
    // Exceptions:
    //   T:System.InvalidOperationException:
    public new void Insert(int index, IView child)
    {
        if ((object)base.PlatformView == null)
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

        if (layoutPanelExt != null)
        {
            LayoutPanelExt? obj = layoutPanelExt;
            if ((object)obj != null && obj.DrawingOrder == DrawingOrder.BelowContent)
            {
                base.PlatformView.Children.Insert(index + 1, child.ToPlatform(base.MauiContext));
            }
            else
            {
                base.PlatformView.Children.Insert(index, child.ToPlatform(base.MauiContext));
            }
        }
    }

    //
    // Summary:
    //     Invalidates the semantics nodes.
    internal void InvalidateSemantics()
    {
        layoutPanelExt?.InvalidateSemantics();
    }

    //
    // Parameters:
    //   platformView:
    protected override void DisconnectHandler(LayoutPanel platformView)
    {
        base.DisconnectHandler(platformView);
        foreach (IView item in base.VirtualView)
        {
            if (item.Handler != null)
            {
                item.Handler.DisconnectHandler();
            }
        }

        if (layoutPanelExt != null)
        {
            layoutPanelExt.Dispose();
            layoutPanelExt = null;
        }
    }
}
