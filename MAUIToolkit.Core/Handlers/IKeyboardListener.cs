namespace MAUIToolkit.Core;

//
// Summary:
//     This interface used to call the OnKeyDown method.
public interface IKeyboardListener
{
    //
    // Summary:
    //     Gets a value indicating whether the view can become the first responder to listen
    //     the keyboard actions.
    //
    // Remarks:
    //     This property will be considered only in iOS Platform.
    bool CanBecomeFirstResponder => false;

    //
    // Summary:
    //     Invoke on key down action.
    //
    // Parameters:
    //   args:
    void OnKeyDown(KeyEventArgs args);

    //
    // Summary:
    //     Invoke on key up action.
    //
    // Parameters:
    //   args:
    void OnKeyUp(KeyEventArgs args);

    //
    // Summary:
    //     Invoke before the key down action.
    //
    // Parameters:
    //   args:
    //
    // Remarks:
    //     This method will be triggered only in WinUI Platform.
    void OnPreviewKeyDown(KeyEventArgs args)
    {
    }
}
