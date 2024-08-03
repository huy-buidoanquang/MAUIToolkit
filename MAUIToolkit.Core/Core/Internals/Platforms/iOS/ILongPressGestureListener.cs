namespace MAUIToolkit.Core.Internals.Platforms;

//
// Summary:
//     This interface used to call the long press gesture method.
public interface ILongPressGestureListener : IGestureListener
{
    //
    // Summary:
    //     Invoke on long press interaction.
    //
    // Parameters:
    //   e:
    void OnLongPress(LongPressEventArgs e);
}
