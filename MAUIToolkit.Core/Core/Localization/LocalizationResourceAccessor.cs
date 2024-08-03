using System.Globalization;
using System.Reflection;
using System.Resources;

namespace MAUIToolkit.Core.Localization;

public class LocalizationResourceAccessor
{
    private static CultureInfo culture = Thread.CurrentThread.CurrentUICulture;

    //
    // Summary:
    //     Gets or sets the resource manager.
    //
    // Value:
    //     The resource manager.
    public static ResourceManager? ResourceManager { get; set; }

    //
    // Summary:
    //     Gets the culture.
    //
    // Value:
    //     The culture.
    internal static CultureInfo Culture
    {
        get
        {
            return culture;
        }
        set
        {
            culture = value;
        }
    }

    //
    // Summary:
    //     Gets the localized string.
    //
    // Parameters:
    //   text:
    //     Text type
    //
    // Returns:
    //     The string.
    public static string? GetString(string text)
    {
        string result = string.Empty;
        try
        {
            if (ResourceManager != null)
            {
                result = ResourceManager.GetString(text, Culture);
            }
        }
        catch
        {
        }

        return result;
    }

    //
    // Parameters:
    //   baseName:
    public static void InitializeDefaultResource(string baseName)
    {
        if (ResourceManager == null)
        {
            Assembly callingAssembly = Assembly.GetCallingAssembly();
            ResourceManager = new ResourceManager(baseName, callingAssembly);
            Culture = new CultureInfo("en-US");
        }
    }
}
