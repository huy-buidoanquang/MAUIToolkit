using System.Runtime.Versioning;

namespace MAUIToolkit.Core.Internals;

public class PointerEventArgs
{
    private Func<IElement?, Point?>? getPosition;

    public long Id { get; private set; }

    public PointerActions Action { get; private set; }

    //
    // Summary:
    //     Returns actual touch point.
    public Point TouchPoint { get; private set; }

    //
    // Summary:
    //     Returns device type of the pointer.
    public PointerDeviceType PointerDeviceType { get; internal set; } = PointerDeviceType.Touch;


    //
    // Summary:
    //     Returns true if left mouse button pressed for desktop devices.
    public bool IsLeftButtonPressed { get; internal set; } = false;


    //
    // Summary:
    //     Returns true if right mouse button pressed for desktop devices.
    //
    // Remarks:
    //     Currently not support for Mac Catalyst
    [UnsupportedOSPlatform("MACCATALYST")]
    public bool IsRightButtonPressed { get; internal set; } = false;


    //
    // Parameters:
    //   id:
    //
    //   action:
    //
    //   touchPoint:
    public PointerEventArgs(long id, PointerActions action, Point touchPoint)
    {
        Id = id;
        Action = action;
        TouchPoint = touchPoint;
    }

    internal PointerEventArgs(Func<IElement?, Point?>? position, long id, PointerActions action, Point touchPoint)
        : this(id, action, touchPoint)
    {
        getPosition = position;
    }

    //
    // Parameters:
    //   id:
    //
    //   action:
    //
    //   deviceType:
    //
    //   touchPoint:
    public PointerEventArgs(long id, PointerActions action, PointerDeviceType deviceType, Point touchPoint)
    {
        Id = id;
        Action = action;
        TouchPoint = touchPoint;
        PointerDeviceType = deviceType;
    }

    internal PointerEventArgs(Func<IElement?, Point?>? position, long id, PointerActions action, PointerDeviceType deviceType, Point touchPoint)
        : this(id, action, deviceType, touchPoint)
    {
        getPosition = position;
    }

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
}
