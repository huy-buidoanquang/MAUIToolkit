namespace MAUIToolkit.Core.Primitives;

public enum DrawingOrder
{
    //
    // Summary:
    //     Draws over the content.
    AboveContent,
    //
    // Summary:
    //     Draws over the content with Drawable view having the touch. This is applicable
    //     only for WinUI platform.
    AboveContentWithTouch,
    //
    // Summary:
    //     Draws below the content.
    BelowContent,
    //
    // Summary:
    //     Disables the drawing.
    NoDraw
}
