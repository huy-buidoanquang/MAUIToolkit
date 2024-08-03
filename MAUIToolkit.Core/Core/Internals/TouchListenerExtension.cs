using MAUIToolkit.Core.Internals.Platforms;

namespace MAUIToolkit.Core.Internals;

//
// Summary:
//     Represents the extension class that create instance for MAUIToolkit.Core.Internals.TouchDetector
//     class and set to target class.
public static class TouchListenerExtension
{
    internal static BindableProperty TouchDetectorProperty = BindableProperty.Create("TouchDetector", typeof(TouchDetector), typeof(View));

    //
    // Summary:
    //     Create the touch detector and add the listener to it.
    //
    // Parameters:
    //   target:
    //
    //   listener:
    public static void AddTouchListener(this Microsoft.Maui.Controls.View target, ITouchListener listener)
    {
        TouchDetector touchDetector = target.GetValue(TouchDetectorProperty) as TouchDetector;
        if (touchDetector == null)
        {
            touchDetector = new TouchDetector(target);
            target.SetValue(TouchDetectorProperty, touchDetector);
        }

        touchDetector.AddListener(listener);
    }

    //
    // Summary:
    //     Remove the listener and detector.
    //
    // Parameters:
    //   target:
    //
    //   listener:
    public static void RemoveTouchListener(this View target, ITouchListener listener)
    {
        if (target.GetValue(TouchDetectorProperty) is TouchDetector touchDetector)
        {
            touchDetector.RemoveListener(listener);
            if (!touchDetector.HasListener())
            {
                touchDetector.Dispose();
                target.SetValue(TouchDetectorProperty, null);
            }
        }
    }

    //
    // Summary:
    //     Clear the listeners and touch detector.
    //
    // Parameters:
    //   target:
    public static void ClearTouchListeners(this View target)
    {
        if (target.GetValue(TouchDetectorProperty) is TouchDetector touchDetector)
        {
            touchDetector.Dispose();
            target.SetValue(TouchDetectorProperty, null);
        }
    }
}
