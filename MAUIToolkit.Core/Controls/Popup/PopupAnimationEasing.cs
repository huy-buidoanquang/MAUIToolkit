namespace MAUIToolkit.Core.Controls.Popup;

//
// Summary:
//     Defines constants that specifies the built-in animation easing effects available
//     in MAUIToolkit.Core.Controls.Popup.SfPopup when opening and closing the popup.
public enum PopupAnimationEasing
{
    //
    // Summary:
    //     This easing function will smoothly accelerate the animation to its final value.
    SinIn,
    //
    // Summary:
    //     This easing function will smoothly decelerate the animation to its final value.
    SinOut,
    //
    // Summary:
    //     This easing function will smoothly accelerate the animation at the beginning
    //     and then smoothly decelerates the animation at the end.
    SinInOut,
    //
    // Summary:
    //     This easing function will use a constant velocity to animate the view and is
    //     the default type.
    Linear
}
