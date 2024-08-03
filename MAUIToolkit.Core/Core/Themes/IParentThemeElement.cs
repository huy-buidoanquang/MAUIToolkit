namespace MAUIToolkit.Core.Themes;

//
// Summary:
//     The interface from which control should be inherited.
internal interface IParentThemeElement : IThemeElement
{
    //
    // Summary:
    //     This method is declared only in IParentThemeElement and you need to implement
    //     this method only in main control.
    //
    // Returns:
    //     ResourceDictionary
    ResourceDictionary GetThemeDictionary();
}
