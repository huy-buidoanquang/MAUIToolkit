using MAUIToolkit.Core.Internals.Platforms;

namespace MAUIToolkit.Core.Controls.Popup;

//
// Summary:
//     The view that holds the PopupView of the MAUIToolkit.Core.Controls.Popup.CPopup.
internal class PopupOverlayContainer : WindowOverlayContainer
{
    private CPopup popup;

    //
    // Summary:
    //     Initializes a new instance of the MAUIToolkit.Core.Controls.Popup.PopupOverlayContainer
    //     class.
    //
    // Parameters:
    //   sfpopup:
    //     The instance of the CPopup.
    internal PopupOverlayContainer(CPopup cPopup)
    {
        popup = cPopup;
    }

    //
    // Summary:
    //     This method will be invoked when the touch interaction is made on the container.
    //
    //
    // Parameters:
    //   x:
    //     Value of X position.
    //
    //   y:
    //     Value of Y position.
    internal override void ProcessTouchInteraction(float x, float y)
    {
        Point touchPoint = new Point(x, y);
        ClosePopupIfRequired(touchPoint);
    }

    //
    // Summary:
    //     Used to set background color for the container.
    //
    // Parameters:
    //   overlayColor:
    //     The color to set as the background color for the PopupOverlayContainer.
    internal void ApplyBackgroundColor(Brush overlayColor)
    {
        base.Background = overlayColor;
    }

    //
    // Summary:
    //     Used to check whether the touch is made outside popup or not.
    //
    // Parameters:
    //   touchPoint:
    //     The touch point of the container view.
    private void ClosePopupIfRequired(Point touchPoint)
    {
        if (!popup.IsPopupAnimationInProgress && !popup.IsContainerAnimationInProgress && popup.IsOpen && popup.CanLayoutForGivenSizeAndPosition() && (!(touchPoint.Y > (double)popup.PopupView.GetY()) || !(touchPoint.Y < (double)popup.PopupView.GetY() + popup.PopupViewHeight) || !(touchPoint.X > (double)popup.PopupView.GetX()) || !(touchPoint.X < (double)popup.PopupView.GetX() + popup.PopupViewWidth)))
        {
            popup.ClosePopupIfRequired();
        }
    }
}
