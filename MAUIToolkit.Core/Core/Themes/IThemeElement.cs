namespace MAUIToolkit.Core.Themes;

//
// Summary:
//     The interface from which control's subviews/styles should be inherited.
internal interface IThemeElement
{
    //
    // Summary:
    //     This method will be called when a theme dictionary that contains the value for
    //     your control key is merged in application.
    //
    // Parameters:
    //   oldTheme:
    //     Old theme.
    //
    //   newTheme:
    //     New theme.
    void OnControlThemeChanged(string oldTheme, string newTheme);

    //
    // Summary:
    //     This method will be called when users merge a theme dictionary that contains
    //     value for “ApplicationTheme” dynamic resource key.
    //
    // Parameters:
    //   oldTheme:
    //     Old theme.
    //
    //   newTheme:
    //     New theme.
    void OnCommonThemeChanged(string oldTheme, string newTheme);
}
