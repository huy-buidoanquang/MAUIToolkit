namespace MAUIToolkit.Core.Internals.Platforms;

//
// Summary:
//     This class serves as an event data for the tap action on the view.
public class RightTapEventArgs
{
    private readonly Point _tapPoint;

    private readonly PointerDeviceType _pointerDeviceType;

    //
    // Summary:
    //     Returns actual touch point.
    public Point TapPoint => _tapPoint;

    //
    // Summary:
    //     Returns device type of the pointer.
    public PointerDeviceType PointerDeviceType => _pointerDeviceType;

    //
    // Summary:
    //     Initializes when MAUIToolkit.Core.Internals.TapEventArgs.
    public RightTapEventArgs(Point touchPoint, PointerDeviceType pointerDeviceType)
    {
        _tapPoint = touchPoint;
        _pointerDeviceType = pointerDeviceType;
    }
}
