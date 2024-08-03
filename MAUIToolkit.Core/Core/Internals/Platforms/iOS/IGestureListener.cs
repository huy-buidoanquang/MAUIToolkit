namespace MAUIToolkit.Core.Internals.Platforms;

//
// Summary:
//     Enable scroll and scale listener.
public interface IGestureListener
{
    //
    // Summary:
    //     Gets the boolean value indicating to pass the touches on either the parent or
    //     child view.
    //
    // Remarks:
    //     Only MAUIToolkit.Core.Internals.IGestureListener inheriting maui Microsoft.Maui.Controls.View
    //     can use this property.
    bool IsTouchHandled => false;

    //
    // Summary:
    //     Gets the value indicating whether the single-tap gesture recognizer should be
    //     restricted when a double-tap gesture is triggered.
    //
    // Remarks:
    //     It is used for internal purposes by the.NET MAUI SfCalendar control, which does
    //     not change dynamically.
    internal bool IsRequiredSingleTapGestureRecognizerToFail => true;
}
