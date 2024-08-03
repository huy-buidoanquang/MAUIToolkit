using Microsoft.Maui.Handlers;
using Microsoft.UI.Xaml.Controls;

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
    internal static Panel? PlatformRootView => GetPlatformRootView();

    //
    // Summary:
    //     Helps to get the root view of the device for each platform.
    //
    // Returns:
    //     Root view of the device.
    private static Panel? GetPlatformRootView()
    {
        Panel result = null;
        Microsoft.Maui.Controls.Application application = IPlatformApplication.Current?.Application as Microsoft.Maui.Controls.Application;
        if (window != null && window.Handler is WindowHandler windowHandler)
        {
            Microsoft.UI.Xaml.Window platformView = windowHandler.PlatformView;
            if ((object)platformView != null)
            {
                result = platformView.Content as Panel;
            }
        }

        return result;
    }
}
