using Android.App;
using Android.Graphics;
using Android.Views;
using AndroidX.AppCompat.Widget;
using Microsoft.Maui.Handlers;

namespace MAUIToolkit.Core.Internals.Platforms;

//
// Summary:
//     Helper class for Microsoft.Maui.WindowOverlay.
internal static class WindowOverlayHelper
{
    //
    // Summary:
    //     Gets the application window.
    internal static IWindow? window => (IPlatformApplication.Current?.Application as Microsoft.Maui.Controls.Application)?.Windows[(IPlatformApplication.Current?.Application as Microsoft.Maui.Controls.Application).Windows.Count - 1];

    //
    // Summary:
    //     Gets the device density.
    internal static float density => (window != null) ? window.RequestDisplayDensity() : 1f;

    //
    // Summary:
    //     Gets the root view of the device.
    internal static ViewGroup? PlatformRootView => GetPlatformRootView();

    //
    // Summary:
    //     Gets the decor view frame.
    internal static Android.Graphics.Rect? decorViewFrame => UpdateDecorFrame();

    internal static Android.Views.View? decorViewContent => GetPlatformWindow()?.DecorView;

    //
    // Summary:
    //     Helps to get the root view of the device for each platform.
    //
    // Returns:
    //     Root view of the device.
    private static ViewGroup? GetPlatformRootView()
    {
        ViewGroup viewGroup = null;
        Microsoft.Maui.Controls.Application application = IPlatformApplication.Current?.Application as Microsoft.Maui.Controls.Application;
        if (window != null)
        {
            viewGroup = window.Content.ToPlatform() as ViewGroup;
            while (viewGroup != null && !(viewGroup is ContentFrameLayout))
            {
                if (viewGroup.Parent != null)
                {
                    viewGroup = viewGroup.Parent as ViewGroup;
                }
            }
        }

        return viewGroup;
    }

    //
    // Summary:
    //     Gets the activity window for Android.
    //
    // Returns:
    //     Returns the window of the platform view.
    internal static Android.Views.Window? GetPlatformWindow()
    {
        if (window != null && window.Handler is WindowHandler windowHandler)
        {
            Activity platformView = windowHandler.PlatformView;
            if (platformView != null)
            {
                if (platformView == null || platformView.WindowManager == null || platformView.WindowManager.DefaultDisplay == null)
                {
                    return null;
                }

                return platformView.Window;
            }
        }

        return null;
    }

    //
    // Summary:
    //     Helps to get the decor view frame for Android.
    //
    // Returns:
    //     Returns the decor view frame.
    private static Android.Graphics.Rect? UpdateDecorFrame()
    {
        Android.Graphics.Rect rect = null;
        Android.Views.Window platformWindow = GetPlatformWindow();
        if (platformWindow != null)
        {
            if (rect == null)
            {
                rect = new Android.Graphics.Rect();
            }
            else if (rect.Handle != IntPtr.Zero)
            {
                rect.SetEmpty();
            }

            platformWindow.DecorView.GetWindowVisibleDisplayFrame(rect);
        }

        return rect;
    }
}
