using MAUIToolkit.Core.Primitives;

namespace MAUIToolkit.Core;

//
// Summary:
//     This class serves as an event data for the key down action on the view.
public class KeyEventArgs
{
    //
    // Summary:
    //     Returns key pressed.
    public KeyboardKey Key { get; }

    //
    // Summary:
    //     Gets or sets a value that marks the routed event as handled. A true value for
    //     Handled to restrict the event to be routed to parent.
    public bool Handled { get; set; }

    //
    // Summary:
    //     Returns the pressed state of Shift key
    public bool IsShiftKeyPressed { get; internal set; }

    //
    // Summary:
    //     Returns the pressed state of Control key
    public bool IsCtrlKeyPressed { get; internal set; }

    //
    // Summary:
    //     Returns the pressed state ofAlt key.
    public bool IsAltKeyPressed { get; internal set; }

    //
    // Summary:
    //     Returns the pressed state of Command key.
    //
    // Remarks:
    //     This property is only applicable to the iOS platform.
    public bool IsCommandKeyPressed { get; internal set; }

    //
    // Summary:
    //     Returns whether the Caps Lock is on or not.
    internal bool IsCapsLockOn { get; set; }

    //
    // Summary:
    //     Returns whether the Num Lock is on or not.
    internal bool IsNumLockOn { get; set; }

    //
    // Summary:
    //     Returns whether the Scroll Lock is on or not.
    internal bool IsScrollLockOn { get; set; }

    //
    // Summary:
    //     Gets or sets the type of key action that invokes the key event.
    internal KeyActions KeyAction { get; set; }

    //
    // Summary:
    //     Initializes when MAUIToolkit.Core.Internals.KeyEventArgs.
    //
    // Parameters:
    //   keyboardKey:
    public KeyEventArgs(KeyboardKey keyboardKey)
    {
        Key = keyboardKey;
    }
}
