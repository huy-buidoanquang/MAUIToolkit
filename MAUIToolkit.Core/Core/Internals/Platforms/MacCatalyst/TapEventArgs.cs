namespace MAUIToolkit.Core.Internals.Platforms;

//
// Summary:
//     This class serves as an event data for the tap action on the view.
public class TapEventArgs
{
    private readonly int _noOfTapCount;

    private readonly Point _tapPoint;

    //
    // Summary:
    //     Returns actual touch point.
    public Point TapPoint => _tapPoint;

    //
    // Summary:
    //     Returns tap count on touch point.
    public int TapCount => _noOfTapCount;

    //
    // Summary:
    //     Initializes when MAUIToolkit.Core.Internals.TapEventArgs.
    public TapEventArgs(Point touchPoint, int tapCount)
    {
        _tapPoint = touchPoint;
        _noOfTapCount = tapCount;
    }
}
