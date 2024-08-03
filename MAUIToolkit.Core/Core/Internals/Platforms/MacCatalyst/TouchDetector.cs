using CoreGraphics;
using MAUIToolkit.Core.Internals;
using UIKit;

namespace MAUIToolkit.Core.Internals.Platforms
{
    public class TouchDetector : IDisposable
    {
        private readonly List<ITouchListener> touchListeners;

        private bool _disposed;

        internal readonly View MauiView;

        //
        // Parameters:
        //   mauiView:
        public TouchDetector(View mauiView)
        {
            MauiView = mauiView;
            touchListeners = new List<ITouchListener>();
            if (mauiView.Handler != null)
            {
                SubscribeNativeTouchEvents(mauiView);
                return;
            }

            mauiView.HandlerChanged += MauiView_HandlerChanged;
            mauiView.HandlerChanging += MauiView_HandlerChanging;
        }

        private void MauiView_HandlerChanged(object? sender, EventArgs e)
        {
            if (sender is View view && view.Handler != null)
            {
                SubscribeNativeTouchEvents(view);
            }
        }

        private void MauiView_HandlerChanging(object? sender, HandlerChangingEventArgs e)
        {
            UnsubscribeNativeTouchEvents(e.OldHandler);
        }

        //
        // Parameters:
        //   listener:
        public void AddListener(ITouchListener listener)
        {
            if (!touchListeners.Contains(listener))
            {
                touchListeners.Add(listener);
            }
        }

        //
        // Parameters:
        //   listener:
        public void RemoveListener(ITouchListener listener)
        {
            if (touchListeners.Contains(listener))
            {
                touchListeners.Remove(listener);
            }
        }

        public bool HasListener()
        {
            return touchListeners.Count > 0;
        }

        public void ClearListeners()
        {
            touchListeners.Clear();
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
                    ClearListeners();
                    Unsubscribe(MauiView);
                }
            }
        }

        internal void OnTouchAction(Func<IElement?, Point?> getPosition, long pointerId, PointerActions action, Point point)
        {
            PointerEventArgs eventArgs = new PointerEventArgs(getPosition, pointerId, action, point);
            OnTouchAction(eventArgs);
        }

        internal void OnTouchAction(Func<IElement?, Point?> getPosition, long pointerId, PointerActions action, PointerDeviceType deviceType, Point point)
        {
            PointerEventArgs eventArgs = new PointerEventArgs(getPosition, pointerId, action, deviceType, point);
            OnTouchAction(eventArgs);
        }

        internal void OnTouchAction(PointerEventArgs eventArgs)
        {
            foreach (ITouchListener touchListener in touchListeners)
            {
                touchListener.OnTouch(eventArgs);
            }
        }

        internal bool OnScrollAction(long pointerId, Point origin, double direction, bool? handled = null)
        {
            ScrollEventArgs scrollEventArgs = new ScrollEventArgs(pointerId, origin, direction);
            if (handled.HasValue)
            {
                scrollEventArgs.Handled = handled.Value;
            }

            foreach (ITouchListener touchListener in touchListeners)
            {
                touchListener.OnScrollWheel(scrollEventArgs);
            }

            return scrollEventArgs.Handled;
        }

        //
        // Summary:
        //     Unsubscribe the events.
        private void Unsubscribe(View? mauiView)
        {
            if (mauiView != null)
            {
                UnsubscribeNativeTouchEvents(mauiView.Handler);
                mauiView.HandlerChanged -= MauiView_HandlerChanged;
                mauiView.HandlerChanging -= MauiView_HandlerChanging;
                mauiView = null;
            }
        }

        internal void SubscribeNativeTouchEvents(View? mauiView)
        {
            if (mauiView != null && mauiView.Handler != null && mauiView.Handler?.PlatformView is UIView uIView)
            {
                UITouchRecognizerExt gestureRecognizer = new UITouchRecognizerExt(this);
                uIView.AddGestureRecognizer(gestureRecognizer);
                if (OperatingSystem.IsIOSVersionAtLeast(13))
                {
                    UIHoverRecognizerExt gestureRecognizer2 = new UIHoverRecognizerExt(this);
                    uIView.AddGestureRecognizer(gestureRecognizer2);
                }

                UIScrollRecognizerExt gestureRecognizer3 = new UIScrollRecognizerExt(this);
                uIView.AddGestureRecognizer(gestureRecognizer3);
            }
        }

        internal void UnsubscribeNativeTouchEvents(IElementHandler handler)
        {
            if (handler == null || !(handler.PlatformView is UIView uIView))
            {
                return;
            }

            UIGestureRecognizer[] gestureRecognizers = uIView.GestureRecognizers;
            if (gestureRecognizers == null)
            {
                return;
            }

            UIGestureRecognizer[] array = gestureRecognizers;
            foreach (UIGestureRecognizer uIGestureRecognizer in array)
            {
                if (uIGestureRecognizer is UITouchRecognizerExt || uIGestureRecognizer is UIHoverRecognizerExt || uIGestureRecognizer is UIScrollRecognizerExt)
                {
                    uIView.RemoveGestureRecognizer(uIGestureRecognizer);
                }
            }
        }

        internal static Point? CalculatePosition(IElement? element, View mauiView, UIGestureRecognizer platformRecognizer)
        {
            IView virtualView = mauiView.Handler.VirtualView;
            UIView uIView = element?.ToPlatform();
            if (virtualView == null)
            {
                return null;
            }

            CGPoint? cGPoint = null;
            if (element == null)
            {
                cGPoint = platformRecognizer.LocationInView(null);
            }
            else if (uIView != null)
            {
                UIView view = uIView;
                if (true)
                {
                    cGPoint = platformRecognizer.LocationInView(view);
                }
            }

            if (!cGPoint.HasValue)
            {
                return null;
            }

            return new Point((int)cGPoint.Value.X, (int)cGPoint.Value.Y);
        }
    }
}
