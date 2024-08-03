using Microsoft.Maui.Controls.Internals;
using System.Reflection;

namespace MAUIToolkit.Core.Themes;

//
// Summary:
//     ThemeElement from which SubViews/Styles of element should be inherited.
internal static class ThemeElement
{
    //
    // Summary:
    //     The common theme property to get common theme.
    public static BindableProperty CommonThemeProperty = BindableProperty.Create("CommonTheme", typeof(string), typeof(IThemeElement), "Default", BindingMode.OneWay, null, OnCommonThemePropertyChanged);

    //
    // Summary:
    //     The control theme property to get theme of the respective control.
    public static BindableProperty ControlThemeProperty = BindableProperty.Create("ControlTheme", typeof(string), typeof(IThemeElement), "Default", BindingMode.OneWay, null, OnControlThemeChanged);

    //
    // Summary:
    //     The property used to cache resource dictionaries which are belonged to control.
    private static readonly Dictionary<string, WeakReference<ResourceDictionary>> ControlThemeCache = new Dictionary<string, WeakReference<ResourceDictionary>>();

    //
    // Summary:
    //     The style target dictionaries.
    private static readonly List<WeakReference<ResourceDictionary>> StyleTargetDictionaries = new List<WeakReference<ResourceDictionary>>();

    //
    // Summary:
    //     The control key property.
    private static BindableProperty controlKeyProperty = BindableProperty.Create("ControlKey", typeof(string), typeof(IThemeElement), string.Empty);

    //
    // Summary:
    //     The implicit style property.
    private static BindableProperty implicitStyleProperty = BindableProperty.Create("ImplicitStyle", typeof(Style), typeof(IThemeElement), null, BindingMode.OneWay, null, OnImplicitStyleChanged);

    //
    // Summary:
    //     The Dictionary contains pending dictionaries to which are to be merged.
    private static Dictionary<ResourceDictionary, List<ResourceDictionary>> pendingDictionariesToMerge = new Dictionary<ResourceDictionary, List<ResourceDictionary>>();

    //
    // Summary:
    //     Holds element of setter object, to call apply method
    private static object[] elements = new object[2];

    //
    // Summary:
    //     Call this static method in the constructor for which implement IParentThemeElement
    //     and IThemeElement interfaces.
    //
    // Parameters:
    //   element:
    //     Element.
    //
    //   controlKey:
    //     Control key.
    internal static void InitializeThemeResources(Element element, string controlKey)
    {
        if (element == null)
        {
            throw new ArgumentNullException("element");
        }

        Type type = element.GetType();
        element.SetValue(controlKeyProperty, controlKey);
        element.SetDynamicResource(CommonThemeProperty, "ApplicationTheme");
        element.SetDynamicResource(ControlThemeProperty, controlKey);
        if (!(element is VisualElement))
        {
            string fullName = type.FullName;
            element.SetDynamicResource(implicitStyleProperty, fullName);
        }
    }

    //
    // Summary:
    //     Adds the style dictionary.
    //
    // Parameters:
    //   resourceDictionary:
    //     Resource dictionary.
    internal static void AddStyleDictionary(ResourceDictionary resourceDictionary)
    {
        if (resourceDictionary == null)
        {
            throw new ArgumentNullException("resourceDictionary");
        }

        StyleTargetDictionaries.Add(new WeakReference<ResourceDictionary>(resourceDictionary));
    }

    //
    // Summary:
    //     Merges the pending dictionaries to existing resource dictionaries.
    private static void MergePendingDictionaries()
    {
        if (pendingDictionariesToMerge == null)
        {
            return;
        }

        foreach (KeyValuePair<ResourceDictionary, List<ResourceDictionary>> item in pendingDictionariesToMerge)
        {
            ResourceDictionary key = item.Key;
            foreach (ResourceDictionary item2 in item.Value)
            {
                key.MergedDictionaries.Add(item2);
            }

            item.Value.Clear();
        }

        pendingDictionariesToMerge.Clear();
    }

    //
    // Summary:
    //     Called when control theme was changed implicitily
    //
    // Parameters:
    //   bindable:
    //     Bindable.
    //
    //   oldValue:
    //     Old value.
    //
    //   newValue:
    //     New value.
    private static void OnImplicitStyleChanged(BindableObject bindable, object oldValue, object newValue)
    {
        Style style = newValue as Style;
        if (!(bindable is Element element) || style == null || ApplyStyle(element, style))
        {
            return;
        }

        foreach (Setter setter in style.Setters)
        {
            if (setter.Value is DynamicResource dynamicResource)
            {
                element.SetDynamicResource(setter.Property, dynamicResource.Key);
            }
            else
            {
                element.SetValue(setter.Property, setter.Value);
            }
        }
    }

    //
    // Summary:
    //     Returns a boolean value indicating whether the Setter's Apply Method is invoked
    //     using Reflection or not.
    //
    // Parameters:
    //   element:
    //
    //   style:
    private static bool ApplyStyle(Element element, Style style)
    {
        MethodInfo methodInfo = typeof(Style).GetInterface("IStyle")?.GetMethod("Apply");
        if (methodInfo != null)
        {
            elements[0] = element;
            methodInfo.Invoke(style, elements);
            return true;
        }

        return false;
    }

    //
    // Summary:
    //     Implementation of common theme property changed.
    //
    // Parameters:
    //   bindable:
    //     Bindable.
    //
    //   oldValue:
    //     Old value.
    //
    //   newValue:
    //     New value.
    private static void OnCommonThemePropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is IThemeElement themeElement)
        {
            themeElement.OnCommonThemeChanged((string)oldValue, (string)newValue);
        }
    }

    //
    // Summary:
    //     Merges the theme dictionary to pending dictionaries.
    //
    // Parameters:
    //   key:
    //     Key.
    //
    //   themeDictionary:
    //     Theme dictionary.
    private static void MergeThemeDictionary(string key, ResourceDictionary themeDictionary)
    {
        if (themeDictionary == null)
        {
            return;
        }

        int count = StyleTargetDictionaries.Count;
        for (int num = count - 1; num >= 0; num--)
        {
            WeakReference<ResourceDictionary> weakReference = StyleTargetDictionaries[num];
            weakReference.TryGetTarget(out var target);
            if (target != null && target.TryGetValue(key, out var _) && !target.MergedDictionaries.Contains(themeDictionary))
            {
                List<ResourceDictionary> list = null;
                if (!pendingDictionariesToMerge.ContainsKey(target))
                {
                    list = new List<ResourceDictionary>();
                    pendingDictionariesToMerge.Add(target, list);
                }
                else
                {
                    list = pendingDictionariesToMerge[target];
                }

                list.Add(themeDictionary);
                MergePendingDictionaries();
            }
        }
    }

    //
    // Summary:
    //     Tries the get theme dictionary from Control.
    //
    // Parameters:
    //   element:
    //     Element.
    //
    //   resourceDictionary:
    //     Resource dictionary.
    //
    // Returns:
    //     true, if get theme dictionary was tryed, false otherwise.
    private static bool TryGetThemeDictionary(VisualElement element, out ResourceDictionary? resourceDictionary)
    {
        resourceDictionary = null;
        if (element != null)
        {
            string key = (string)element.GetValue(controlKeyProperty);
            WeakReference<ResourceDictionary> value = null;
            if (ControlThemeCache.TryGetValue(key, out value) && value.TryGetTarget(out resourceDictionary))
            {
                return true;
            }

            if (ControlThemeCache.ContainsKey(key))
            {
                ControlThemeCache.Remove(key);
            }

            if (element is IParentThemeElement parentThemeElement)
            {
                resourceDictionary = parentThemeElement.GetThemeDictionary();
                if (resourceDictionary != null)
                {
                    value = new WeakReference<ResourceDictionary>(resourceDictionary);
                    ControlThemeCache.Add(key, value);
                    return true;
                }
            }
        }

        return false;
    }

    //
    // Summary:
    //     Implementation of control theme changed.
    //
    // Parameters:
    //   bindable:
    //     Bindable.
    //
    //   oldValue:
    //     Old value.
    //
    //   newValue:
    //     New value.
    private static void OnControlThemeChanged(BindableObject bindable, object oldValue, object newValue)
    {
        IThemeElement themeElement = bindable as IThemeElement;
        if (bindable == null)
        {
            return;
        }

        string key = (string)bindable.GetValue(controlKeyProperty);
        if (bindable is VisualElement element)
        {
            List<WeakReference<ResourceDictionary>> styleTargetDictionaries = StyleTargetDictionaries;
            if (styleTargetDictionaries != null && styleTargetDictionaries.Count > 0 && TryGetThemeDictionary(element, out ResourceDictionary resourceDictionary) && resourceDictionary != null)
            {
                MergeThemeDictionary(key, resourceDictionary);
            }
        }

        themeElement?.OnControlThemeChanged((string)oldValue, (string)newValue);
    }
}
