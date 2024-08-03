using MAUIToolkit.Core.Controls;
using Microsoft.Maui.Platform;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;

namespace MAUIToolkit.Core.Platforms;

internal class LayoutPanelExt : LayoutPanel
{
    private DrawingOrder drawingOrder = DrawingOrder.NoDraw;

    private NativePlatformGraphicsView? nativeGraphicsView;

    private readonly CView mauiView;

    internal Func<double, double, Microsoft.Maui.Graphics.Size>? CrossPlatformMeasure { get; set; }

    internal Func<Microsoft.Maui.Graphics.Rect, Microsoft.Maui.Graphics.Size>? CrossPlatformArrange { get; set; }

    public DrawingOrder DrawingOrder
    {
        get
        {
            return drawingOrder;
        }
        set
        {
            drawingOrder = value;
            if (DrawingOrder == DrawingOrder.NoDraw)
            {
                RemoveDrawableView();
                return;
            }

            InitializeNativeGraphicsView();
            ArrangeNativeGraphicsView();
        }
    }

    public IDrawable Drawable { get; set; }

    //
    // Parameters:
    //   layout:
    public LayoutPanelExt(CView layout)
    {
        Drawable = layout;
        mauiView = layout;
        base.AllowFocusOnInteraction = true;
        base.UseSystemFocusVisuals = true;
        base.SizeChanged += ContentPanelExt_SizeChanged;
    }

    private void ContentPanelExt_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        nativeGraphicsView?.Invalidate();
    }

    internal void InitializeNativeGraphicsView()
    {
        if (!base.Children.Contains(nativeGraphicsView))
        {
            nativeGraphicsView = new NativePlatformGraphicsView(mauiView)
            {
                Drawable = Drawable
            };
        }

        if (nativeGraphicsView != null)
        {
            if (DrawingOrder == DrawingOrder.AboveContentWithTouch || DrawingOrder == DrawingOrder.BelowContent)
            {
                nativeGraphicsView.IsHitTestVisible = true;
            }
            else
            {
                nativeGraphicsView.IsHitTestVisible = false;
            }
        }
    }

    internal void RemoveDrawableView()
    {
        if (nativeGraphicsView != null && base.Children.Contains(nativeGraphicsView))
        {
            base.Children.Remove(nativeGraphicsView);
        }
    }

    internal void ArrangeNativeGraphicsView()
    {
        if (nativeGraphicsView != null)
        {
            if (base.Children.Contains(nativeGraphicsView))
            {
                base.Children.Remove(nativeGraphicsView);
            }

            if (DrawingOrder == DrawingOrder.AboveContentWithTouch || DrawingOrder == DrawingOrder.AboveContent)
            {
                base.Children.Add(nativeGraphicsView);
            }
            else
            {
                base.Children.Insert(0, nativeGraphicsView);
            }
        }
    }

    internal void Invalidate()
    {
        nativeGraphicsView?.Invalidate();
    }

    //
    // Summary:
    //     Invalidates the semantics nodes.
    internal void InvalidateSemantics()
    {
        if (!(nativeGraphicsView == null) && !(nativeGraphicsView.SemanticsAutomationPeer == null))
        {
            nativeGraphicsView.SemanticsAutomationPeer.InvalidateSemantics();
        }
    }

    //
    // Parameters:
    //   finalSize:
    protected override global::Windows.Foundation.Size ArrangeOverride(global::Windows.Foundation.Size finalSize)
    {
        if (CrossPlatformArrange == null)
        {
            return base.ArrangeOverride(finalSize);
        }

        double width = finalSize.Width;
        double height = finalSize.Height;
        CrossPlatformArrange(new Microsoft.Maui.Graphics.Rect(0.0, 0.0, width, height));
        if (base.ClipsToBounds && base.Clip != null && (base.Clip.Bounds.Width != finalSize.Width || base.Clip.Bounds.Height != finalSize.Height))
        {
            base.Clip = new RectangleGeometry
            {
                Rect = new global::Windows.Foundation.Rect(0.0, 0.0, finalSize.Width, finalSize.Height)
            };
        }

        if (nativeGraphicsView != null)
        {
            nativeGraphicsView.Arrange(new global::Windows.Foundation.Rect(0.0, 0.0, width, height));
        }

        return finalSize;
    }

    //
    // Parameters:
    //   availableSize:
    protected override global::Windows.Foundation.Size MeasureOverride(global::Windows.Foundation.Size availableSize)
    {
        if (CrossPlatformMeasure == null)
        {
            return base.MeasureOverride(availableSize);
        }

        double width = availableSize.Width;
        double height = availableSize.Height;
        Microsoft.Maui.Graphics.Size size = CrossPlatformMeasure(width, height);
        width = size.Width;
        height = size.Height;
        if (nativeGraphicsView != null)
        {
            nativeGraphicsView.Measure(availableSize);
        }

        return new global::Windows.Foundation.Size(width, height);
    }

    internal void Dispose()
    {
        if (nativeGraphicsView != null)
        {
            nativeGraphicsView = null;
        }
    }
}
