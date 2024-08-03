using CoreGraphics;
using Microsoft.Maui.Platform;
using UIKit;

namespace MAUIToolkit.Core.Internals.Platforms;

//
// Summary:
//     The MAUIToolkit.Core.Internals.Platforms.WindowOverlay allows the users to add or
//     update an independent Microsoft.Maui.Controls.View to float above the application
//     window. The AddOrUpdate() methods allows you to position it both absolutely and
//     relatively. The passed view is eliminated from the floating window via the Remove()
//     function. Using the RemoveFromWindow() method, you can also delete all floating
//     views.
internal class WindowOverlay
{
    private IWindow? window;

    private bool hasOverlayStackInRoot = false;

    private readonly Dictionary<UIView, PositionDetails> positionDetails;

    private WindowOverlayContainer? overlayStackView;

    private UIView? rootView;

    private WindowOverlayStack? overlayStack;

    //
    // Summary:
    //     Initializes a new instance of the MAUIToolkit.Core.Internals.Platforms.WindowOverlay
    //     class.
    internal WindowOverlay()
    {
        positionDetails = new Dictionary<UIView, PositionDetails>();
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
    //     Returns a MAUIToolkit.Core.Internals.Platforms.WindowOverlayStack.
    public virtual WindowOverlayStack CreateStack()
    {
        WindowOverlayStack windowOverlayStack = null;
        IMauiContext mauiContext = window?.Handler?.MauiContext;
        if (mauiContext != null)
        {
            windowOverlayStack = (WindowOverlayStack)(overlayStackView?.ToPlatform(mauiContext));
            if (windowOverlayStack != null && overlayStackView != null)
            {
                windowOverlayStack.canHandleTouch = !overlayStackView.canHandleTouch;
            }
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
    //     • For MAUIToolkit.Core.Internals.Platforms.WindowOverlayHorizontalAlignment.Left, the
    //     child left position will starts from the x.
    //     • For MAUIToolkit.Core.Internals.Platforms.WindowOverlayHorizontalAlignment.Right,
    //     the child right position will starts from the x.
    //     • For MAUIToolkit.Core.Internals.Platforms.WindowOverlayHorizontalAlignment.Center,
    //     the child center position will be the x.
    //
    //   verticalAlignment:
    //     The vertical alignment behaves as like below,
    //
    //     • For MAUIToolkit.Core.Internals.Platforms.WindowOverlayVerticalAlignment.Top, the
    //     child top position will starts from the y.
    //     • For MAUIToolkit.Core.Internals.Platforms.WindowOverlayVerticalAlignment.Bottom, the
    //     child bottom position will starts from the y.
    //     • For MAUIToolkit.Core.Internals.Platforms.WindowOverlayVerticalAlignment.Center, the
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
            UIView uIView = child.ToPlatform(mauiContext);
            PositionDetails positionDetails;
            if (this.positionDetails.ContainsKey(uIView))
            {
                positionDetails = this.positionDetails[uIView];
            }
            else
            {
                positionDetails = new PositionDetails();
                this.positionDetails.Add(uIView, positionDetails);
            }

            float x2 = (float)x;
            float y2 = (float)y;
            positionDetails.X = x2;
            positionDetails.Y = y2;
            positionDetails.HorizontalAlignment = horizontalAlignment;
            positionDetails.VerticalAlignment = verticalAlignment;
            if (!uIView.IsDescendantOfView(overlayStack))
            {
                overlayStack.AddSubview(uIView);
                uIView.Frame = overlayStack.Frame;
            }

            uIView.SizeToFit();
            if (!uIView.Frame.IsEmpty)
            {
                AlignPosition(horizontalAlignment, verticalAlignment, (float)uIView.Frame.Width, (float)uIView.Frame.Height, ref x2, ref y2);
                uIView.Frame = new CGRect(x2, y2, uIView.Frame.Width, uIView.Frame.Height);
            }
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
    //     • For MAUIToolkit.Core.Internals.Platforms.WindowOverlayHorizontalAlignment.Left, the
    //     child left position will starts from the relative.Left.
    //     • For MAUIToolkit.Core.Internals.Platforms.WindowOverlayHorizontalAlignment.Right,
    //     the child right position will starts from the relative.Right.
    //     • For MAUIToolkit.Core.Internals.Platforms.WindowOverlayHorizontalAlignment.Center,
    //     the child center position will be the relative.Center.
    //
    //   verticalAlignment:
    //     The vertical alignment behaves as like below,
    //
    //     • For MAUIToolkit.Core.Internals.Platforms.WindowOverlayVerticalAlignment.Top, the
    //     child bottom position will starts from the relative.Top.
    //     • For MAUIToolkit.Core.Internals.Platforms.WindowOverlayVerticalAlignment.Bottom, the
    //     child top position will starts from the relative.Bottom.
    //     • For MAUIToolkit.Core.Internals.Platforms.WindowOverlayVerticalAlignment.Center, the
    //     child center position will be the relative.Center.
    public void AddOrUpdate(View child, View relative, double x = 0.0, double y = 0.0, WindowOverlayHorizontalAlignment horizontalAlignment = WindowOverlayHorizontalAlignment.Left, WindowOverlayVerticalAlignment verticalAlignment = WindowOverlayVerticalAlignment.Top)
    {
        AddToWindow();
        if (!hasOverlayStackInRoot || overlayStack == null || child == null || relative == null || relative.Frame.Width < 0.0 || relative.Frame.Height < 0.0)
        {
            return;
        }

        IMauiContext mauiContext = window?.Handler?.MauiContext;
        if (mauiContext != null && relative.Handler != null && relative.Handler.MauiContext != null)
        {
            UIView uIView = child.ToPlatform(mauiContext);
            UIView uIView2 = relative.ToPlatform(relative.Handler.MauiContext);
            PositionDetails positionDetails;
            if (this.positionDetails.ContainsKey(uIView))
            {
                positionDetails = this.positionDetails[uIView];
            }
            else
            {
                positionDetails = new PositionDetails();
                this.positionDetails.Add(uIView, positionDetails);
            }

            float x2 = (float)x;
            float y2 = (float)y;
            positionDetails.X = x2;
            positionDetails.Y = y2;
            positionDetails.HorizontalAlignment = horizontalAlignment;
            positionDetails.VerticalAlignment = verticalAlignment;
            positionDetails.Relative = uIView2;
            if (!uIView.IsDescendantOfView(overlayStack))
            {
                overlayStack.AddSubview(uIView);
                uIView.Frame = overlayStack.Frame;
                uIView.SizeToFit();
            }

            AlignPositionToRelative(horizontalAlignment, verticalAlignment, (float)uIView.Frame.Width, (float)uIView.Frame.Height, (float)uIView2.Frame.Width, (float)uIView2.Frame.Height, ref x2, ref y2);
            CGPoint cGPoint = uIView2.ConvertPointToView(new CGPoint(0f, 0f), rootView);
            if (rootView != null)
            {
                double num = x2 + cGPoint.X;
                double x3 = Math.Max(0.0, (num > (double)(float)(rootView.Frame.Width - uIView.Frame.Width)) ? ((double)(float)(rootView.Frame.Width - uIView.Frame.Width)) : num);
                double num2 = y2 + cGPoint.Y;
                double y3 = Math.Max(0.0, (num2 > (double)(float)(rootView.Frame.Height - uIView.Frame.Height)) ? ((double)(float)(rootView.Frame.Height - uIView.Frame.Height)) : num2);
                uIView.Frame = new CGRect(x3, y3, uIView.Frame.Width, uIView.Frame.Height);
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
            UIView uIView = view.ToPlatform(view.Handler.MauiContext);
            uIView.RemoveFromSuperview();
            positionDetails.Remove(uIView);
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
            overlayStack.RemoveFromSuperview();
            overlayStack = null;
        }

        hasOverlayStackInRoot = false;
    }

    private void Initialize()
    {
        if (hasOverlayStackInRoot || window == null || window.Content == null)
        {
            return;
        }

        rootView = WindowOverlayHelper.PlatformRootView;
        if (rootView != null)
        {
            if (overlayStack == null)
            {
                overlayStack = CreateStack();
            }

            if (!rootView.Subviews.Contains(overlayStack))
            {
                rootView.AddSubview(overlayStack);
                rootView.BringSubviewToFront(overlayStack);
                overlayStack.Frame = rootView.Frame;
                overlayStack.AutoresizingMask = UIViewAutoresizing.All;
            }

            hasOverlayStackInRoot = true;
        }
    }

    private void ClearChildren()
    {
        if (overlayStack != null && positionDetails.Count > 0)
        {
            overlayStack.ClearSubviews();
            positionDetails.Clear();
        }
    }
}

