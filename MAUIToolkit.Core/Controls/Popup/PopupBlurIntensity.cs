namespace MAUIToolkit.Core.Controls.Popup;

//
// Summary:
//     Defines constants that specifies the intensity of the blur effect applied to
//     the overlay.
public enum PopupBlurIntensity
{
    //
    // Summary:
    //     Specifies that a light blur effect will be used for the overlay.
    Light,
    //
    // Summary:
    //     Specifies that a very light blur effect will be used for the overlay.
    ExtraLight,
    //
    // Summary:
    //     Specifies a very dark blur effect intensity for the overlay.
    ExtraDark,
    //
    // Summary:
    //     Specifies a dark blur effect intensity for the overlay.
    Dark,
    //
    // Summary:
    //     Specifies that the overlay’s blur effect intensity will be a custom float value
    //     set in MAUIToolkit.Core.Controls.Popup.PopupStyle.BlurRadius.
    Custom
}
