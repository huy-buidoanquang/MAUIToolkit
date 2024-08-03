namespace MAUIToolkit.Core.Internals.Platforms;

//
// Summary:
//     Holds the MAUIToolkit.Core.Internals.SfWindowOverlay child positioning details
//     for re-layouting during the window resize and orientation changes.
internal class PositionDetails
{
    internal Android.Views.View? Relative { get; set; }

    internal float X { get; set; }

    internal float Y { get; set; }

    internal WindowOverlayHorizontalAlignment HorizontalAlignment { get; set; }

    internal WindowOverlayVerticalAlignment VerticalAlignment { get; set; }
}
