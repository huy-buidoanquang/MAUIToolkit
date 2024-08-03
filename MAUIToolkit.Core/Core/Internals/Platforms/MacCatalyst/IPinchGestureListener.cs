namespace MAUIToolkit.Core.Internals.Platforms;

//
// Summary:
//     This interface used to call the pinch gesture method.
public interface IPinchGestureListener : IGestureListener
{
    //
    // Summary:
    //     Invoke on scale interaction.
    void OnPinch(PinchEventArgs e);
}
