using CoreGraphics;
using Foundation;
using UIKit;

namespace MAUIToolkit.Core.Internals.Platforms;

internal class UITouchRecognizerExt : UIPanGestureRecognizer
{
    private TouchDetector touchDetector;

    private ITouchListener? touchListener;

    internal UITouchRecognizerExt(TouchDetector listener)
    {
        touchDetector = listener;
        if (touchDetector.MauiView is ITouchListener touchListener)
        {
            this.touchListener = touchListener;
        }

        base.ShouldRecognizeSimultaneously = (UIGesturesProbe)System.Delegate.Combine(base.ShouldRecognizeSimultaneously, new UIGesturesProbe(GestureRecognizer));
    }

    private bool GestureRecognizer(UIGestureRecognizer gestureRecognizer, UIGestureRecognizer otherGestureRecognizer)
    {
        if (otherGestureRecognizer is GestureDetector.UIPanGestureExt || otherGestureRecognizer is UIScrollRecognizerExt || touchListener == null)
        {
            return true;
        }

        return !touchListener.IsTouchHandled;
    }

    public override void TouchesBegan(NSSet touches, UIEvent evt)
    {
        base.TouchesBegan(touches, evt);
        if (touchDetector.MauiView.IsEnabled && !touchDetector.MauiView.InputTransparent && touches.AnyObject is UITouch uITouch)
        {
            PointerDeviceType deviceType = ((uITouch.Type == UITouchType.Stylus) ? PointerDeviceType.Stylus : PointerDeviceType.Touch);
            long id = ((IntPtr)uITouch.Handle.Handle).ToInt64();
            CGPoint cGPoint = uITouch.LocationInView(View);
            touchDetector.OnTouchAction(new PointerEventArgs((IElement? relativeTo) => TouchDetector.CalculatePosition(relativeTo, touchDetector.MauiView, this), id, PointerActions.Pressed, deviceType, new Point(cGPoint.X, cGPoint.Y))
            {
                IsLeftButtonPressed = (uITouch.TapCount == 1)
            });
        }
    }

    public override void TouchesMoved(NSSet touches, UIEvent evt)
    {
        base.TouchesMoved(touches, evt);
        if (touchDetector.MauiView.IsEnabled && !touchDetector.MauiView.InputTransparent && touches.AnyObject is UITouch uITouch)
        {
            PointerDeviceType deviceType = ((uITouch.Type == UITouchType.Stylus) ? PointerDeviceType.Stylus : PointerDeviceType.Touch);
            long id = ((IntPtr)uITouch.Handle.Handle).ToInt64();
            CGPoint cGPoint = uITouch.LocationInView(View);
            touchDetector.OnTouchAction(new PointerEventArgs((IElement? relativeTo) => TouchDetector.CalculatePosition(relativeTo, touchDetector.MauiView, this), id, PointerActions.Moved, deviceType, new Point(cGPoint.X, cGPoint.Y))
            {
                IsLeftButtonPressed = (uITouch.TapCount == 1)
            });
        }
    }

    public override void TouchesEnded(NSSet touches, UIEvent evt)
    {
        base.TouchesEnded(touches, evt);
        if (touchDetector.MauiView.IsEnabled && !touchDetector.MauiView.InputTransparent && touches.AnyObject is UITouch uITouch)
        {
            PointerDeviceType deviceType = ((uITouch.Type == UITouchType.Stylus) ? PointerDeviceType.Stylus : PointerDeviceType.Touch);
            long pointerId = ((IntPtr)uITouch.Handle.Handle).ToInt64();
            CGPoint cGPoint = uITouch.LocationInView(View);
            touchDetector.OnTouchAction((IElement? relativeTo) => TouchDetector.CalculatePosition(relativeTo, touchDetector.MauiView, this), pointerId, PointerActions.Released, deviceType, new Point(cGPoint.X, cGPoint.Y));
        }
    }

    public override void TouchesCancelled(NSSet touches, UIEvent evt)
    {
        base.TouchesCancelled(touches, evt);
        if (touchDetector.MauiView.IsEnabled && !touchDetector.MauiView.InputTransparent && touches.AnyObject is UITouch uITouch)
        {
            PointerDeviceType deviceType = ((uITouch.Type == UITouchType.Stylus) ? PointerDeviceType.Stylus : PointerDeviceType.Touch);
            long pointerId = ((IntPtr)uITouch.Handle.Handle).ToInt64();
            CGPoint cGPoint = uITouch.LocationInView(View);
            touchDetector.OnTouchAction((IElement? relativeTo) => TouchDetector.CalculatePosition(relativeTo, touchDetector.MauiView, this), pointerId, PointerActions.Cancelled, deviceType, new Point(cGPoint.X, cGPoint.Y));
        }
    }
}
