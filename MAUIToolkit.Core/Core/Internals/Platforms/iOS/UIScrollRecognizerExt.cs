using CoreGraphics;
using UIKit;

namespace MAUIToolkit.Core.Internals.Platforms;

internal class UIScrollRecognizerExt : UIPanGestureRecognizer
{
    private TouchDetector touchDetector;

    private ITouchListener? touchListener;

    internal UIScrollRecognizerExt(TouchDetector listener)
    {
        touchDetector = listener;
        if (touchDetector.MauiView is ITouchListener touchListener)
        {
            this.touchListener = touchListener;
        }

        AddTarget((Action)delegate
        {
            OnScroll(this);
        });
        if (UpdateAllowedScrollTypesMask())
        {
            AllowedScrollTypesMask = UIScrollTypeMask.All;
        }

        base.ShouldRecognizeSimultaneously = (UIGesturesProbe)System.Delegate.Combine(base.ShouldRecognizeSimultaneously, new UIGesturesProbe(GestureRecognizer));
        base.ShouldReceiveTouch = (UITouchEventArgs)System.Delegate.Combine(base.ShouldReceiveTouch, new UITouchEventArgs(GesturerTouchRecognizer));
    }

    private bool UpdateAllowedScrollTypesMask()
    {
        if (DeviceInfo.Platform == DevicePlatform.iOS)
        {
            Version version = new Version(UIDevice.CurrentDevice.SystemVersion);
            if (version >= new Version(13, 4))
            {
                return true;
            }
        }
        else if (!(DeviceInfo.Platform == DevicePlatform.MacCatalyst))
        {
        }

        return false;
    }

    private bool GesturerTouchRecognizer(UIGestureRecognizer recognizer, UITouch touch)
    {
        return false;
    }

    private void OnScroll(UIScrollRecognizerExt touchRecognizerExt)
    {
        if (touchDetector.MauiView.IsEnabled && !touchDetector.MauiView.InputTransparent)
        {
            long pointerId = ((IntPtr)touchRecognizerExt.Handle.Handle).ToInt64();
            CGPoint cGPoint = touchRecognizerExt.TranslationInView(View);
            CGPoint cGPoint2 = touchRecognizerExt.LocationInView(View);
            touchDetector.OnScrollAction(pointerId, new Point(cGPoint2.X, cGPoint2.Y), (cGPoint.Y != 0) ? cGPoint.Y : cGPoint.X);
        }
    }

    private bool GestureRecognizer(UIGestureRecognizer gestureRecognizer, UIGestureRecognizer otherGestureRecognizer)
    {
        if (otherGestureRecognizer is GestureDetector.UIPanGestureExt || otherGestureRecognizer is UITouchRecognizerExt || touchListener == null)
        {
            return true;
        }

        return !touchListener.IsTouchHandled;
    }
}
