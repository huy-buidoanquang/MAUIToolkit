namespace MAUIToolkit.Core.Internals;

public class ScrollEventArgs
{
    //
    // Summary:
    //     Returns pointer Id.
    public long PointerID { get; private set; }

    //
    // Summary:
    //     Returns mouse wheel moving delta.
    public double ScrollDelta { get; private set; }

    //
    // Summary:
    //     Returns actual touch point.
    public Point TouchPoint { get; private set; }

    //
    // Summary:
    //     Gets or sets a value that marks the routed event as handled, and prevents most
    //     handlers along the event route from handling the same event again.
    //
    // Remarks:
    //     It is applicable only for routed events which are supported in Windows.
    internal bool Handled { get; set; }

    //
    // Summary:
    //     Initializes when MAUIToolkit.Core.Internals.ScrollEventArgs
    public ScrollEventArgs(long id, Point origin, double direction)
    {
        PointerID = id;
        TouchPoint = origin;
        ScrollDelta = direction;
    }
}
