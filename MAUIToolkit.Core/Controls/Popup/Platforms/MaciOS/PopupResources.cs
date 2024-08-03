using MAUIToolkit.Core.Localization;
using System.Globalization;

namespace MAUIToolkit.Core.Controls.Popup;

//
// Summary:
//     A resource accessor for localization that gives the localised string according
//     to culture.
public class PopupResources : LocalizationResourceAccessor
{
    //
    // Summary:
    //     Gets the culture info.
    //
    // Value:
    //     The culture.
    internal static CultureInfo CultureInfo => System.Globalization.CultureInfo.CurrentUICulture;

    //
    // Summary:
    //     Gets the localized string.
    //
    // Parameters:
    //   text:
    //     Text type.
    //
    // Returns:
    //     The string.
    internal static string GetLocalizedString(string text)
    {
        string text2 = string.Empty;
        if (LocalizationResourceAccessor.ResourceManager != null)
        {
            LocalizationResourceAccessor.Culture = System.Globalization.CultureInfo.CurrentUICulture;
            text2 = LocalizationResourceAccessor.GetString(text);
        }

        if (string.IsNullOrEmpty(text2))
        {
            text2 = GetDefaultString(text);
        }

        return text2;
    }

    //
    // Summary:
    //     Method to get the default custom string based on culture.
    //
    // Parameters:
    //   text:
    //     The default local string to be localized based on culture.
    //
    // Returns:
    //     The default localized string.
    private static string GetDefaultString(string text)
    {
        string result = string.Empty;
        switch (text)
        {
            case "Title":
                result = "Title";
                break;
            case "Message":
                result = "Popup Message";
                break;
            case "DeclineButtonText":
                result = "DECLINE";
                break;
            case "AcceptButtonText":
                result = "ACCEPT";
                break;
        }

        return result;
    }
}
