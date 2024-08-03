namespace MAUIToolkit.Core.Internals.Platforms;

//
// Summary:
//     This class serves as an event data for the panning action on the view.
public class PanEventArgs
{
    private readonly Point _touchPoint;

    private readonly Point _translatePoint;

    private readonly GestureStatus _status;

    private readonly Point _velocity;

    private Func<IElement?, Point?>? getPosition;

    //
    // Summary:
    //     Returns Microsoft.Maui.GestureStatus on pan interaction.
    public GestureStatus Status => _status;

    //
    // Summary:
    //     Returns translate distance point on panning.
    public Point TranslatePoint => _translatePoint;

    //
    // Summary:
    //     Returns actual touch point on panning.
    public Point TouchPoint => _touchPoint;

    //
    // Summary:
    //     Returns the pan velocity values in X and Y direction.
    //
    // Value:
    //     Velocity values start from 0, 1000, 2000... ranges based on X and Y directions.
    //     While panning with less friction the velocity values are in the range of 0 to
    //     1000 based on X and Y directions.
    public Point Velocity => _velocity;

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
    //     Initializes when MAUIToolkit.Core.Internals.PanEventArgs
    public PanEventArgs(GestureStatus status, Point touchPoint, Point translatePoint, Point velocity)
    {
        _status = status;
        _touchPoint = touchPoint;
        _translatePoint = translatePoint;
        _velocity = velocity;
    }

    internal PanEventArgs(Func<IElement?, Point?>? position, GestureStatus status, Point touchPoint, Point translatePoint, Point velocity)
        : this(status, touchPoint, translatePoint, velocity)
    {
        getPosition = position;
    }
}
