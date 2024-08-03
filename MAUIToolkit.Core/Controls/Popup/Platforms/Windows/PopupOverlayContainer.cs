using MAUIToolkit.Core.Controls.Popup.Platforms.Windows;
using MAUIToolkit.Core.Internals;
using MAUIToolkit.Core.Internals.Platforms;

namespace MAUIToolkit.Core.Controls.Popup
{
    //
    // Summary:
    //     The view that holds the PopupView of the MAUIToolkit.Core.Controls.CPopup.
    internal class PopupOverlayContainer : WindowOverlayContainer, ITouchListener
    {
        private CPopup popup;

        //
        // Summary:
        //     Initializes a new instance of the MAUIToolkit.Core.Controls.PopupOverlayContainer
        //     class.
        //
        // Parameters:
        //   sfpopup:
        //     The instance of the CPopup.
        internal PopupOverlayContainer(CPopup popup)
        {
            this.popup = popup;
            this.AddTouchListener(this);
        }

        //
        // Summary:
        //     This method will be called based on the view touch.
        //
        // Parameters:
        //   e:
        //     The PanEventArgs object.
        public void OnTouch(Internals.PointerEventArgs e)
        {
            if (e.Action == PointerActions.Pressed)
            {
                ClosePopupIfRequired(e.TouchPoint);
            }
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
}
