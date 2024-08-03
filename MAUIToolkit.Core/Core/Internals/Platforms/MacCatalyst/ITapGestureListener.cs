namespace MAUIToolkit.Core.Internals.Platforms;

//
// Summary:
//     This interface used to call the tap gesture method.
public interface ITapGestureListener : IGestureListener
{
    //
    // Summary:
    //     Invoke on tap interaction.
    //
    // Parameters:
    //   e:
    void OnTap(TapEventArgs e);

    //
    // Summary:
    //     Invoke on tap interaction with sender argument
    //
    // Parameters:
    //   sender:
    //
    //   e:
    void OnTap(object sender, TapEventArgs e)
    {
    }

    //
    // Summary:
    //     Invoke on tap interaction to prevent the touch event of parent element.
    //
    // Parameters:
    //   view:
    void ShouldHandleTap(object view)
    {
    }
}
