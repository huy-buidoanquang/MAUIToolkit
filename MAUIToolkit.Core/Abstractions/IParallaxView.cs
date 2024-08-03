using MAUIToolkit.Core.Events;

namespace MAUIToolkit.Core.Abstractions;

//
// Summary:
//     The interface defines a property and an event that needs to be implemented to
//     get relevant data from custom scrollable elements to perform parallax scrolling.
public interface IParallaxView
{
    //
    // Summary:
    //     Gets or sets the total content size of the scrollable source element.
    Size ScrollableContentSize { get; set; }

    //
    // Summary:
    //     Raise this event whenever the source scrollable element is scrolled.
    event EventHandler<ParallaxScrollingEventArgs> Scrolling;
}
