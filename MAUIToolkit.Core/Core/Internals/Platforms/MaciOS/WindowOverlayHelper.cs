using UIKit;

namespace MAUIToolkit.Core.Internals.Platforms;

//
// Summary:
//     Helper class for Microsoft.Maui.WindowOverlay.
internal static class WindowOverlayHelper
{
    //
    // Summary:
    //     Gets the application window.
    internal static IWindow? window => (IPlatformApplication.Current?.Application as Application)?.Windows[(IPlatformApplication.Current?.Application as Application).Windows.Count - 1];

    //
    // Summary:
    //     Gets the device density.
    internal static float density => (window != null) ? window.RequestDisplayDensity() : 1f;

    //
    // Summary:
    //     Gets the root view of the device.
    internal static UIView? PlatformRootView => GetPlatformRootView();

    //
    // Summary:
    //     Helps to get the root view of the device for each platform.
    //
    // Returns:
    //     Root view of the device.
    private static UIView? GetPlatformRootView()
    {
        UIView result = null;
        Application application = IPlatformApplication.Current?.Application as Application;
        if (window != null)
        {
            result = window.ToPlatform();
        }

        return result;
    }
}
