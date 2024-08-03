namespace MAUIToolkit.Core.Internals.Platforms;

//
// Summary:
//     This class serves as an event data for the long press action on the view.
public class LongPressEventArgs
{
    private readonly Point _touchPoint;

    private Func<IElement?, Point?>? getPosition;

    private readonly GestureStatus _status;

    internal GestureStatus Status => _status;

    //
    // Summary:
    //     Returns actual touch point on long press.
    public Point TouchPoint => _touchPoint;

    //
    // Summary:
    //     Method to obtain the touch position relative to the given element.
    //
    // Parameters:
    //   relativeTo:
    public Point? GetPosition(Element? relativeTo)
    {
        return getPosition?.Invoke(relativeTo);
    }

    //
    // Summary:
    //     Initializes when MAUIToolkit.Core.Internals.LongPressEventArgs.
    public LongPressEventArgs(Point touchPoint, GestureStatus status)
    {
        _touchPoint = touchPoint;
        _status = status;
    }

    internal LongPressEventArgs(Func<IElement?, Point?>? position, Point touchPoint, GestureStatus status)
        : this(touchPoint, status)
    {
        getPosition = position;
    }
}
