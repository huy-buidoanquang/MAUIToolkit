namespace MAUIToolkit.Core.Events;

//
// Summary:
//     ParallaxScrollingEventArgs defines the scrolling event args.
public class ParallaxScrollingEventArgs : EventArgs
{
    //
    // Summary:
    //     Gets the current X scroll position.
    public double ScrollX { get; internal set; }

    //
    // Summary:
    //     Gets the current Y scroll position.
    public double ScrollY { get; internal set; }

    //
    // Summary:
    //     Gets a value indicating whether the parallax content needs to be animated.
    public bool CanAnimate { get; internal set; }

    //
    // Summary:
    //     Initializes a new instance of the ParallaxScrollingEventArgs class.
    //
    // Parameters:
    //   scrollX:
    //
    //   scrollY:
    //
    //   canAnimate:
    public ParallaxScrollingEventArgs(double scrollX, double scrollY, bool canAnimate = false)
    {
        ScrollX = scrollX;
        ScrollY = scrollY;
        CanAnimate = canAnimate;
    }
}
