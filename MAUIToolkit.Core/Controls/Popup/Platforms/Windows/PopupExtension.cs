using MAUIToolkit.Core.Internals.Platforms;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.UI.Xaml.Hosting;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Composition;
using System.Numerics;
using Point = Windows.Foundation.Point;
using Microsoft.Maui.Platform;

namespace MAUIToolkit.Core.Controls.Popup.Platforms.Windows;

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

            if (application.MainPage is NavigationPage navigationPage && !shouldReturnOnlyMainPage)
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
            FrameworkElement frameworkElement = Internals.Platforms.ElementExtensions.ToPlatform(view, mauiContext);
            if (frameworkElement != null)
            {
                return (int)frameworkElement.ActualOffset.X;
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
            FrameworkElement frameworkElement = Internals.Platforms.ElementExtensions.ToPlatform(view, mauiContext);
            if (frameworkElement != null)
            {
                return (int)frameworkElement.ActualOffset.Y;
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
        return 30;
    }

    //
    // Summary:
    //     Gets the height of the device.
    //
    // Returns:
    //     The height of the device.
    internal static int GetScreenHeight()
    {
        Panel platformRootView = WindowOverlayHelper.PlatformRootView;
        return (platformRootView != null) ? ((int)platformRootView.DesiredSize.Height) : 0;
    }

    //
    // Summary:
    //     Gets the width of the device.
    //
    // Returns:
    //     The width of the device.
    internal static int GetScreenWidth()
    {
        Panel platformRootView = WindowOverlayHelper.PlatformRootView;
        return (platformRootView != null) ? ((int)platformRootView.DesiredSize.Width) : 0;
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
    internal static void Blur(View view, CPopup popup, bool isopen)
    {
        if (!isopen)
        {
            return;
        }

        IReadOnlyList<Microsoft.UI.Xaml.Controls.Primitives.Popup> openPopupsForXamlRoot = VisualTreeHelper.GetOpenPopupsForXamlRoot(WindowOverlayHelper.PlatformRootView.XamlRoot);
        UIElement uIElement = null;
        if (openPopupsForXamlRoot != null)
        {
            uIElement = ((openPopupsForXamlRoot.Count != 1) ? openPopupsForXamlRoot[1].Child : WindowOverlayHelper.PlatformRootView?.Children.LastOrDefault((UIElement x) => x is WindowRootView));
            if (!(uIElement == null))
            {
                Compositor compositor = ElementCompositionPreview.GetElementVisual(uIElement).Compositor;
                SpriteVisual spriteVisual = compositor?.CreateSpriteVisual();
                GaussianBlurEffect graphicsEffect = new GaussianBlurEffect
                {
                    Name = "Blur",
                    BlurAmount = popup.GetBlurRadius(),
                    BorderMode = EffectBorderMode.Soft,
                    Source = new CompositionEffectSourceParameter("Background")
                };
                CompositionEffectBrush compositionEffectBrush = compositor?.CreateEffectFactory(graphicsEffect).CreateBrush();
                compositionEffectBrush?.SetSourceParameter("Background", compositor?.CreateBackdropBrush());
                spriteVisual.Brush = compositionEffectBrush;
                spriteVisual.Size = new Vector2(GetScreenWidth(), GetScreenHeight());
                ElementCompositionPreview.SetElementChildVisual(uIElement, spriteVisual);
                popup.BlurElement = uIElement;
                compositor = null;
                compositionEffectBrush = null;
                spriteVisual = null;
                graphicsEffect = null;
                openPopupsForXamlRoot = null;
            }
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
        if (popup.BlurElement != null)
        {
            ElementCompositionPreview.GetElementChildVisual(popup.BlurElement)?.Dispose();
            Visual visual = null;
            ElementCompositionPreview.SetElementChildVisual(popup.BlurElement, null);
            popup.BlurElement = null;
        }
    }

    //
    // Summary:
    //     Updates effect visual size when resize the window.
    //
    // Parameters:
    //   popup:
    //     The instance of the CPopup.
    internal static void UpdateEffectVisualSize(CPopup popup)
    {
        if (popup.BlurElement != null)
        {
            Visual elementChildVisual = ElementCompositionPreview.GetElementChildVisual(popup.BlurElement);
            if (elementChildVisual != null)
            {
                elementChildVisual.Size = new Vector2(GetScreenWidth(), GetScreenHeight());
                elementChildVisual = null;
            }
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
        bool flag = false;
        if ((((IVisualElementController)relative).EffectiveFlowDirection & EffectiveFlowDirection.RightToLeft) == EffectiveFlowDirection.RightToLeft)
        {
            flag = true;
        }

        Panel platformRootView = WindowOverlayHelper.PlatformRootView;
        FrameworkElement frameworkElement = Internals.Platforms.ElementExtensions.ToPlatform(relative, relative.Handler.MauiContext);
        double popupViewWidth = popupView.Popup.PopupViewWidth;
        double popupViewHeight = popupView.Popup.PopupViewHeight;
        float y = frameworkElement.ActualSize.Y;
        float x = frameworkElement.ActualSize.X;
        double[] array = new double[2];
        GeneralTransform generalTransform = frameworkElement.TransformToVisual(platformRootView);
        array[0] = generalTransform.TransformPoint(new Point(0f, 0f)).X;
        array[1] = generalTransform.TransformPoint(new Point(0f, 0f)).Y;
        array[0] += absoluteX;
        array[1] += absoluteY;
        int screenHeight = GetScreenHeight();
        int screenWidth = GetScreenWidth();
        if (position == PopupRelativePosition.AlignToLeftOf || position == PopupRelativePosition.AlignTopLeft || position == PopupRelativePosition.AlignBottomLeft)
        {
            if (popupView.Popup.IsRTL)
            {
                if (flag)
                {
                    relativeX = Math.Max(array[0] - 2.0 * absoluteX - (double)x - popupViewWidth, 0.0);
                }
                else
                {
                    relativeX = Math.Max(array[0] - 2.0 * absoluteX - popupViewWidth, 0.0);
                }
            }
            else
            {
                relativeX = ((array[0] - popupViewWidth > 0.0) ? (array[0] - popupViewWidth) : 0.0);
            }
        }
        else if (position == PopupRelativePosition.AlignToRightOf || position == PopupRelativePosition.AlignTopRight || position == PopupRelativePosition.AlignBottomRight)
        {
            if (popupView.Popup.IsRTL)
            {
                if (flag)
                {
                    relativeX = Math.Max(array[0] - 2.0 * absoluteX - popupViewWidth, 0.0);
                }
                else
                {
                    relativeX = Math.Max(array[0] - 2.0 * absoluteX + (double)x - popupViewWidth, 0.0);
                }
            }
            else
            {
                relativeX = ((array[0] + (double)x + popupViewWidth < (double)screenWidth) ? (array[0] + (double)x) : ((double)screenWidth - popupViewWidth));
            }
        }
        else if (popupView.Popup.IsRTL)
        {
            if (flag)
            {
                relativeX = Math.Max(array[0] - 2.0 * absoluteX - popupViewWidth, 0.0);
            }
            else
            {
                relativeX = Math.Max(array[0] - 2.0 * absoluteX + (double)x - popupViewWidth, 0.0);
            }
        }
        else
        {
            relativeX = ((array[0] + popupViewWidth < (double)screenWidth) ? array[0] : ((double)screenWidth - popupViewWidth));
        }

        if (position == PopupRelativePosition.AlignTop || position == PopupRelativePosition.AlignTopLeft || position == PopupRelativePosition.AlignTopRight)
        {
            relativeY = Math.Max(GetStatusBarHeight() + GetActionBarHeight(), array[1] - popupViewHeight);
        }
        else if (position == PopupRelativePosition.AlignBottom || position == PopupRelativePosition.AlignBottomLeft || position == PopupRelativePosition.AlignBottomRight)
        {
            relativeY = ((array[1] + (double)y + popupViewHeight < (double)screenHeight) ? (array[1] + (double)y) : ((double)screenHeight - popupViewHeight));
        }
        else
        {
            relativeY = ((array[1] + popupViewHeight < (double)screenHeight) ? array[1] : ((double)screenHeight - popupViewHeight));
        }
    }

    //
    // Summary:
    //     Gets the radius to determine blur intensity.
    //
    // Parameters:
    //   popup:
    //     The instance of the CPopup.
    //
    // Returns:
    //     Return radius value based on the defined blur intensity value.
    private static float GetBlurRadius(this CPopup popup)
    {
        if (popup.PopupStyle.BlurIntensity == PopupBlurIntensity.Light)
        {
            return 5f;
        }

        if (popup.PopupStyle.BlurIntensity == PopupBlurIntensity.ExtraDark)
        {
            return 10f;
        }

        if (popup.PopupStyle.BlurIntensity == PopupBlurIntensity.ExtraLight)
        {
            return 2.5f;
        }

        if (popup.PopupStyle.BlurIntensity == PopupBlurIntensity.Custom)
        {
            return popup.PopupStyle.BlurRadius;
        }

        return 7.5f;
    }
}
