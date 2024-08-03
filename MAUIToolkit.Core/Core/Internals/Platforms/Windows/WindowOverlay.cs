using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Media;

namespace MAUIToolkit.Core.Internals.Platforms;

//
// Summary:
//     The MAUIToolkit.Core.Internals.SfWindowOverlay allows the users to add or
//     update an independent Microsoft.Maui.Controls.View to float above the application
//     window. The AddOrUpdate() methods allows you to position it both absolutely and
//     relatively. The passed view is eliminated from the floating window via the Remove()
//     function. Using the RemoveFromWindow() method, you can also delete all floating
//     views.
internal class WindowOverlay
{
    private IWindow? window;

    private bool hasOverlayStackInRoot = false;

    private readonly Dictionary<FrameworkElement, PositionDetails> positionDetails;

    private WindowOverlayContainer? overlayStackView;

    private Panel? rootView;

    private WindowOverlayStack? overlayStack;

    private Popup? PrimitivePopup;

    //
    // Summary:
    //     Initializes a new instance of the MAUIToolkit.Core.Internals.SfWindowOverlay
    //     class.
    internal WindowOverlay()
    {
        positionDetails = new Dictionary<FrameworkElement, PositionDetails>();
    }

    internal void SetWindowOverlayContainer(WindowOverlayContainer view)
    {
        overlayStackView = view;
    }

    //
    // Summary:
    //     Gets the application windows and adds a new overlay stack for including the independent
    //     views.
    public void AddToWindow()
    {
        if (!hasOverlayStackInRoot)
        {
            window = WindowOverlayHelper.window;
            Initialize();
        }
    }

    //
    // Summary:
    //     Calculates a new absolute position based on the given alignment and size.
    //
    // Parameters:
    //   horizontalAlignment:
    //
    //   verticalAlignment:
    //
    //   width:
    //
    //   height:
    //
    //   x:
    //
    //   y:
    private void AlignPosition(WindowOverlayHorizontalAlignment horizontalAlignment, WindowOverlayVerticalAlignment verticalAlignment, float width, float height, ref float x, ref float y)
    {
        switch (horizontalAlignment)
        {
            case WindowOverlayHorizontalAlignment.Right:
                x -= width;
                break;
            case WindowOverlayHorizontalAlignment.Center:
                x -= width / 2f;
                break;
        }

        switch (verticalAlignment)
        {
            case WindowOverlayVerticalAlignment.Bottom:
                y -= height;
                break;
            case WindowOverlayVerticalAlignment.Center:
                y -= height / 2f;
                break;
        }
    }

    //
    // Summary:
    //     Calculates a new relative position based on the given alignment, relative view
    //     size, and child size.
    //
    // Parameters:
    //   horizontalAlignment:
    //
    //   verticalAlignment:
    //
    //   childWidth:
    //
    //   childHeight:
    //
    //   relativeViewWidth:
    //
    //   relativeViewHeight:
    //
    //   x:
    //
    //   y:
    private void AlignPositionToRelative(WindowOverlayHorizontalAlignment horizontalAlignment, WindowOverlayVerticalAlignment verticalAlignment, float childWidth, float childHeight, float relativeViewWidth, float relativeViewHeight, ref float x, ref float y)
    {
        switch (horizontalAlignment)
        {
            case WindowOverlayHorizontalAlignment.Right:
                x += relativeViewWidth;
                break;
            case WindowOverlayHorizontalAlignment.Center:
                x += relativeViewWidth / 2f - childWidth / 2f;
                break;
            case WindowOverlayHorizontalAlignment.Left:
                x += 0f - childWidth;
                break;
        }

        switch (verticalAlignment)
        {
            case WindowOverlayVerticalAlignment.Bottom:
                y += relativeViewHeight;
                break;
            case WindowOverlayVerticalAlignment.Center:
                y += relativeViewHeight / 2f - childHeight / 2f;
                break;
            case WindowOverlayVerticalAlignment.Top:
                y += 0f - childHeight;
                break;
        }
    }

    //
    // Summary:
    //     Returns a MAUIToolkit.Core.Internals.WindowOverlayStack.
    public virtual WindowOverlayStack CreateStack()
    {
        WindowOverlayStack windowOverlayStack = null;
        IMauiContext mauiContext = window?.Handler?.MauiContext;
        if (mauiContext != null)
        {
            windowOverlayStack = (WindowOverlayStack)(overlayStackView?.ToPlatform(mauiContext));
        }

        return (windowOverlayStack != null) ? windowOverlayStack : new WindowOverlayStack();
    }

    //
    // Summary:
    //     Adds or updates the child layout absolutely to the overlay stack.
    //
    // Parameters:
    //   child:
    //     Adds the child to the floating window.
    //
    //   x:
    //     Positions the child in the x point from the application left.
    //
    //   y:
    //     Positions the child in the y point from the application top.
    //
    //   horizontalAlignment:
    //     The horizontal alignment behaves as like below,
    //
    //     • For MAUIToolkit.Core.Internals.WindowOverlayHorizontalAlignment.Left, the
    //     child left position will starts from the x.
    //     • For MAUIToolkit.Core.Internals.WindowOverlayHorizontalAlignment.Right,
    //     the child right position will starts from the x.
    //     • For MAUIToolkit.Core.Internals.WindowOverlayHorizontalAlignment.Center,
    //     the child center position will be the x.
    //
    //   verticalAlignment:
    //     The vertical alignment behaves as like below,
    //
    //     • For MAUIToolkit.Core.Internals.WindowOverlayVerticalAlignment.Top, the
    //     child top position will starts from the y.
    //     • For MAUIToolkit.Core.Internals.WindowOverlayVerticalAlignment.Bottom, the
    //     child bottom position will starts from the y.
    //     • For MAUIToolkit.Core.Internals.WindowOverlayVerticalAlignment.Center, the
    //     child center position will be the y.
    public void AddOrUpdate(View child, double x, double y, WindowOverlayHorizontalAlignment horizontalAlignment = WindowOverlayHorizontalAlignment.Left, WindowOverlayVerticalAlignment verticalAlignment = WindowOverlayVerticalAlignment.Top)
    {
        AddToWindow();
        if (!hasOverlayStackInRoot || overlayStack == null || child == null)
        {
            return;
        }

        IMauiContext mauiContext = window?.Handler?.MauiContext;
        if (mauiContext != null)
        {
            FrameworkElement frameworkElement = child.ToPlatform(mauiContext);
            PositionDetails positionDetails;
            if (this.positionDetails.ContainsKey(frameworkElement))
            {
                positionDetails = this.positionDetails[frameworkElement];
            }
            else
            {
                positionDetails = new PositionDetails();
                this.positionDetails.Add(frameworkElement, positionDetails);
            }

            float x2 = (float)x;
            float y2 = (float)y;
            positionDetails.X = x2;
            positionDetails.Y = y2;
            positionDetails.HorizontalAlignment = horizontalAlignment;
            positionDetails.VerticalAlignment = verticalAlignment;
            if (!overlayStack.Children.Contains(frameworkElement))
            {
                overlayStack.Children.Add(frameworkElement);
                frameworkElement.LayoutUpdated += OnChildLayoutChanged;
            }

            if (frameworkElement.DesiredSize.Width <= 0.0 || frameworkElement.DesiredSize.Height <= 0.0)
            {
                frameworkElement.Measure(new Windows.Foundation.Size(double.PositiveInfinity, double.PositiveInfinity));
            }

            AlignPosition(horizontalAlignment, verticalAlignment, (float)frameworkElement.DesiredSize.Width, (float)frameworkElement.DesiredSize.Height, ref x2, ref y2);
            Canvas.SetLeft(frameworkElement, x2);
            Canvas.SetTop(frameworkElement, y2);
        }
    }

    //
    // Summary:
    //     Adds or updates the child layout relatively to the overlay stack. After the relative
    //     positioning, the x and y will the added with the left and top positions.
    //
    // Parameters:
    //   child:
    //     Adds the child to the floating window.
    //
    //   relative:
    //     Positions the child relatively to the relative view.
    //
    //   x:
    //     Adds the x point to the child left after the relative positioning.
    //
    //   y:
    //     Adds the y point to the child top after the relative positioning.
    //
    //   horizontalAlignment:
    //     The horizontal alignment behaves as like below,
    //
    //     • For MAUIToolkit.Core.Internals.WindowOverlayHorizontalAlignment.Left, the
    //     child left position will starts from the relative.Left.
    //     • For MAUIToolkit.Core.Internals.WindowOverlayHorizontalAlignment.Right,
    //     the child right position will starts from the relative.Right.
    //     • For MAUIToolkit.Core.Internals.WindowOverlayHorizontalAlignment.Center,
    //     the child center position will be the relative.Center.
    //
    //   verticalAlignment:
    //     The vertical alignment behaves as like below,
    //
    //     • For MAUIToolkit.Core.Internals.WindowOverlayVerticalAlignment.Top, the
    //     child bottom position will starts from the relative.Top.
    //     • For MAUIToolkit.Core.Internals.WindowOverlayVerticalAlignment.Bottom, the
    //     child top position will starts from the relative.Bottom.
    //     • For MAUIToolkit.Core.Internals.WindowOverlayVerticalAlignment.Center, the
    //     child center position will be the relative.Center.
    public void AddOrUpdate(View child, View relative, double x = 0.0, double y = 0.0, WindowOverlayHorizontalAlignment horizontalAlignment = WindowOverlayHorizontalAlignment.Left, WindowOverlayVerticalAlignment verticalAlignment = WindowOverlayVerticalAlignment.Top)
    {
        AddToWindow();
        if (!hasOverlayStackInRoot || overlayStack == null || child == null || relative == null || relative.Width < 0.0 || relative.Height < 0.0)
        {
            return;
        }

        IMauiContext mauiContext = window?.Handler?.MauiContext;
        if (mauiContext != null && relative.Handler != null && relative.Handler.MauiContext != null)
        {
            FrameworkElement frameworkElement = child.ToPlatform(mauiContext);
            FrameworkElement frameworkElement2 = relative.ToPlatform(relative.Handler.MauiContext);
            PositionDetails positionDetails;
            if (this.positionDetails.ContainsKey(frameworkElement))
            {
                positionDetails = this.positionDetails[frameworkElement];
            }
            else
            {
                positionDetails = new PositionDetails();
                this.positionDetails.Add(frameworkElement, positionDetails);
            }

            float x2 = (float)x;
            float y2 = (float)y;
            positionDetails.X = x2;
            positionDetails.Y = y2;
            positionDetails.HorizontalAlignment = horizontalAlignment;
            positionDetails.VerticalAlignment = verticalAlignment;
            positionDetails.Relative = frameworkElement2;
            if (!overlayStack.Children.Contains(frameworkElement))
            {
                overlayStack.Children.Add(frameworkElement);
                frameworkElement.LayoutUpdated += OnChildLayoutChanged;
            }

            if (frameworkElement.DesiredSize.Width <= 0.0 && frameworkElement.DesiredSize.Height <= 0.0)
            {
                frameworkElement.Measure(new Windows.Foundation.Size(double.PositiveInfinity, double.PositiveInfinity));
            }

            AlignPositionToRelative(horizontalAlignment, verticalAlignment, (float)frameworkElement.DesiredSize.Width, (float)frameworkElement.DesiredSize.Height, frameworkElement2.ActualSize.X, frameworkElement2.ActualSize.Y, ref x2, ref y2);
            GeneralTransform generalTransform = frameworkElement2.TransformToVisual(rootView);
            Windows.Foundation.Point point = generalTransform.TransformPoint(new Windows.Foundation.Point(0f, 0f));
            if (!(rootView == null))
            {
                double num = (double)x2 + point.X;
                double length = Math.Max(0.0, (num > rootView.DesiredSize.Width - frameworkElement.DesiredSize.Width) ? (rootView.DesiredSize.Width - frameworkElement.DesiredSize.Width) : num);
                double num2 = (double)y2 + point.Y;
                double length2 = Math.Max(0.0, (num2 > rootView.DesiredSize.Height - frameworkElement.DesiredSize.Height) ? (rootView.DesiredSize.Height - frameworkElement.DesiredSize.Height) : num2);
                Canvas.SetLeft(frameworkElement, length);
                Canvas.SetTop(frameworkElement, length2);
            }
        }
    }

    //
    // Summary:
    //     Eliminates the view from the floating window.
    //
    // Parameters:
    //   view:
    //     Specifies the view to be removed from the floating window.
    public void Remove(View view)
    {
        if (hasOverlayStackInRoot && view != null && view.Handler != null && view.Handler.MauiContext != null)
        {
            FrameworkElement frameworkElement = view.ToPlatform(view.Handler.MauiContext);
            frameworkElement.LayoutUpdated -= OnChildLayoutChanged;
            overlayStack?.Children.Remove(frameworkElement);
            positionDetails.Remove(frameworkElement);
        }
    }

    //
    // Summary:
    //     Removes the current overlay window from root view with all its children.
    public void RemoveFromWindow()
    {
        if (overlayStack != null)
        {
            ClearChildren();
            PrimitivePopup.Child = null;
            PrimitivePopup = null;
            overlayStack = null;
        }

        hasOverlayStackInRoot = false;
    }

    //
    // Summary:
    //     Update the size of overlaystack.
    internal void UpdateOverlaySize()
    {
        if (WindowOverlayHelper.PlatformRootView != null)
        {
            overlayStack.Width = WindowOverlayHelper.PlatformRootView.DesiredSize.Width;
            overlayStack.Height = WindowOverlayHelper.PlatformRootView.DesiredSize.Height;
        }
    }

    private void Initialize()
    {
        if (hasOverlayStackInRoot || window == null || window.Content == null)
        {
            return;
        }

        rootView = WindowOverlayHelper.PlatformRootView;
        if (!(rootView == null))
        {
            if ((object)overlayStack == null)
            {
                overlayStack = CreateStack();
            }

            PrimitivePopup = new Popup();
            UpdateOverlaySize();
            PrimitivePopup.Child = overlayStack;
            if (rootView.XamlRoot != null)
            {
                PrimitivePopup.XamlRoot = rootView.XamlRoot;
                PrimitivePopup.IsOpen = true;
            }

            overlayStack.SetValue(Canvas.ZIndexProperty, 99);
            hasOverlayStackInRoot = true;
        }
    }

    private void OnChildLayoutChanged(object? sender, object e)
    {
        if (sender == null)
        {
            return;
        }

        FrameworkElement frameworkElement = (FrameworkElement)sender;
        if (!positionDetails.TryGetValue(frameworkElement, out PositionDetails value) || value == null)
        {
            return;
        }

        FrameworkElement relative = value.Relative;
        float x = value.X;
        float y = value.Y;
        if (relative == null && frameworkElement.Width > 0.0 && frameworkElement.Height > 0.0)
        {
            AlignPosition(value.HorizontalAlignment, value.VerticalAlignment, (float)frameworkElement.DesiredSize.Width, (float)frameworkElement.DesiredSize.Height, ref x, ref y);
            Canvas.SetLeft(frameworkElement, x);
            Canvas.SetTop(frameworkElement, y);
        }
        else if (relative != null && relative.Width > 0.0 && relative.Height > 0.0)
        {
            AlignPositionToRelative(value.HorizontalAlignment, value.VerticalAlignment, (float)frameworkElement.DesiredSize.Width, (float)frameworkElement.DesiredSize.Height, relative.ActualSize.X, relative.ActualSize.Y, ref x, ref y);
            GeneralTransform generalTransform = relative.TransformToVisual(rootView);
            Windows.Foundation.Point point = generalTransform.TransformPoint(new Windows.Foundation.Point(0f, 0f));
            if (!(rootView == null))
            {
                double num = (double)x + point.X - (double)value.X;
                double length = Math.Max(value.X, (num > rootView.DesiredSize.Width - frameworkElement.DesiredSize.Width) ? (rootView.DesiredSize.Width - frameworkElement.DesiredSize.Width) : num);
                double num2 = (double)y + point.Y - (double)value.Y;
                double length2 = Math.Max(value.Y, (num2 > rootView.DesiredSize.Height - frameworkElement.DesiredSize.Height) ? (rootView.DesiredSize.Height - frameworkElement.DesiredSize.Height) : num2);
                Canvas.SetLeft(frameworkElement, length);
                Canvas.SetTop(frameworkElement, length2);
            }
        }
    }

    private void ClearChildren()
    {
        if (!(overlayStack != null) || positionDetails.Count <= 0)
        {
            return;
        }

        foreach (FrameworkElement key in positionDetails.Keys)
        {
            key.LayoutUpdated -= OnChildLayoutChanged;
            overlayStack.Children.Remove(key);
        }

        positionDetails.Clear();
    }
}
