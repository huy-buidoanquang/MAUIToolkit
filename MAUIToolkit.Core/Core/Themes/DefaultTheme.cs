using System.CodeDom.Compiler;

namespace MAUIToolkit.Core.Themes;

//
// Summary:
//     Dark theme resource dictionary.
[XamlCompilation(XamlCompilationOptions.Skip)]
[XamlFilePath("Theme\\Resources\\DefaultTheme.xaml")]
public class DefaultTheme : ThemeDictionary
{
    //
    // Summary:
    //     Initializes a new instance of the MAUIToolkit.Core.Themes.DefaultTheme class.
    public DefaultTheme()
    {
        InitializeComponent();
    }

    //
    // Parameters:
    //   isDark:
    public DefaultTheme(bool isDark = false)
        : base(isDark)
    {
        InitializeComponent();
    }

    [GeneratedCode("Microsoft.Maui.Controls.SourceGen", "1.0.0.0")]
    private void InitializeComponent()
    {
        this.LoadFromXaml(typeof(DefaultTheme));
    }
}
