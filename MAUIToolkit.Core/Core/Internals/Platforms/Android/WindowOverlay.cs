using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Views;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;

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
    private ViewGroup? rootView;

    private Android.Graphics.Rect? decorViewFrame;

    private float density = 1f;

    //
    // Summary:
    //     Platform view of overlay container.
    private WindowOverlayStack? overlayStack;

    //
    // Summary:
    //     WindowManagerLayoutParams for Window overlay.
    private WindowManagerLayoutParams? WindowManagerLayoutParams;

    //
    // Summary:
    //     Window manager of platform window.
    private IWindowManager? windowManager;

    private IWindow? window;

    private bool hasOverlayStackInRoot = false;

    private readonly Dictionary<Android.Views.View, PositionDetails> positionDetails;

    private WindowOverlayContainer? overlayStackView;

    //
    // Summary:
    //     Returns a MAUIToolkit.Core.Internals.WindowOverlayStack.
    //
    // Parameters:
    //   context:
    //     Passes the information about the view group.
    public virtual WindowOverlayStack CreateStack(Context context)
    {
        WindowOverlayStack windowOverlayStack = null;
        IMauiContext mauiContext = window?.Handler?.MauiContext;
        if (mauiContext != null)
        {
            windowOverlayStack = (WindowOverlayStack)(overlayStackView?.ToPlatform(mauiContext));
        }

        return (windowOverlayStack != null) ? windowOverlayStack : new WindowOverlayStack(context);
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
    public void AddOrUpdate(Microsoft.Maui.Controls.View child, double x, double y, WindowOverlayHorizontalAlignment horizontalAlignment = WindowOverlayHorizontalAlignment.Left, WindowOverlayVerticalAlignment verticalAlignment = WindowOverlayVerticalAlignment.Top)
    {
        AddToWindow();
        if (!hasOverlayStackInRoot || overlayStack == null || child == null)
        {
            return;
        }

        IMauiContext mauiContext = child.Handler?.MauiContext ?? window?.Handler?.MauiContext;
        if (mauiContext != null)
        {
            Android.Views.View view = (Android.Views.View)child.ToPlatform(mauiContext);
            PositionDetails positionDetails;
            if (this.positionDetails.ContainsKey(view))
            {
                positionDetails = this.positionDetails[view];
            }
            else
            {
                positionDetails = new PositionDetails();
                this.positionDetails.Add(view, positionDetails);
            }

            float x2 = (float)x * density;
            float y2 = (float)y * density;
            positionDetails.X = x2;
            positionDetails.Y = y2;
            positionDetails.HorizontalAlignment = horizontalAlignment;
            positionDetails.VerticalAlignment = verticalAlignment;
            if (view.Parent == null)
            {
                overlayStack.AddView(view, new ViewGroup.LayoutParams(-2, -2));
                view.LayoutChange += OnChildLayoutChanged;
            }

            if (view.Width > 0 && view.Height > 0)
            {
                AlignPosition(horizontalAlignment, verticalAlignment, view.Width, view.Height, ref x2, ref y2);
                view.SetX(x2);
                view.SetY(y2);
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
    public void AddOrUpdate(Microsoft.Maui.Controls.View child, Microsoft.Maui.Controls.View relative, double x = 0.0, double y = 0.0, WindowOverlayHorizontalAlignment horizontalAlignment = WindowOverlayHorizontalAlignment.Left, WindowOverlayVerticalAlignment verticalAlignment = WindowOverlayVerticalAlignment.Top)
    {
        AddToWindow();
        if (!hasOverlayStackInRoot || overlayStack == null || child == null || relative == null || relative.Width < 0.0 || relative.Height < 0.0)
        {
            return;
        }

        IMauiContext mauiContext = child.Handler?.MauiContext ?? window?.Handler?.MauiContext;
        if (mauiContext != null && relative.Handler != null && relative.Handler.MauiContext != null)
        {
            Android.Views.View view = (Android.Views.View)child.ToPlatform(mauiContext);
            Android.Views.View view2 = (Android.Views.View)relative.ToPlatform(relative.Handler.MauiContext);
            PositionDetails positionDetails;
            if (this.positionDetails.ContainsKey(view))
            {
                positionDetails = this.positionDetails[view];
            }
            else
            {
                positionDetails = new PositionDetails();
                this.positionDetails.Add(view, positionDetails);
            }

            float x2 = (float)x * density;
            float y2 = (float)y * density;
            positionDetails.X = x2;
            positionDetails.Y = y2;
            positionDetails.HorizontalAlignment = horizontalAlignment;
            positionDetails.VerticalAlignment = verticalAlignment;
            positionDetails.Relative = view2;
            if (view.Parent == null)
            {
                overlayStack.AddView(view, new ViewGroup.LayoutParams(-2, -2));
                view.LayoutChange += OnChildLayoutChanged;
            }

            if (view.Width > 0 && view.Height > 0)
            {
                AlignPositionToRelative(horizontalAlignment, verticalAlignment, view.Width, view.Height, view2.Width, view2.Height, ref x2, ref y2);
                int[] array = new int[2];
                view2.GetLocationOnScreen(array);
                float num = x2 + (float)array[0] - (((float?)decorViewFrame?.Left) ?? 0f);
                float x3 = Math.Max(0f, (num > (((float?)decorViewFrame?.Right) ?? 0f) - (float)view.Width) ? ((((float?)decorViewFrame?.Right) ?? 0f) - (float)view.Width - (((float?)decorViewFrame?.Left) ?? 0f)) : num);
                float num2 = y2 + (float)array[1] - (((float?)decorViewFrame?.Top) ?? 0f);
                float y3 = Math.Max(0f, (num2 > (((float?)decorViewFrame?.Bottom) ?? 0f) - (float)view.Height) ? ((((float?)decorViewFrame?.Bottom) ?? 0f) - (float)view.Height - (((float?)decorViewFrame?.Top) ?? 0f)) : num2);
                view?.SetX(x3);
                view?.SetY(y3);
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
    public void Remove(Microsoft.Maui.Controls.View view)
    {
        if (hasOverlayStackInRoot && view != null && view.Handler != null && view.Handler.MauiContext != null)
        {
            Android.Views.View view2 = (Android.Views.View)view.ToPlatform(view.Handler.MauiContext);
            view2.LayoutChange -= OnChildLayoutChanged;
            view2.RemoveFromParent();
            positionDetails.Remove(view2);
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
            overlayStack.LayoutChange -= OnOverlayStackLayoutChange;
            windowManager.RemoveView(overlayStack);
            WindowManagerLayoutParams = null;
            overlayStack = null;
        }

        decorViewFrame?.Dispose();
        decorViewFrame = null;
        hasOverlayStackInRoot = false;
    }

    //
    // Summary:
    //     Helps to get the WindowManagerLayoutParams for Window overlay.
    //
    // Returns:
    //     Returns the WindowManagerLayoutParams.
    internal WindowManagerLayoutParams GetWindowManagerLayoutParams()
    {
        if (WindowManagerLayoutParams == null)
        {
            WindowManagerLayoutParams = new WindowManagerLayoutParams();
            if (WindowOverlayHelper.decorViewFrame != null)
            {
                WindowManagerLayoutParams.Width = WindowOverlayHelper.decorViewFrame.Right;
                WindowManagerLayoutParams.Height = WindowOverlayHelper.decorViewFrame.Bottom;
            }

            WindowManagerLayoutParams.Format = Format.Translucent;
        }

        return WindowManagerLayoutParams;
    }

    //
    // Summary:
    //     Helps to get the WindowManagerLayoutParams for Window overlay.
    //
    // Returns:
    //     Returns the WindowManagerLayoutParams.
    internal void UpdateWindowManagerLayoutParamsSize()
    {
        WindowManagerLayoutParams.Width = WindowOverlayHelper.decorViewFrame.Right;
        WindowManagerLayoutParams.Height = WindowOverlayHelper.decorViewFrame.Bottom;
        windowManager.UpdateViewLayout(overlayStack, WindowManagerLayoutParams);
    }

    private void Initialize()
    {
        if (hasOverlayStackInRoot || window == null || window.Content == null)
        {
            return;
        }

        density = WindowOverlayHelper.density;
        rootView = WindowOverlayHelper.PlatformRootView;
        if (rootView == null || !(window.Handler is WindowHandler windowHandler) || windowHandler.MauiContext == null)
        {
            return;
        }

        Activity platformView = windowHandler.PlatformView;
        if (platformView == null)
        {
            return;
        }

        decorViewFrame = WindowOverlayHelper.decorViewFrame;
        if (overlayStack == null && windowHandler.MauiContext.Context != null)
        {
            overlayStack = CreateStack(windowHandler.MauiContext.Context);
            if (WindowManagerLayoutParams == null)
            {
                GetWindowManagerLayoutParams();
                WindowManagerLayoutParams.Flags = WindowManagerFlags.LayoutInScreen;
            }

            windowManager = WindowOverlayHelper.GetPlatformWindow().WindowManager;
            windowManager.AddView(overlayStack, WindowManagerLayoutParams);
            overlayStack.LayoutChange += OnOverlayStackLayoutChange;
            overlayStack.BringToFront();
        }

        hasOverlayStackInRoot = true;
    }

    private void OnOverlayStackLayoutChange(object? sender, Android.Views.View.LayoutChangeEventArgs e)
    {
        decorViewFrame = WindowOverlayHelper.decorViewFrame;
    }

    private void OnChildLayoutChanged(object? sender, Android.Views.View.LayoutChangeEventArgs e)
    {
        if (sender == null)
        {
            return;
        }

        Android.Views.View view = (Android.Views.View)sender;
        if (positionDetails.TryGetValue(view, out PositionDetails value) && value != null)
        {
            float x = value.X;
            float y = value.Y;
            Android.Views.View relative = value.Relative;
            if (relative == null && view.Width > 0 && view.Height > 0)
            {
                AlignPosition(value.HorizontalAlignment, value.VerticalAlignment, view.Width, view.Height, ref x, ref y);
                view.SetX(x);
                view.SetY(y);
            }
            else if (relative != null && relative.Width > 0 && relative.Height > 0 && view.Width > 0 && view.Height > 0)
            {
                AlignPositionToRelative(value.HorizontalAlignment, value.VerticalAlignment, view.Width, view.Height, relative.Width, relative.Height, ref x, ref y);
                int[] array = new int[2];
                relative.GetLocationOnScreen(array);
                float num = x + (float)array[0] - (((float?)decorViewFrame?.Left) ?? 0f);
                float x2 = Math.Max(0f, (num > (((float?)decorViewFrame?.Right) ?? 0f) - (float)view.Width) ? ((((float?)decorViewFrame?.Right) ?? 0f) - (float)view.Width - (((float?)decorViewFrame?.Left) ?? 0f)) : num);
                float num2 = y + (float)array[1] - (((float?)decorViewFrame?.Top) ?? 0f);
                float y2 = Math.Max(0f, (num2 > (((float?)decorViewFrame?.Bottom) ?? 0f) - (float)view.Height) ? ((((float?)decorViewFrame?.Bottom) ?? 0f) - (float)view.Height - (((float?)decorViewFrame?.Top) ?? 0f)) : num2);
                view?.SetX(x2);
                view?.SetY(y2);
            }
        }
    }

    private void ClearChildren()
    {
        if (overlayStack == null || positionDetails.Count <= 0)
        {
            return;
        }

        foreach (Android.Views.View key in positionDetails.Keys)
        {
            key.LayoutChange -= OnChildLayoutChanged;
            key.RemoveFromParent();
        }

        positionDetails.Clear();
    }

    //
    // Summary:
    //     Initializes a new instance of the MAUIToolkit.Core.Internals.SfWindowOverlay
    //     class.
    internal WindowOverlay()
    {
        positionDetails = new Dictionary<Android.Views.View, PositionDetails>();
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
}
