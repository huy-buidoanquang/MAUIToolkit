using Android.Content;
using Android.Views;
using MAUIToolkit.Core.Internals;
using Microsoft.Maui.Platform;

namespace MAUIToolkit.Core.Internals.Platforms;

public class TouchDetector : IDisposable
{
    private readonly List<ITouchListener> touchListeners;

    private bool _disposed;

    internal readonly Microsoft.Maui.Controls.View MauiView;

    internal void SubscribeNativeTouchEvents(Microsoft.Maui.Controls.View? mauiView)
    {
        if (mauiView != null && mauiView.Handler?.PlatformView is Android.Views.View view)
        {
            view.Touch += OnTouch;
        }
    }

    internal void UnsubscribeNativeTouchEvents(IElementHandler handler)
    {
        if (handler != null && handler.PlatformView is Android.Views.View view)
        {
            view.Touch -= OnTouch;
        }
    }

    internal void OnTouch(object? sender, Android.Views.View.TouchEventArgs e)
    {
        if (!MauiView.IsEnabled || MauiView.InputTransparent)
        {
            return;
        }

        Android.Views.View view = sender as Android.Views.View;
        MotionEvent motionEvent = e.Event;
        if (view == null || motionEvent == null)
        {
            return;
        }

        int actionIndex = motionEvent.ActionIndex;
        int pointerId = motionEvent.GetPointerId(actionIndex);
        Point point = new Point(motionEvent.GetX(actionIndex), motionEvent.GetY(actionIndex));
        Func<double, double> func = Android.App.Application.Context.FromPixels;
        Point point2 = new Point(func(point.X), func(point.Y));
        bool disallowIntercept = touchListeners[0].IsTouchHandled || motionEvent.Action == MotionEventActions.Pointer2Down || motionEvent.PointerCount > 1;
        if (view.Parent != null)
        {
            view.Parent.RequestDisallowInterceptTouchEvent(disallowIntercept);
        }

        PointerDeviceType deviceType = PointerDeviceType.Touch;
        if (motionEvent.PointerCount > 0 && motionEvent.GetToolType(0) == MotionEventToolType.Stylus)
        {
            deviceType = PointerDeviceType.Stylus;
        }

        switch (motionEvent.ActionMasked)
        {
            case MotionEventActions.Down:
            case MotionEventActions.Pointer1Down:
                OnTouchAction((IElement? relativeTo) => CalculatePosition(motionEvent, MauiView, relativeTo), pointerId, PointerActions.Pressed, deviceType, point2);
                break;
            case MotionEventActions.Move:
                OnTouchAction((IElement? relativeTo) => CalculatePosition(motionEvent, MauiView, relativeTo), pointerId, PointerActions.Moved, deviceType, point2);
                break;
            case MotionEventActions.Up:
            case MotionEventActions.Pointer1Up:
                OnTouchAction((IElement? relativeTo) => CalculatePosition(motionEvent, MauiView, relativeTo), pointerId, PointerActions.Released, deviceType, point2);
                break;
            case MotionEventActions.Cancel:
                OnTouchAction((IElement? relativeTo) => CalculatePosition(motionEvent, MauiView, relativeTo), pointerId, PointerActions.Cancelled, deviceType, point2);
                break;
            case MotionEventActions.Outside:
                break;
        }
    }

    internal static Point? CalculatePosition(MotionEvent? e, IElement? sourceElement, IElement? relativeElement)
    {
        Context context = sourceElement?.Handler?.MauiContext?.Context;
        if (context == null)
        {
            return null;
        }

        if (e == null)
        {
            return null;
        }

        if (relativeElement == null)
        {
            return new Point(context.FromPixels(e.RawX), context.FromPixels(e.RawY));
        }

        if (relativeElement == sourceElement)
        {
            return new Point(context.FromPixels(e.GetX()), context.FromPixels(e.GetY()));
        }

        if (relativeElement?.Handler?.PlatformView is Android.Views.View view)
        {
            Point locationOnScreenPx = GetLocationOnScreenPx(view);
            double pixels = (double)e.RawX - locationOnScreenPx.X;
            double pixels2 = (double)e.RawY - locationOnScreenPx.Y;
            return new Point(context.FromPixels(pixels), context.FromPixels(pixels2));
        }

        return null;
    }

    internal static Point GetLocationOnScreenPx(Android.Views.View view)
    {
        int[] array = new int[2];
        view.GetLocationOnScreen(array);
        return new Point(array[0], array[1]);
    }

    //
    // Parameters:
    //   mauiView:
    public TouchDetector(Microsoft.Maui.Controls.View mauiView)
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
        if (sender is Microsoft.Maui.Controls.View view && view.Handler != null)
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
    private void Unsubscribe(Microsoft.Maui.Controls.View? mauiView)
    {
        if (mauiView != null)
        {
            UnsubscribeNativeTouchEvents(mauiView.Handler);
            mauiView.HandlerChanged -= MauiView_HandlerChanged;
            mauiView.HandlerChanging -= MauiView_HandlerChanging;
            mauiView = null;
        }
    }
}
