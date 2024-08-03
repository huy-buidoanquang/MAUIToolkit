namespace MAUIToolkit.Core.Themes;

internal class ThemeResourceDictionary : ResourceDictionary
{
    private ThemeVisuals visualTheme = ThemeVisuals.Light;

    public ThemeVisuals VisualTheme
    {
        get
        {
            return visualTheme;
        }
        set
        {
            visualTheme = value;
            UpdateVisualTheme();
        }
    }

    public ThemeResourceDictionary()
    {
        UpdateDefaultTheme();
    }

    private void UpdateVisualTheme()
    {
        base.MergedDictionaries.Clear();
        if (VisualTheme == ThemeVisuals.Light)
        {
            UpdateDefaultTheme();
        }
        else if (VisualTheme == ThemeVisuals.Dark)
        {
            UpdateDefaultTheme(isDark: true);
        }
    }

    private void UpdateDefaultTheme(bool isDark = false)
    {
        base.MergedDictionaries.Clear();
        base.MergedDictionaries.Add(new DefaultTheme(isDark));
    }
}
