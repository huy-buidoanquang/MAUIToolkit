using Android.Views;

namespace MAUIToolkit.Core.Platforms;

internal class TabViewExt : ContentView.CContentView
{
    internal virtual bool OnInterceptTouchEvent(MotionEvent? ev)
    {
        return false;
    }
}
