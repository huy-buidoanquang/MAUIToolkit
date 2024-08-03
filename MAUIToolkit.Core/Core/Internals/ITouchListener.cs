using MAUIToolkit.Core.Internals;

namespace MAUIToolkit.Core.Internals;

public interface ITouchListener
{
    //
    // Summary:
    //     Gets the boolean value indicating to pass the touches on either the parent or
    //     child view.
    bool IsTouchHandled => false;

    //
    // Parameters:
    //   e:
    void OnTouch(Internals.PointerEventArgs e);

    //
    // Summary:
    //     Serves event for the mouse wheel action on the view.
    //
    // Parameters:
    //   e:
    //     MAUIToolkit.Core.Internals.ScrollEventArgs
    void OnScrollWheel(ScrollEventArgs e)
    {
    }
}
