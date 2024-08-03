namespace MAUIToolkit.Core.Internals.Platforms;

//
// Summary:
//     This class serves as an event data for the panning action on the view.
public interface IPanGestureListener : IGestureListener
{
    //
    // Summary:
    //     Invoke on panning interaction.
    void OnPan(PanEventArgs e);
}
