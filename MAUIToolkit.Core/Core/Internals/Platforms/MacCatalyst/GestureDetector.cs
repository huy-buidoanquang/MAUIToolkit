using CoreGraphics;
using UIKit;

namespace MAUIToolkit.Core.Internals.Platforms;

//
// Summary:
//     Enables MAUI view to recognize scale and scroll interaction.
public class GestureDetector : IDisposable
{
    internal class UIPanGestureExt : UIPanGestureRecognizer
    {
        private IGestureListener? gestureListener;

        public UIPanGestureExt(GestureDetector gestureDetector)
        {
            GestureDetector gestureDetector2 = gestureDetector;
            //base._002Ector();
            UIPanGestureExt uIPanGestureExt = this;
            if (gestureDetector2.MauiView is IGestureListener gestureListener)
            {
                this.gestureListener = gestureListener;
            }

            AddTarget((Action)delegate
            {
                uIPanGestureExt.OnScroll(gestureDetector2);
            });
            base.ShouldRecognizeSimultaneously = (UIGesturesProbe)System.Delegate.Combine(base.ShouldRecognizeSimultaneously, new UIGesturesProbe(GestureRecognizer));
        }

        private bool GestureRecognizer(UIGestureRecognizer gestureRecognizer, UIGestureRecognizer otherGestureRecognizer)
        {
            if (otherGestureRecognizer is UITouchRecognizerExt || otherGestureRecognizer is UIScrollRecognizerExt || gestureListener == null)
            {
                return true;
            }

            return !gestureListener.IsTouchHandled;
        }

        private void OnScroll(GestureDetector gestureDetector)
        {
            GestureDetector gestureDetector2 = gestureDetector;
            if (!gestureDetector2.MauiView.IsEnabled || gestureDetector2.MauiView.InputTransparent)
            {
                return;
            }

            CGPoint cGPoint = LocationInView(View);
            CGPoint cGPoint2 = TranslationInView(View);
            GestureStatus gestureStatus = GestureStatus.Completed;
            UIGestureRecognizerState state = State;
            UIGestureRecognizerState uIGestureRecognizerState = state;
            UIGestureRecognizerState num = uIGestureRecognizerState - 1;
            if ((ulong)num <= 4uL)
            {
                switch (num)
                {
                    case UIGestureRecognizerState.Possible:
                        gestureStatus = GestureStatus.Started;
                        break;
                    case UIGestureRecognizerState.Began:
                        gestureStatus = GestureStatus.Running;
                        break;
                    case UIGestureRecognizerState.Ended:
                    case UIGestureRecognizerState.Cancelled:
                        gestureStatus = GestureStatus.Canceled;
                        break;
                    case UIGestureRecognizerState.Changed:
                        gestureStatus = GestureStatus.Completed;
                        break;
                }
            }

            Point velocity = Point.Zero;
            if (gestureStatus == GestureStatus.Completed || gestureStatus == GestureStatus.Canceled)
            {
                CGPoint cGPoint3 = VelocityInView(View);
                velocity = new Point(cGPoint3.X, cGPoint3.Y);
            }

            gestureDetector2.OnScroll((IElement? relativeTo) => TouchDetector.CalculatePosition(relativeTo, gestureDetector2.MauiView, this), gestureStatus, new Point(cGPoint.X, cGPoint.Y), new Point(cGPoint2.X, cGPoint2.Y), velocity);
            SetTranslation(CGPoint.Empty, View);
        }
    }

    private class UIPinchGestureExt : UIPinchGestureRecognizer
    {
        private IGestureListener? gestureListener;

        public UIPinchGestureExt(GestureDetector gestureDetector)
        {
            GestureDetector gestureDetector2 = gestureDetector;
            //base._002Ector();
            UIPinchGestureExt uIPinchGestureExt = this;
            if (gestureDetector2.MauiView is IGestureListener gestureListener)
            {
                this.gestureListener = gestureListener;
            }

            AddTarget((Action)delegate
            {
                uIPinchGestureExt.OnPinch(gestureDetector2);
            });
            base.ShouldRecognizeSimultaneously = (UIGestureRecognizer g, UIGestureRecognizer o) => uIPinchGestureExt.gestureListener == null || !uIPinchGestureExt.gestureListener.IsTouchHandled;
        }

        private void OnPinch(GestureDetector gestureDetector)
        {
            GestureDetector gestureDetector2 = gestureDetector;
            if (!gestureDetector2.MauiView.IsEnabled || gestureDetector2.MauiView.InputTransparent)
            {
                return;
            }

            CGPoint cGPoint = LocationInView(View);
            GestureStatus state = GestureStatus.Completed;
            double pinchAngle = double.NaN;
            if (NumberOfTouches == 2)
            {
                CGPoint cGPoint2 = LocationOfTouch(0, View);
                CGPoint cGPoint3 = LocationOfTouch(1, View);
                pinchAngle = MathUtils.GetAngle((float)cGPoint2.X, (float)cGPoint3.X, (float)cGPoint2.Y, (float)cGPoint3.Y);
            }

            UIGestureRecognizerState state2 = State;
            UIGestureRecognizerState uIGestureRecognizerState = state2;
            UIGestureRecognizerState num = uIGestureRecognizerState - 1;
            if ((ulong)num <= 4uL)
            {
                switch (num)
                {
                    case UIGestureRecognizerState.Possible:
                        state = GestureStatus.Started;
                        break;
                    case UIGestureRecognizerState.Began:
                        state = GestureStatus.Running;
                        break;
                    case UIGestureRecognizerState.Ended:
                    case UIGestureRecognizerState.Cancelled:
                        state = GestureStatus.Canceled;
                        break;
                    case UIGestureRecognizerState.Changed:
                        state = GestureStatus.Completed;
                        break;
                }
            }

            gestureDetector2.OnPinch((IElement? relativeTo) => TouchDetector.CalculatePosition(relativeTo, gestureDetector2.MauiView, this), state, new Point(cGPoint.X, cGPoint.Y), pinchAngle, (float)Scale);
            Scale = 1;
        }
    }

    internal class UITapGestureExt : UITapGestureRecognizer
    {
        private IGestureListener? gestureListener;

        public UITapGestureExt(GestureDetector gestureDetector, nuint tapsCount)
        {
            GestureDetector gestureDetector2 = gestureDetector;
            //base._002Ector();
            UITapGestureExt uITapGestureExt = this;
            NumberOfTapsRequired = tapsCount;
            if (gestureDetector2.MauiView is IGestureListener gestureListener)
            {
                this.gestureListener = gestureListener;
            }

            if (tapsCount == 1 && gestureDetector2.DoubleTapGesture != null && this.gestureListener != null && this.gestureListener.IsRequiredSingleTapGestureRecognizerToFail)
            {
                RequireGestureRecognizerToFail(gestureDetector2.DoubleTapGesture);
            }

            AddTarget((Action)delegate
            {
                uITapGestureExt.OnTap(gestureDetector2);
            });
            base.ShouldRecognizeSimultaneously = (UIGesturesProbe)System.Delegate.Combine(base.ShouldRecognizeSimultaneously, new UIGesturesProbe(GestureRecognizer));
            base.ShouldReceiveTouch = (UITouchEventArgs)System.Delegate.Combine(base.ShouldReceiveTouch, new UITouchEventArgs(HandleTouchGesture));
        }

        private bool HandleTouchGesture(UIGestureRecognizer recognizer, UITouch touch)
        {
            if (gestureListener is ITapGestureListener)
            {
                (gestureListener as ITapGestureListener).ShouldHandleTap(touch.View);
            }

            return true;
        }

        private bool GestureRecognizer(UIGestureRecognizer gestureRecognizer, UIGestureRecognizer otherGestureRecognizer)
        {
            if (otherGestureRecognizer is UILongPressGestureExt)
            {
                return false;
            }

            if (gestureListener != null && !gestureListener.IsRequiredSingleTapGestureRecognizerToFail && object.Equals(gestureRecognizer.View, otherGestureRecognizer.View) && gestureRecognizer is UITapGestureExt uITapGestureExt && otherGestureRecognizer is UITapGestureExt uITapGestureExt2 && uITapGestureExt.NumberOfTapsRequired != uITapGestureExt2.NumberOfTapsRequired)
            {
                return false;
            }

            return gestureListener == null || !gestureListener.IsTouchHandled;
        }

        private void OnTap(GestureDetector gestureDetector)
        {
            if (gestureDetector.MauiView.IsEnabled && !gestureDetector.MauiView.InputTransparent)
            {
                CGPoint cGPoint = LocationInView(View);
                gestureDetector.OnTapped(new Point(cGPoint.X, cGPoint.Y), (int)NumberOfTapsRequired);
            }
        }
    }

    private class UILongPressGestureExt : UILongPressGestureRecognizer
    {
        private IGestureListener? gestureListener;

        public UILongPressGestureExt(GestureDetector gestureDetector)
        {
            GestureDetector gestureDetector2 = gestureDetector;
            //base._002Ector();
            UILongPressGestureExt uILongPressGestureExt = this;
            if (gestureDetector2.MauiView is IGestureListener gestureListener)
            {
                this.gestureListener = gestureListener;
            }

            AddTarget((Action)delegate
            {
                uILongPressGestureExt.OnLongPress(gestureDetector2);
            });
            base.ShouldRecognizeSimultaneously = (UIGesturesProbe)System.Delegate.Combine(base.ShouldRecognizeSimultaneously, new UIGesturesProbe(GestureRecognizer));
        }

        private bool GestureRecognizer(UIGestureRecognizer gestureRecognizer, UIGestureRecognizer otherGestureRecognizer)
        {
            if (otherGestureRecognizer is UITapGestureExt)
            {
                return false;
            }

            return gestureListener == null || !gestureListener.IsTouchHandled;
        }

        private void OnLongPress(GestureDetector gestureDetector)
        {
            GestureDetector gestureDetector2 = gestureDetector;
            if (!gestureDetector2.MauiView.IsEnabled || gestureDetector2.MauiView.InputTransparent)
            {
                return;
            }

            GestureStatus status = GestureStatus.Completed;
            CGPoint cGPoint = LocationInView(View);
            UIGestureRecognizerState state = State;
            UIGestureRecognizerState uIGestureRecognizerState = state;
            UIGestureRecognizerState num = uIGestureRecognizerState - 1;
            if ((ulong)num <= 4uL)
            {
                switch (num)
                {
                    case UIGestureRecognizerState.Possible:
                        status = GestureStatus.Started;
                        break;
                    case UIGestureRecognizerState.Began:
                        status = GestureStatus.Running;
                        break;
                    case UIGestureRecognizerState.Ended:
                    case UIGestureRecognizerState.Cancelled:
                        status = GestureStatus.Canceled;
                        break;
                    case UIGestureRecognizerState.Changed:
                        status = GestureStatus.Completed;
                        break;
                }
            }

            if (State != UIGestureRecognizerState.Possible)
            {
                gestureDetector2.OnLongPress((IElement? relativeTo) => TouchDetector.CalculatePosition(relativeTo, gestureDetector2.MauiView, this), new Point(cGPoint.X, cGPoint.Y), status);
            }
        }
    }

    private List<IRightTapGestureListener>? rightTapGestureListeners;

    private List<ITapGestureListener>? tapGestureListeners;

    private List<IDoubleTapGestureListener>? doubleTapGestureListeners;

    private List<IPinchGestureListener>? pinchGestureListeners;

    private List<IPanGestureListener>? panGestureListeners;

    private List<ILongPressGestureListener>? longPressGestureListeners;

    private bool _disposed;

    private bool isViewListenerAdded;

    internal readonly View MauiView;

    private UIPinchGestureExt? pinchGesture;

    private UIPanGestureExt? panGesture;

    private UITapGestureExt? tapGesture;

    private UILongPressGestureExt? longPressGesture;

    internal UITapGestureExt? DoubleTapGesture { get; set; }

    //
    // Summary:
    //     Invoke on Gesture listener created
    //
    // Parameters:
    //   mauiView:
    //     is type of MAUIToolkit.Core.Internals.IGestureListener
    public GestureDetector(View mauiView)
    {
        MauiView = mauiView;
        if (mauiView.Handler != null)
        {
            SubscribeNativeGestureEvents(mauiView);
            return;
        }

        mauiView.HandlerChanged += MauiView_HandlerChanged;
        mauiView.HandlerChanging += MauiView_HandlerChanging;
    }

    private void MauiView_HandlerChanged(object? sender, EventArgs e)
    {
        if (sender is View view && view.Handler != null)
        {
            SubscribeNativeGestureEvents(view);
        }
    }

    private void MauiView_HandlerChanging(object? sender, HandlerChangingEventArgs e)
    {
        UnsubscribeNativeGestureEvents(e.OldHandler);
    }

    //
    // Parameters:
    //   listener:
    public void AddListener(IGestureListener listener)
    {
        if (listener is IPanGestureListener item)
        {
            if (panGestureListeners == null)
            {
                panGestureListeners = new List<IPanGestureListener>();
            }

            panGestureListeners.Add(item);
        }

        if (listener is IPinchGestureListener item2)
        {
            if (pinchGestureListeners == null)
            {
                pinchGestureListeners = new List<IPinchGestureListener>();
            }

            pinchGestureListeners.Add(item2);
        }

        if (listener is ILongPressGestureListener item3)
        {
            if (longPressGestureListeners == null)
            {
                longPressGestureListeners = new List<ILongPressGestureListener>();
            }

            longPressGestureListeners.Add(item3);
        }

        if (listener is ITapGestureListener item4)
        {
            if (tapGestureListeners == null)
            {
                tapGestureListeners = new List<ITapGestureListener>();
            }

            tapGestureListeners.Add(item4);
        }

        if (listener is IRightTapGestureListener item5)
        {
            if (rightTapGestureListeners == null)
            {
                rightTapGestureListeners = new List<IRightTapGestureListener>();
            }

            rightTapGestureListeners.Add(item5);
        }

        if (listener is IDoubleTapGestureListener item6)
        {
            if (doubleTapGestureListeners == null)
            {
                doubleTapGestureListeners = new List<IDoubleTapGestureListener>();
            }

            doubleTapGestureListeners.Add(item6);
        }

        if (!isViewListenerAdded)
        {
            CreateNativeListener();
        }

        isViewListenerAdded = true;
    }

    //
    // Parameters:
    //   listener:
    public void RemoveListener(IGestureListener listener)
    {
        if (listener is IPanGestureListener item && panGestureListeners != null && panGestureListeners.Contains(item))
        {
            panGestureListeners.Remove(item);
        }

        if (listener is IPinchGestureListener item2 && pinchGestureListeners != null && pinchGestureListeners.Contains(item2))
        {
            pinchGestureListeners.Remove(item2);
        }

        if (listener is ILongPressGestureListener item3 && longPressGestureListeners != null && longPressGestureListeners.Contains(item3))
        {
            longPressGestureListeners.Remove(item3);
        }

        if (listener is ITapGestureListener item4 && tapGestureListeners != null && tapGestureListeners.Contains(item4))
        {
            tapGestureListeners.Remove(item4);
        }

        if (listener is IRightTapGestureListener item5 && rightTapGestureListeners != null && rightTapGestureListeners.Contains(item5))
        {
            rightTapGestureListeners.Remove(item5);
        }

        if (listener is IDoubleTapGestureListener item6 && doubleTapGestureListeners != null && doubleTapGestureListeners.Contains(item6))
        {
            doubleTapGestureListeners.Remove(item6);
        }
    }

    public void ClearListeners()
    {
        rightTapGestureListeners?.Clear();
        tapGestureListeners?.Clear();
        doubleTapGestureListeners?.Clear();
        pinchGestureListeners?.Clear();
        panGestureListeners?.Clear();
        longPressGestureListeners?.Clear();
    }

    public bool HasListener()
    {
        List<ITapGestureListener>? list = tapGestureListeners;
        int result;
        if (list == null || list.Count <= 0)
        {
            List<IDoubleTapGestureListener>? list2 = doubleTapGestureListeners;
            if (list2 == null || list2.Count <= 0)
            {
                List<ILongPressGestureListener>? list3 = longPressGestureListeners;
                if (list3 == null || list3.Count <= 0)
                {
                    List<IPinchGestureListener>? list4 = pinchGestureListeners;
                    if (list4 == null || list4.Count <= 0)
                    {
                        List<IPanGestureListener>? list5 = panGestureListeners;
                        if (list5 == null || list5.Count <= 0)
                        {
                            List<IRightTapGestureListener>? list6 = rightTapGestureListeners;
                            result = ((list6 != null && list6.Count > 0) ? 1 : 0);
                            goto IL_008c;
                        }
                    }
                }
            }
        }

        result = 1;
        goto IL_008c;
    IL_008c:
        return (byte)result != 0;
    }

    //
    // Exceptions:
    //   T:System.NotImplementedException:
    public void Dispose()
    {
        Dispose(disposing: true);
    }

    //
    // Parameters:
    //   disposing:
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            _disposed = true;
            if (disposing)
            {
                isViewListenerAdded = false;
                ClearListeners();
                Unsubscribe(MauiView);
            }
        }
    }

    //
    // Summary:
    //     Invoke on pinch interaction.
    //
    // Parameters:
    //   position:
    //
    //   state:
    //     Type ofMicrosoft.Maui.GestureStatus
    //
    //   point:
    //     Type of Microsoft.Maui.Graphics.Point
    //
    //   pinchAngle:
    //     Type of System.Double
    //
    //   scale:
    //     Type of System.Single
    internal virtual void OnPinch(Func<IElement?, Point?>? position, GestureStatus state, Point point, double pinchAngle, float scale)
    {
        if (pinchGestureListeners == null)
        {
            return;
        }

        PinchEventArgs e = new PinchEventArgs(position, state, point, pinchAngle, scale);
        foreach (IPinchGestureListener pinchGestureListener in pinchGestureListeners)
        {
            pinchGestureListener.OnPinch(e);
        }
    }

    //
    // Summary:
    //     Invoke on pan interaction.
    //
    // Parameters:
    //   getPosition:
    //
    //   state:
    //
    //   startPoint:
    //     Type of Microsoft.Maui.Graphics.Point
    //
    //   scalePoint:
    //     Type of Microsoft.Maui.Graphics.Point
    //
    //   velocity:
    //     Type of Microsoft.Maui.Graphics.Point
    internal virtual void OnScroll(Func<IElement?, Point?> getPosition, GestureStatus state, Point startPoint, Point scalePoint, Point velocity)
    {
        if (panGestureListeners == null)
        {
            return;
        }

        PanEventArgs e = new PanEventArgs(getPosition, state, startPoint, scalePoint, velocity);
        foreach (IPanGestureListener panGestureListener in panGestureListeners)
        {
            panGestureListener.OnPan(e);
        }
    }

    //
    // Summary:
    //     Invoke on double tap interaction.
    internal virtual void OnTapped(Point touchPoint, int tapCount)
    {
        TapEventArgs e;
        if (tapCount == 1 && tapGestureListeners != null)
        {
            e = new TapEventArgs(touchPoint, tapCount);
            foreach (ITapGestureListener tapGestureListener in tapGestureListeners)
            {
                tapGestureListener.OnTap(MauiView, e);
                tapGestureListener.OnTap(e);
            }
        }

        if (tapCount != 2 || doubleTapGestureListeners == null)
        {
            return;
        }

        e = new TapEventArgs(touchPoint, tapCount);
        foreach (IDoubleTapGestureListener doubleTapGestureListener in doubleTapGestureListeners)
        {
            doubleTapGestureListener.OnDoubleTap(e);
        }
    }

    internal virtual void OnRightTapped(Point touchPoint, PointerDeviceType pointerDeviceType)
    {
        if (rightTapGestureListeners == null)
        {
            return;
        }

        RightTapEventArgs e = new RightTapEventArgs(touchPoint, pointerDeviceType);
        foreach (IRightTapGestureListener rightTapGestureListener in rightTapGestureListeners)
        {
            rightTapGestureListener.OnRightTap(MauiView, e);
            rightTapGestureListener.OnRightTap(e);
        }
    }

    //
    // Summary:
    //     Invoke on long press interaction.
    internal virtual void OnLongPress(Func<IElement?, Point?>? position, Point touchPoint, GestureStatus status)
    {
        if (longPressGestureListeners == null)
        {
            return;
        }

        LongPressEventArgs e = new LongPressEventArgs(position, touchPoint, status);
        foreach (ILongPressGestureListener longPressGestureListener in longPressGestureListeners)
        {
            longPressGestureListener.OnLongPress(e);
        }
    }

    //
    // Summary:
    //     Unsubscribe the events
    //
    // Parameters:
    //   mauiView:
    private void Unsubscribe(View? mauiView)
    {
        if (mauiView != null)
        {
            UnsubscribeNativeGestureEvents(mauiView.Handler);
            mauiView.HandlerChanged -= MauiView_HandlerChanged;
            mauiView.HandlerChanging -= MauiView_HandlerChanging;
            mauiView = null;
        }
    }

    internal void SubscribeNativeGestureEvents(View? mauiView)
    {
        if (mauiView != null && mauiView.Handler?.PlatformView is UIView uIView)
        {
            List<IPinchGestureListener>? list = pinchGestureListeners;
            if (list != null && list.Count > 0)
            {
                pinchGesture = new UIPinchGestureExt(this);
                uIView.AddGestureRecognizer(pinchGesture);
            }

            List<IPanGestureListener>? list2 = panGestureListeners;
            if (list2 != null && list2.Count > 0)
            {
                panGesture = new UIPanGestureExt(this);
                uIView.AddGestureRecognizer(panGesture);
            }

            List<IDoubleTapGestureListener>? list3 = doubleTapGestureListeners;
            if (list3 != null && list3.Count > 0)
            {
                DoubleTapGesture = new UITapGestureExt(this, 2u);
                uIView.AddGestureRecognizer(DoubleTapGesture);
            }

            List<ITapGestureListener>? list4 = tapGestureListeners;
            if (list4 != null && list4.Count > 0)
            {
                tapGesture = new UITapGestureExt(this, 1u);
                uIView.AddGestureRecognizer(tapGesture);
            }

            List<ILongPressGestureListener>? list5 = longPressGestureListeners;
            if (list5 != null && list5.Count > 0)
            {
                longPressGesture = new UILongPressGestureExt(this);
                uIView.AddGestureRecognizer(longPressGesture);
            }
        }
    }

    internal void CreateNativeListener()
    {
        if (MauiView == null || !(MauiView.Handler?.PlatformView is UIView uIView))
        {
            return;
        }

        if (pinchGesture == null)
        {
            List<IPinchGestureListener>? list = pinchGestureListeners;
            if (list != null && list.Count > 0)
            {
                pinchGesture = new UIPinchGestureExt(this);
                uIView.AddGestureRecognizer(pinchGesture);
            }
        }

        if (panGesture == null)
        {
            List<IPanGestureListener>? list2 = panGestureListeners;
            if (list2 != null && list2.Count > 0)
            {
                panGesture = new UIPanGestureExt(this);
                uIView.AddGestureRecognizer(panGesture);
            }
        }

        if (DoubleTapGesture == null)
        {
            List<IDoubleTapGestureListener>? list3 = doubleTapGestureListeners;
            if (list3 != null && list3.Count > 0)
            {
                DoubleTapGesture = new UITapGestureExt(this, 2u);
                uIView.AddGestureRecognizer(DoubleTapGesture);
            }
        }

        if (tapGesture == null)
        {
            List<ITapGestureListener>? list4 = tapGestureListeners;
            if (list4 != null && list4.Count > 0)
            {
                tapGesture = new UITapGestureExt(this, 1u);
                uIView.AddGestureRecognizer(tapGesture);
            }
        }

        if (longPressGesture == null)
        {
            List<ILongPressGestureListener>? list5 = longPressGestureListeners;
            if (list5 != null && list5.Count > 0)
            {
                longPressGesture = new UILongPressGestureExt(this);
                uIView.AddGestureRecognizer(longPressGesture);
            }
        }
    }

    internal void UnsubscribeNativeGestureEvents(IElementHandler handler)
    {
        if (handler == null || !(handler.PlatformView is UIView uIView))
        {
            return;
        }

        UIGestureRecognizer[] gestureRecognizers = uIView.GestureRecognizers;
        if (gestureRecognizers != null)
        {
            if (pinchGesture != null)
            {
                uIView.RemoveGestureRecognizer(pinchGesture);
            }

            if (panGesture != null)
            {
                uIView.RemoveGestureRecognizer(panGesture);
            }

            if (tapGesture != null)
            {
                uIView.RemoveGestureRecognizer(tapGesture);
            }

            if (DoubleTapGesture != null)
            {
                uIView.RemoveGestureRecognizer(DoubleTapGesture);
            }

            if (longPressGesture != null)
            {
                uIView.RemoveGestureRecognizer(longPressGesture);
            }
        }
    }
}
