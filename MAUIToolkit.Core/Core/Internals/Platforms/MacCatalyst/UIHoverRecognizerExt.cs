using CoreGraphics;
using UIKit;

namespace MAUIToolkit.Core.Internals.Platforms;

internal class UIHoverRecognizerExt : UIHoverGestureRecognizer
{
    private TouchDetector touchDetector;

    private ITouchListener? touchListener;

    public UIHoverRecognizerExt(TouchDetector listener)
        : base(Hovering)
    {
        touchDetector = listener;
        if (touchDetector.MauiView is ITouchListener touchListener)
        {
            this.touchListener = touchListener;
        }

        base.ShouldRecognizeSimultaneously = (UIGesturesProbe)System.Delegate.Combine(base.ShouldRecognizeSimultaneously, new UIGesturesProbe(GestureRecognizer));
        AddTarget((Action)delegate
        {
            OnHover(touchDetector);
        });
    }

    //
    // Summary:
    //     Having static member for base action hence UIKit.UIHoverGestureRecognizer does
    //     not have default consturctor.
    private static void Hovering()
    {
    }

    private bool GestureRecognizer(UIGestureRecognizer gestureRecognizer, UIGestureRecognizer otherGestureRecognizer)
    {
        if (otherGestureRecognizer is UITouchRecognizerExt || touchListener == null)
        {
            return true;
        }

        return !touchListener.IsTouchHandled;
    }

    private void OnHover(TouchDetector gestureDetecture)
    {
        if (touchDetector.MauiView.IsEnabled && !touchDetector.MauiView.InputTransparent)
        {
            PointerActions action = ((State == UIGestureRecognizerState.Began) ? PointerActions.Entered : ((State == UIGestureRecognizerState.Changed) ? PointerActions.Moved : ((State == UIGestureRecognizerState.Ended) ? PointerActions.Exited : PointerActions.Cancelled)));
            long pointerId = ((IntPtr)base.Handle.Handle).ToInt64();
            CGPoint cGPoint = LocationInView(View);
            gestureDetecture.OnTouchAction((IElement? relativeTo) => TouchDetector.CalculatePosition(relativeTo, touchDetector.MauiView, this), pointerId, action, new Point(cGPoint.X, cGPoint.Y));
        }
    }
}
