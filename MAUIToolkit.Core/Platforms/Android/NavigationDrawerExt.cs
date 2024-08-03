using Android.Views;
using MAUIToolkit.Core.Controls;

namespace MAUIToolkit.Core.Platforms;

internal class NavigationDrawerExt : CView
{
    internal virtual bool OnInterceptTouchEvent(MotionEvent? ev)
    {
        return false;
    }
}
