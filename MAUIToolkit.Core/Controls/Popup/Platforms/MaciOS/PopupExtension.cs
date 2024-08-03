using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using CoreGraphics;
using MAUIToolkit.Core.Internals.Platforms;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;
using Microsoft.Maui.Platform;
using UIKit;

namespace MAUIToolkit.Core.Controls.Popup;

//
// Summary:
//     Represents the MAUIToolkit.Core.Controls.Popup.PopupExtension class.
internal static class PopupExtension
{
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
    internal static Microsoft.Maui.Controls.Page? GetMainPage(bool shouldReturnOnlyMainPage = false)
    {
        if (IPlatformApplication.Current?.Application is Microsoft.Maui.Controls.Application application && application.MainPage != null)
        {
            if (application.MainPage.Navigation != null && application.MainPage.Navigation.ModalStack != null)
            {
                Microsoft.Maui.Controls.Page page = application.MainPage.Navigation.ModalStack.LastOrDefault();
                if (page != null)
                {
                    return page;
                }
            }

            if (application.MainPage is Microsoft.Maui.Controls.NavigationPage navigationPage && !shouldReturnOnlyMainPage)
            {
                if (navigationPage?.CurrentPage == null)
                {
                    return new Microsoft.Maui.Controls.Page();
                }

                return navigationPage?.CurrentPage;
            }

            if (application.MainPage is Shell shell)
            {
                if (shell?.CurrentPage == null)
                {
                    return new Microsoft.Maui.Controls.Page();
                }

                return shell?.CurrentPage;
            }

            return application.MainPage;
        }

        return new Microsoft.Maui.Controls.Page();
    }

    //
    // Summary:
    //     Gets the width of the device.
    //
    // Returns:
    //     The width of the device.
    internal static int GetScreenWidth()
    {
        UIView platformRootView = WindowOverlayHelper.PlatformRootView;
        return (platformRootView != null) ? ((int)platformRootView.Frame.Width) : 0;
    }

    //
    // Summary:
    //     Gets the height of the device.
    //
    // Returns:
    //     The height of the device.
    internal static int GetScreenHeight()
    {
        UIView platformRootView = WindowOverlayHelper.PlatformRootView;
        return (platformRootView != null) ? ((int)platformRootView.Frame.Height) : 0;
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
    internal static int GetX(this View view)
    {
        IMauiContext mauiContext = view.Handler?.MauiContext;
        if (mauiContext != null)
        {
            UIView uIView = Internals.Platforms.ElementExtensions.ToPlatform(view, mauiContext);
            if (uIView != null)
            {
                return (int)uIView.Frame.X;
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
    internal static int GetY(this View view)
    {
        IMauiContext mauiContext = view.Handler?.MauiContext;
        if (mauiContext != null)
        {
            UIView uIView = Internals.Platforms.ElementExtensions.ToPlatform(view, mauiContext);
            if (uIView != null)
            {
                return (int)uIView.Frame.Y;
            }
        }

        return 0;
    }

    //
    // Summary:
    //     Gets the status bar height.
    //
    // Returns:
    //     Returns the status bar height.
    internal static int GetStatusBarHeight()
    {
        return (WindowOverlayHelper.window?.ToPlatform() is UIWindow uIWindow) ? ((int)uIWindow.SafeAreaInsets.Top) : 0;
    }

    //
    // Summary:
    //     Gets action bar height.
    //
    // Returns:
    //     Returns action bar height.
    internal static int GetActionBarHeight()
    {
        return 0;
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
        if (Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific.Page.GetUseSafeArea(GetMainPage()))
        {
            UIWindow uIWindow = WindowOverlayHelper.window?.ToPlatform() as UIWindow;
            UIInterfaceOrientation? uIInterfaceOrientation = uIWindow?.WindowScene?.InterfaceOrientation;
            if (position == "Top")
            {
                UIView navigationBar = GetNavigationBar();
                return (navigationBar != null) ? ((int)navigationBar.Frame.Height) : 0;
            }

            if ((uIInterfaceOrientation == UIInterfaceOrientation.Portrait || uIInterfaceOrientation == UIInterfaceOrientation.PortraitUpsideDown || uIInterfaceOrientation == UIInterfaceOrientation.LandscapeLeft || uIInterfaceOrientation == UIInterfaceOrientation.LandscapeRight || UIDevice.CurrentDevice.Orientation == UIDeviceOrientation.FaceUp || UIDevice.CurrentDevice.Orientation == UIDeviceOrientation.FaceDown) && position == "Bottom")
            {
                return (uIWindow?.SafeAreaInsets.Bottom > 0) ? ((int)uIWindow.SafeAreaInsets.Bottom) : 0;
            }

            if ((UIDevice.CurrentDevice.Orientation == UIDeviceOrientation.FaceUp || UIDevice.CurrentDevice.Orientation == UIDeviceOrientation.FaceDown) && position == "Right" && GetScreenWidth() > GetScreenHeight())
            {
                return (uIWindow?.SafeAreaInsets.Right > 0) ? ((int)uIWindow.SafeAreaInsets.Right) : 0;
            }

            if ((UIDevice.CurrentDevice.Orientation == UIDeviceOrientation.FaceUp || UIDevice.CurrentDevice.Orientation == UIDeviceOrientation.FaceDown) && position == "Left" && GetScreenWidth() > GetScreenHeight())
            {
                return (uIWindow?.SafeAreaInsets.Left > 0) ? ((int)uIWindow.SafeAreaInsets.Left) : 0;
            }

            if (uIInterfaceOrientation == UIInterfaceOrientation.LandscapeLeft)
            {
                return (uIWindow?.SafeAreaInsets.Left > 0) ? ((int)uIWindow.SafeAreaInsets.Left) : 0;
            }

            if (uIInterfaceOrientation == UIInterfaceOrientation.LandscapeRight)
            {
                return (uIWindow?.SafeAreaInsets.Right > 0) ? ((int)uIWindow.SafeAreaInsets.Right) : 0;
            }
        }

        return 0;
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
    //     The instance of the CPopup.
    //
    //   isopen:
    //     Specifies whether the popup is open or not.
    internal static void Blur(this View view, CPopup popup, bool isopen)
    {
        CPopup popup2 = popup;
        if (!isopen)
        {
            return;
        }

        popup2.BlurView = new UIVisualEffectView(UIBlurEffect.FromStyle(popup2.GetUIBlurStyle()))
        {
            Frame = new RectangleF(0f, 0f, GetScreenWidth(), GetScreenHeight())
        };
        if (popup2.PopupStyle.BlurIntensity == PopupBlurIntensity.Custom)
        {
            popup2.BlurView.Effect = null;
            UIViewPropertyAnimator uIViewPropertyAnimator = new UIViewPropertyAnimator();
            uIViewPropertyAnimator.AddAnimations(delegate
            {
                popup2.BlurView.Effect = UIBlurEffect.FromStyle(popup2.GetUIBlurStyle());
            });
            uIViewPropertyAnimator.FractionComplete = popup2.PopupStyle.BlurRadius;
            uIViewPropertyAnimator = null;
        }

        if (WindowOverlayHelper.PlatformRootView != null)
        {
            popup2.PopupOverlayContainer.ToPlatform()?.InsertSubview(popup2.BlurView, 0);
        }
    }

    //
    // Summary:
    //     Used to clear the blur effect for popup.
    //
    // Parameters:
    //   popup:
    //     The instance of the CPopup.
    internal static void ClearBlurViews(CPopup popup)
    {
        if (popup.BlurView != null)
        {
            popup.BlurView?.RemoveFromSuperview();
            popup.BlurView?.Dispose();
            popup.BlurView = null;
        }
    }

    //
    // Summary:
    //     Updates blurios frame when resize the window.
    //
    // Parameters:
    //   popup:
    //     The instance of the CPopup.
    internal static void UpdateBluriosFrame(CPopup popup)
    {
        if (popup.BlurView != null)
        {
            popup.BlurView.Frame = new RectangleF(0f, 0f, GetScreenWidth(), GetScreenHeight());
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
    internal static void CalculateRelativePoint(PopupView popupView, View relative, PopupRelativePosition position, double absoluteX, double absoluteY, ref double relativeX, ref double relativeY)
    {
        UIView platformRootView = WindowOverlayHelper.PlatformRootView;
        UIView uIView = Internals.Platforms.ElementExtensions.ToPlatform(relative, relative.Handler.MauiContext);
        double popupViewWidth = popupView.Popup.PopupViewWidth;
        double popupViewHeight = popupView.Popup.PopupViewHeight;
        NFloat height = uIView.Frame.Height;
        NFloat width = uIView.Frame.Width;
        NFloat[] array = new NFloat[2];
        CGPoint cGPoint = uIView.ConvertPointToView(new CGPoint(0f, 0f), platformRootView);
        array[0] = cGPoint.X;
        array[1] = cGPoint.Y;
        array[0] += (NFloat)(float)absoluteX;
        array[1] += (NFloat)(float)absoluteY;
        int screenHeight = GetScreenHeight();
        int screenWidth = GetScreenWidth();
        int safeAreaHeight = GetSafeAreaHeight("Left");
        int safeAreaHeight2 = GetSafeAreaHeight("Right");
        int safeAreaHeight3 = GetSafeAreaHeight("Top");
        int safeAreaHeight4 = GetSafeAreaHeight("Bottom");
        if (position == PopupRelativePosition.AlignToLeftOf || position == PopupRelativePosition.AlignTopLeft || position == PopupRelativePosition.AlignBottomLeft)
        {
            if (popupView.Popup.IsRTL)
            {
                relativeX = (((double)array[0] - 2.0 * absoluteX - popupViewWidth - (double)safeAreaHeight > 0.0) ? ((double)array[0] - 2.0 * absoluteX - popupViewWidth) : ((double)safeAreaHeight));
            }
            else
            {
                relativeX = (((double)array[0] - popupViewWidth - (double)safeAreaHeight > 0.0) ? ((double)array[0] - popupViewWidth) : ((double)safeAreaHeight));
            }
        }
        else if (position == PopupRelativePosition.AlignToRightOf || position == PopupRelativePosition.AlignTopRight || position == PopupRelativePosition.AlignBottomRight)
        {
            if (popupView.Popup.IsRTL)
            {
                relativeX = (((double)array[0] - 2.0 * absoluteX + (double)width - popupViewWidth - (double)safeAreaHeight > 0.0) ? ((double)array[0] - 2.0 * absoluteX + (double)width - popupViewWidth) : ((double)safeAreaHeight));
            }
            else
            {
                relativeX = (((double)(array[0] + width) + popupViewWidth < (double)(screenWidth - safeAreaHeight2)) ? ((double)(array[0] + width)) : ((double)(screenWidth - safeAreaHeight2) - popupViewWidth));
            }
        }
        else if (popupView.Popup.IsRTL)
        {
            relativeX = (((double)array[0] - 2.0 * absoluteX + (double)width - popupViewWidth - (double)safeAreaHeight > 0.0) ? ((double)array[0] - 2.0 * absoluteX + (double)width - popupViewWidth) : ((double)safeAreaHeight));
        }
        else
        {
            relativeX = (((double)array[0] + popupViewWidth < (double)(screenWidth - safeAreaHeight2)) ? ((double)array[0]) : ((double)(screenWidth - safeAreaHeight2) - popupViewWidth));
        }

        if (position == PopupRelativePosition.AlignTop || position == PopupRelativePosition.AlignTopLeft || position == PopupRelativePosition.AlignTopRight)
        {
            relativeY = (((double)array[1] - popupViewHeight - (double)(GetStatusBarHeight() + safeAreaHeight3) > 0.0) ? ((double)array[1] - popupViewHeight) : ((double)(GetStatusBarHeight() + safeAreaHeight3)));
        }
        else if (position == PopupRelativePosition.AlignBottom || position == PopupRelativePosition.AlignBottomLeft || position == PopupRelativePosition.AlignBottomRight)
        {
            relativeY = (((double)(array[1] + height) + popupViewHeight < (double)(screenHeight - safeAreaHeight4)) ? ((double)(array[1] + height)) : ((double)(screenHeight - safeAreaHeight4) - popupViewHeight));
        }
        else
        {
            relativeY = (((double)array[1] + popupViewHeight < (double)(screenHeight - safeAreaHeight4)) ? ((double)array[1]) : ((double)(screenHeight - safeAreaHeight4) - popupViewHeight));
        }
    }

    //
    // Summary:
    //     Checks for navigation bar.
    //
    // Returns:
    //     Returns true if navigation bar present. Else returns false.
    private static UIView? CheckNavigationBar()
    {
        Microsoft.Maui.Controls.Page page = GetMainPage(shouldReturnOnlyMainPage: true);
        if (page is Microsoft.Maui.Controls.NavigationPage navigationPage)
        {
            page = navigationPage;
        }
        else if (page is Microsoft.Maui.Controls.TabbedPage tabbedPage && tabbedPage.CurrentPage != null && tabbedPage.CurrentPage is Microsoft.Maui.Controls.NavigationPage navigationPage2)
        {
            page = navigationPage2;
        }
        else if (page is Microsoft.Maui.Controls.FlyoutPage flyoutPage && flyoutPage.Detail != null && flyoutPage.Detail is Microsoft.Maui.Controls.NavigationPage navigationPage3)
        {
            page = navigationPage3;
        }

        if (page is Microsoft.Maui.Controls.NavigationPage)
        {
            UIView uIView = (UIView)(page.Handler?.PlatformView);
            if (uIView != null && (uIView.Subviews.Any((UIView x) => !string.IsNullOrEmpty(x.Class.Name) && x.Class.Name.Equals("UINavigationTransitionView")) || uIView.Subviews.Any((UIView x) => !string.IsNullOrEmpty(x.Class.Name) && x.Class.Name.Equals("Microsoft_Maui_Controls_Handlers_Compatibility_NavigationRenderer_MauiControlsNavigationBar"))))
            {
                return uIView;
            }

            page = null;
            return uIView;
        }

        return null;
    }

    //
    // Summary:
    //     Gets the navigation bar.
    //
    // Returns:
    //     Returns the navigation bar.
    private static UIView? GetNavigationBar()
    {
        UIView uIView = CheckNavigationBar();
        if (uIView != null)
        {
            return uIView?.Subviews?.FirstOrDefault((UIView x) => !string.IsNullOrEmpty(x.Class.Name) && x.Class.Name.Equals("Microsoft_Maui_Controls_Handlers_Compatibility_NavigationRenderer_MauiControlsNavigationBar"));
        }

        return uIView;
    }

    //
    // Summary:
    //     Gets the UI blur effect style to determine the blur intensity.
    //
    // Parameters:
    //   popup:
    //     The instance of the CPopup.
    //
    // Returns:
    //     Return UI blur style value based on the defined blur intensity value.
    private static UIBlurEffectStyle GetUIBlurStyle(this CPopup popup)
    {
        if (popup.PopupStyle.BlurIntensity == PopupBlurIntensity.Light)
        {
            return UIBlurEffectStyle.Light;
        }

        if (popup.PopupStyle.BlurIntensity == PopupBlurIntensity.ExtraLight)
        {
            return UIBlurEffectStyle.ExtraLight;
        }

        if (popup.PopupStyle.BlurIntensity == PopupBlurIntensity.Custom)
        {
            return UIBlurEffectStyle.Light;
        }

        return UIBlurEffectStyle.Dark;
    }
}
