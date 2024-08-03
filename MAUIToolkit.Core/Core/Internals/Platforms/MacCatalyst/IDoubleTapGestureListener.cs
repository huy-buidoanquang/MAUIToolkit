namespace MAUIToolkit.Core.Internals.Platforms;

//
// Summary:
//     This interface used to call the double tap gesture method.
public interface IDoubleTapGestureListener : IGestureListener
{
    //
    // Summary:
    //     Invoke on tap interaction.
    //
    // Parameters:
    //   e:
    void OnDoubleTap(TapEventArgs e);
}
