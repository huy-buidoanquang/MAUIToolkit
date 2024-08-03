namespace MAUIToolkit.Core.Internals.Platforms;

internal class WindowOverlayContainer : Microsoft.Maui.Controls.View
{
    internal virtual bool canHandleTouch => false;

    internal virtual void ProcessTouchInteraction(float x, float y)
    {
    }
}
