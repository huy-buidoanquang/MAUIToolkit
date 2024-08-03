namespace MAUIToolkit.Core.Controls.Popup;

//
// Summary:
//     Built-in animations available in MAUIToolkit.Core.Controls.Popup, which is applied
//     when the PopupView opens and closes in the screen.
public enum PopupAnimationMode
{
    //
    // Summary:
    //     zoom-out animation will be applied when the PopupView opens and zoom-in animation
    //     will be applied when the PopupView closes.
    Zoom,
    //
    // Summary:
    //     Fade-out animation will be applied when the PopupView opens and Fade-in animation
    //     will be applied when the PopupView closes.
    Fade,
    //
    // Summary:
    //     PopupView will be animated from left-to-right, when it opens and it will be animated
    //     from right-to-left when the PopupView closes.
    SlideOnLeft,
    //
    // Summary:
    //     PopupView will be animated from right-to-left, when it opens and it will be animated
    //     from left-to-right when the PopupView closes.
    SlideOnRight,
    //
    // Summary:
    //     PopupView will be animated from top-to-bottom, when it opens and it will be animated
    //     from bottom-to-top when the PopupView closes.
    SlideOnTop,
    //
    // Summary:
    //     PopupView will be animated from bottom-to-top, when it opens and it will be animated
    //     from top-to-bottom when the PopupView closes.
    SlideOnBottom,
    //
    // Summary:
    //     Animation will not be applied.
    None
}
