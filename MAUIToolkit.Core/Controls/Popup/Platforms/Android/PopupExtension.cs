using Android.Content.Res;
using Android.Views;
using MAUIToolkit.Core.Internals.Platforms;
using ARect = Android.Graphics.Rect;
using AView = Android.Views.View;

namespace MAUIToolkit.Core.Controls.Popup.Platforms.Android;

//
// Summary:
//     Represents the MAUIToolkit.Core.Controls.Popup.PopupExtension class for Android platform.
internal static class PopupExtension
{
    //
    // Summary:
    //     Gets the status bar height.
    //
    // Returns:
    //     Returns the status bar height.
    internal static int GetStatusBarHeight()
    {
        if (WindowOverlayHelper.PlatformRootView != null)
        {
            int[] array = new int[2];
            WindowOverlayHelper.PlatformRootView.GetLocationInWindow(array);
            return (int)((float)array[1] / WindowOverlayHelper.density);
        }

        return 0;
    }

    //
    // Summary:
    //     Gets action bar height.
    //
    // Returns:
    //     Returns the Y coordinates of the page.
    internal static int GetActionBarHeight()
    {
        int[] array = new int[2];
        if (!(GetMainPage()?.Handler?.PlatformView is ViewGroup viewGroup) || (viewGroup != null && viewGroup.Height == 0))
        {
            return 0;
        }

        viewGroup?.GetLocationInWindow(array);
        return (int)Math.Round((float)array[1] / WindowOverlayHelper.density - (float)GetStatusBarHeight());
    }

    //
    // Summary:
    //     Gets the X coordinate of the view.
    //
    // Parameters:
    //   view:
    //     The view.
    //
    // Returns:
    //     Returns the X coordinate of the view.
    internal static int GetX(this Microsoft.Maui.Controls.View view)
    {
        IMauiContext mauiContext = view.Handler?.MauiContext;
        if (mauiContext != null)
        {
            AView view2 = (AView)view.ToPlatform(mauiContext);
            if (view2 != null)
            {
                float x = view2.GetX();
                return (int)Math.Round(x / WindowOverlayHelper.density);
            }
        }

        return 0;
    }

    //
    // Summary:
    //     Gets the Y coordinate of the view.
    //
    // Parameters:
    //   view:
    //     The view.
    //
    // Returns:
    //     Returns the Y coordinate of the view.
    internal static int GetY(this Microsoft.Maui.Controls.View view)
    {
        IMauiContext mauiContext = view.Handler?.MauiContext;
        if (mauiContext != null)
        {
            AView view2 = (AView)view.ToPlatform(mauiContext);
            if (view2 != null)
            {
                float y = view2.GetY();
                return (int)Math.Round(y / WindowOverlayHelper.density);
            }
        }

        return 0;
    }

    //
    // Summary:
    //     Gets the width of the device.
    //
    // Returns:
    //     The width of the device in pixels.
    internal static int GetScreenWidth()
    {
        ViewGroup platformRootView = WindowOverlayHelper.PlatformRootView;
        if (platformRootView == null || (platformRootView != null && platformRootView.Width == 0))
        {
            return (int)((float)Resources.System.DisplayMetrics.WidthPixels / WindowOverlayHelper.density);
        }

        if (DeviceDisplay.MainDisplayInfo.Orientation == DisplayOrientation.Landscape)
        {
            if (platformRootView.Width < platformRootView.Height)
            {
                return (int)Math.Round((float)platformRootView.Height / WindowOverlayHelper.density);
            }

            return (int)Math.Round((float)platformRootView.Width / WindowOverlayHelper.density);
        }

        if (platformRootView.Width < platformRootView.Height)
        {
            return (int)Math.Round((float)platformRootView.Width / WindowOverlayHelper.density);
        }

        return (int)Math.Round((float)platformRootView.Height / WindowOverlayHelper.density);
    }

    //
    // Summary:
    //     Gets the height of the device.
    //
    // Returns:
    //     The height of the device in pixels.
    internal static int GetScreenHeight()
    {
        ViewGroup platformRootView = WindowOverlayHelper.PlatformRootView;
        if (platformRootView == null || (platformRootView != null && platformRootView.Height == 0))
        {
            return (int)((float)Resources.System.DisplayMetrics.HeightPixels / WindowOverlayHelper.density);
        }

        if (DeviceDisplay.MainDisplayInfo.Orientation == DisplayOrientation.Landscape)
        {
            if (platformRootView.Width < platformRootView.Height)
            {
                return (int)Math.Round((float)platformRootView.Width / WindowOverlayHelper.density) + GetStatusBarHeight();
            }

            return (int)Math.Round((float)platformRootView.Height / WindowOverlayHelper.density) + GetStatusBarHeight();
        }

        if (platformRootView.Width < platformRootView.Height)
        {
            return (int)Math.Round((float)platformRootView.Height / WindowOverlayHelper.density) + GetStatusBarHeight();
        }

        return (int)Math.Round((float)platformRootView.Width / WindowOverlayHelper.density) + GetStatusBarHeight();
    }

    //
    // Summary:
    //     Used to applied the blur effect for popup.
    //
    // Parameters:
    //   view:
    //     The instance of the MauiView.
    //
    //   popup:
    //     The instance of the SfPopup.
    //
    //   isopen:
    //     Specifies whether the popup is open or not.
    internal static void Blur(Microsoft.Maui.Controls.View view, CPopup popup, bool isopen)
    {
        if (OperatingSystem.IsAndroidVersionAtLeast(31))
        {
            WindowManagerLayoutParams windowManagerLayoutParams = popup.PopupOverlay.GetWindowManagerLayoutParams();
            windowManagerLayoutParams.Flags = WindowManagerFlags.BlurBehind;
            windowManagerLayoutParams.BlurBehindRadius = (int)popup.GetBlurRadius();
        }
    }

    //
    // Summary:
    //     Used to clear the blur effect for popup.
    //
    // Parameters:
    //   popup:
    //     The instance of the SfPopup.
    internal static void ClearBlurViews(CPopup popup)
    {
        if (!OperatingSystem.IsAndroidVersionAtLeast(31))
        {
            return;
        }

        if (popup.BlurredViews != null && popup.BlurredViews.Count > 0)
        {
            foreach (AView blurredView in popup.BlurredViews)
            {
                blurredView?.SetRenderEffect(null);
            }
        }

        if (popup.BlurredViews != null)
        {
            popup.BlurredViews.Clear();
            popup.BlurredViews = null;
        }
    }

    //
    // Summary:
    //     Calculates the X and Y point of the popup, relative to the given view.
    //
    // Parameters:
    //   popupView:
    //     popup view to display in the view.
    //
    //   relative:
    //     Positions the popup view relatively to the relative view.
    //
    //   position:
    //     The relative position from the view.
    //
    //   absoluteX:
    //     Absolute X Point where the popup should be positioned from the relative view.
    //
    //
    //   absoluteY:
    //     Absolute Y-Point where the popup should be positioned from the relative view.
    //
    //
    //   relativeX:
    //     References the X position of popup relative to view.
    //
    //   relativeY:
    //     References the Y position of popup relative to view.
    internal static void CalculateRelativePoint(PopupView popupView, Microsoft.Maui.Controls.View relative, PopupRelativePosition position, double absoluteX, double absoluteY, ref double relativeX, ref double relativeY)
    {
        ARect decorViewFrame = WindowOverlayHelper.decorViewFrame;
        AView view = (AView)relative.ToPlatform(relative.Handler.MauiContext);
        double num = popupView.Popup.PopupViewWidth * (double)WindowOverlayHelper.density;
        double num2 = popupView.Popup.PopupViewHeight * (double)WindowOverlayHelper.density;
        int height = view.Height;
        int width = view.Width;
        int[] array = new int[2];
        view.GetLocationInWindow(array);
        array[0] += (int)absoluteX;
        array[1] += (int)(absoluteY - (double)((popupView.Popup.OverlayMode == PopupOverlayMode.Blur) ? ((float)GetStatusBarHeight() * WindowOverlayHelper.density) : 0f));
        float num3 = (float)GetScreenHeight() * WindowOverlayHelper.density;
        float num4 = (float)GetScreenWidth() * WindowOverlayHelper.density;
        float num5 = ((popupView.Popup.OverlayMode == PopupOverlayMode.Blur) ? 0f : ((float)GetStatusBarHeight() * WindowOverlayHelper.density));
        float num6 = (float)GetActionBarHeight() * WindowOverlayHelper.density;
        if (position == PopupRelativePosition.AlignToLeftOf || position == PopupRelativePosition.AlignTopLeft || position == PopupRelativePosition.AlignBottomLeft)
        {
            if (popupView.Popup.IsRTL)
            {
                relativeX = Math.Max((double)array[0] - 2.0 * absoluteX - num, 0.0);
            }
            else
            {
                relativeX = (((double)array[0] - num > 0.0) ? ((double)array[0] - num) : 0.0);
            }
        }
        else if (position == PopupRelativePosition.AlignToRightOf || position == PopupRelativePosition.AlignTopRight || position == PopupRelativePosition.AlignBottomRight)
        {
            if (popupView.Popup.IsRTL)
            {
                relativeX = Math.Max((double)array[0] - 2.0 * absoluteX + (double)width - num, 0.0);
                relativeX = popupView.Popup.ValidatePopupPosition(relativeX, num, num4);
            }
            else
            {
                relativeX = (((double)(array[0] + width) + num < (double)num4) ? ((double)(array[0] + width)) : ((double)num4 - num));
            }
        }
        else if (popupView.Popup.IsRTL)
        {
            relativeX = Math.Max((double)array[0] - 2.0 * absoluteX + (double)width - num, 0.0);
            relativeX = popupView.Popup.ValidatePopupPosition(relativeX, num, num4);
        }
        else
        {
            relativeX = (((double)array[0] + num < (double)num4) ? ((double)array[0]) : ((double)num4 - num));
            relativeX = popupView.Popup.ValidatePopupPosition(relativeX, num, num4);
        }

        if (position == PopupRelativePosition.AlignTop || position == PopupRelativePosition.AlignTopLeft || position == PopupRelativePosition.AlignTopRight)
        {
            relativeY = Math.Max(num5 + num6, (double)array[1] - num2);
        }
        else if (position == PopupRelativePosition.AlignBottom || position == PopupRelativePosition.AlignBottomLeft || position == PopupRelativePosition.AlignBottomRight)
        {
            relativeY = (((double)(array[1] + height) + num2 < (double)num3) ? ((double)(array[1] + height)) : ((double)num3 - num2));
        }
        else
        {
            relativeY = Math.Max(num5 + num6, ((double)array[1] + num2 < (double)num3) ? ((double)array[1]) : ((double)num3 - num2));
        }

        relativeX /= WindowOverlayHelper.density;
        relativeY /= WindowOverlayHelper.density;
    }

    //
    // Summary:
    //     Gets the radius to determine blur intensity.
    //
    // Parameters:
    //   popup:
    //     The instance of the SfPopup.
    //
    // Returns:
    //     Return radius value based on the defined blur intensity value.
    private static float GetBlurRadius(this CPopup popup)
    {
        if (popup.PopupStyle.BlurIntensity == PopupBlurIntensity.Light)
        {
            return 11f;
        }

        if (popup.PopupStyle.BlurIntensity == PopupBlurIntensity.ExtraDark)
        {
            return 21f;
        }

        if (popup.PopupStyle.BlurIntensity == PopupBlurIntensity.ExtraLight)
        {
            return 4f;
        }

        if (popup.PopupStyle.BlurIntensity == PopupBlurIntensity.Custom)
        {
            return popup.PopupStyle.BlurRadius;
        }

        return 16f;
    }

    //
    // Summary:
    //     Gets the main page of the application.
    //
    // Parameters:
    //   shouldReturnOnlyMainPage:
    //     bool value to specify whether to return Navigation Page directly without returning
    //     the current page of Navigation Page if the Navigation page is used as the Main
    //     page in Application.
    //
    // Returns:
    //     Returns the main page of the application.
    internal static Page? GetMainPage(bool shouldReturnOnlyMainPage = false)
    {
        if (IPlatformApplication.Current?.Application is Application application && application.MainPage != null)
        {
            if (application.MainPage.Navigation != null && application.MainPage.Navigation.ModalStack != null)
            {
                Page page = application.MainPage.Navigation.ModalStack.LastOrDefault();
                if (page != null)
                {
                    return page;
                }
            }

            if (application.MainPage is NavigationPage navigationPage && !shouldReturnOnlyMainPage)
            {
                if (navigationPage?.CurrentPage == null)
                {
                    return new Page();
                }

                return navigationPage?.CurrentPage;
            }

            if (application.MainPage is Shell shell)
            {
                if (shell?.CurrentPage == null)
                {
                    return new Page();
                }

                return shell?.CurrentPage;
            }

            return application.MainPage;
        }

        return new Page();
    }

    //
    // Summary:
    //     This method will returns the SafeAreaHeight.
    //
    // Parameters:
    //   position:
    //     Position of the safe area.
    //
    // Returns:
    //     Returns the SafeAreaHeight.
    internal static int GetSafeAreaHeight(string position)
    {
        return 0;
    }
}
