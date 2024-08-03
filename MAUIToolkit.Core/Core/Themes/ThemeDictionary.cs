using System.CodeDom.Compiler;

namespace MAUIToolkit.Core.Themes;

//
// Summary:
//     ThemeDictionary class for Controls in which controls themes are to be included.
[XamlCompilation(XamlCompilationOptions.Skip)]
[XamlFilePath("Theme\\Resources\\ThemeDictionary.xaml")]
public class ThemeDictionary : ResourceDictionary
{
    //
    // Summary:
    //     Initializes a new instance of the MAUIToolkit.Core.Themes.ThemeDictionary
    //     class.
    public ThemeDictionary()
    {
        InitializeElement();
    }

    //
    // Parameters:
    //   isDark:
    public ThemeDictionary(bool isDark = false)
    {
        if (isDark)
        {
            base.MergedDictionaries.Add(new DarkThemeColors());
        }
        else
        {
            base.MergedDictionaries.Add(new LightThemeColors());
        }

        InitializeElement();
    }

    private void InitializeElement()
    {
        InitializeComponent();
        ThemeElement.AddStyleDictionary(this);
    }

    [GeneratedCode("Microsoft.Maui.Controls.SourceGen", "1.0.0.0")]
    private void InitializeComponent()
    {
        this.LoadFromXaml(typeof(ThemeDictionary));
    }
}
