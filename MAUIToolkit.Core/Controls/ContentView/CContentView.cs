using MAUIToolkit.Core.Controls;

namespace MAUIToolkit.Core.ContentView;

[ContentProperty("Content")]
public abstract class CContentView : CView
{
    //
    // Summary:
    //     Identifies the MAUIToolkit.Core.ContentView.Content bindable property.
    //
    //
    // Value:
    //     The identifier for MAUIToolkit.Core.ContentView.Content bindable property.
    public static readonly BindableProperty ContentProperty = BindableProperty.Create("Content", typeof(View), typeof(CContentView), null, BindingMode.OneWay, null, OnContentPropertyChanged);

    public View Content
    {
        get
        {
            return (View)GetValue(ContentProperty);
        }
        set
        {
            SetValue(ContentProperty, value);
        }
    }

    //
    // Summary:
    //     Invoked whenever the MAUIToolkit.Core.ContentView.ContentProperty is set
    //     for ContentView.
    //
    // Parameters:
    //   bindable:
    //     The bindable.
    //
    //   oldValue:
    //     The old value.
    //
    //   newValue:
    //     The new value.
    private static void OnContentPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is CContentView contentView)
        {
            contentView?.OnContentChanged(oldValue, newValue);
        }
    }

    protected virtual void OnContentChanged(object oldValue, object newValue)
    {
        if (oldValue != null && oldValue is View view)
        {
            Remove(view);
        }

        if (newValue != null && newValue is View view2)
        {
            Add(view2);
        }
    }
}
