using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Widget;

namespace MAUIToolkit.Core.Internals.Platforms;

internal class WindowOverlayStack : FrameLayout
{
    internal bool HasBlurMode { get; set; }

    public WindowOverlayStack(Context context)
        : base(context)
    {
    }

    public WindowOverlayStack(Context context, IAttributeSet? attrs)
        : base(context, attrs)
    {
    }

    public WindowOverlayStack(Context context, IAttributeSet? attrs, int defStyleAttr)
        : base(context, attrs, defStyleAttr)
    {
    }

    public WindowOverlayStack(Context context, IAttributeSet? attrs, int defStyleAttr, int defStyleRes)
        : base(context, attrs, defStyleAttr, defStyleRes)
    {
    }

    protected WindowOverlayStack(nint javaReference, JniHandleOwnership transfer)
        : base(javaReference, transfer)
    {
    }
}
