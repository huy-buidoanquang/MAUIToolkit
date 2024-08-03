using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;

namespace MAUIToolkit.Core.Semantics;

//
// Summary:
//     Holds the methods that used for accessibility implementation.
internal static class AccessibilityHelper
{
    //
    // Summary:
    //     Return the view start position form screen.
    //
    // Parameters:
    //   element:
    //     Element or layout that needed to calculate the start position.
    //
    //   scale:
    //     Scale value of the screen.
    internal static Microsoft.Maui.Graphics.Point GetViewStartPosition(UIElement element, double scale)
    {
        GeneralTransform generalTransform = element.TransformToVisual(null);
        Windows.Foundation.Point point = generalTransform.TransformPoint(new Windows.Foundation.Point(0f, 0f));
        return new Microsoft.Maui.Graphics.Point(point.X * scale, point.Y * scale);
    }
}
