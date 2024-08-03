using CoreGraphics;
using Foundation;
using UIKit;

namespace MAUIToolkit.Core.Internals.Platforms;

internal class WindowOverlayStack : UIView
{
    internal bool canHandleTouch = false;

    private WindowOverlayContainer? virtualView;

    public override UIView HitTest(CGPoint point, UIEvent? uievent)
    {
        UIView uIView = base.HitTest(point, uievent);
        return (!canHandleTouch && uIView == this) ? null : uIView;
    }

    internal void Connect(WindowOverlayContainer mauiView)
    {
        virtualView = mauiView;
    }

    internal void DisConnect()
    {
        virtualView = null;
    }

    public override void TouchesBegan(NSSet touches, UIEvent? evt)
    {
        base.TouchesBegan(touches, evt);
        if (touches.AnyObject is UITouch uITouch)
        {
            CGPoint cGPoint = uITouch.LocationInView(this);
            virtualView?.ProcessTouchInteraction((float)cGPoint.X, (float)cGPoint.Y);
        }
    }
}

