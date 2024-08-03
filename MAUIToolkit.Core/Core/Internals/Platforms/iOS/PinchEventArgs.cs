namespace MAUIToolkit.Core.Internals.Platforms;

//
// Summary:
//     This class serves as an event data for the pinch action on the view.
public class PinchEventArgs
{
    private readonly Point _touchPoint;

    private readonly double _angle;

    private readonly float _scale;

    private readonly GestureStatus _status;

    private Func<IElement?, Point?>? getPosition;

    //
    // Summary:
    //     Returns Microsoft.Maui.GestureStatus on pinch interaction.
    public GestureStatus Status => _status;

    //
    // Summary:
    //     Returns scale value on pinch interaction.
    public float Scale => _scale;

    //
    // Summary:
    //     Returns actual touch point on pinch interaction.
    public Point TouchPoint => _touchPoint;

    //
    // Summary:
    //     Returns angle on pinch interaction.
    public double Angle => _angle;

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
    //     Initializes when MAUIToolkit.Core.Internals.PinchEventArgs
    public PinchEventArgs(GestureStatus status, Point touchPoint, double angle, float scale)
    {
        _status = status;
        _touchPoint = touchPoint;
        _scale = scale;
        _angle = angle;
    }

    internal PinchEventArgs(Func<IElement?, Point?>? position, GestureStatus status, Point touchPoint, double angle, float scale)
        : this(status, touchPoint, angle, scale)
    {
        getPosition = position;
    }
}
