
using MAUIToolkit.Core.Controls.SignaturePad;
using MAUIToolkit.Core.Themes;
using MAUIToolkit.Core.Handlers;
using System.ComponentModel;

namespace MAUIToolkit.Graphics.Core.SignaturePad;

public class CSignaturePad : View, ICSignaturePad, IView, IElement, ITransform, IParentThemeElement, IThemeElement
{
    public static readonly BindableProperty MaximumStrokeThicknessProperty = BindableProperty.Create("MaximumStrokeThickness", typeof(double), typeof(CSignaturePad), null, BindingMode.OneWay, null, null, null, null, MaximumStrokeThicknessDefaultCreator);
    public static readonly BindableProperty MinimumStrokeThicknessProperty = BindableProperty.Create("MinimumStrokeThickness", typeof(double), typeof(CSignaturePad), null, BindingMode.OneWay, null, null, null, null, MinimumStrokThicknessDefaultCreator);
    public static readonly BindableProperty StrokeColorProperty = BindableProperty.Create("StrokeColor", typeof(Color), typeof(CSignaturePad), null, BindingMode.OneWay, null, null, null, null, (BindableObject bindable) => Colors.Black);

    public double MinimumStrokeThickness
    {
        get
        {
            return (double)GetValue(MinimumStrokeThicknessProperty);
        }
        set
        {
            SetValue(MinimumStrokeThicknessProperty, value);
        }
    }

    public double MaximumStrokeThickness
    {
        get
        {
            return (double)GetValue(MaximumStrokeThicknessProperty);
        }
        set
        {
            SetValue(MaximumStrokeThicknessProperty, value);
        }
    }

    public Color StrokeColor
    {
        get
        {
            return (Color)GetValue(StrokeColorProperty);
        }
        set
        {
            SetValue(StrokeColorProperty, value);
        }
    }

    public event EventHandler<CancelEventArgs>? DrawStarted;
    public event EventHandler<EventArgs>? DrawCompleted;

    public CSignaturePad()
    {
        ThemeElement.InitializeThemeResources(this, "CSignaturePadTheme");
    }

    //private static void Current_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    //{
    //    Page page = (IPlatformApplication.Current?.Application as Application)?.MainPage;
    //    if (e.PropertyName != null && e.PropertyName.Equals("MainPage") && page != null)
    //    {
    //        page.Loaded -= MainPage_Loaded;
    //        page.Loaded += MainPage_Loaded;
    //    }
    //}

    //private static void MainPage_Loaded(object? sender, EventArgs e)
    //{
    //    MainThread.BeginInvokeOnMainThread(delegate
    //    {
    //        ShowLicenseMessage(licenseMessage);
    //    });
    //}

    public ImageSource? ToImageSource()
    {
        return (base.Handler as CSignaturePadHandler)?.ToImageSource();
    }

    internal List<List<float>>? GetPointsCollection()
    {
        return (base.Handler as CSignaturePadHandler)?.GetPointsCollection();
    }

    public void Clear()
    {
        base.Handler?.Invoke("Clear");
    }

    protected override Size MeasureOverride(double widthConstraint, double heightConstraint)
    {
        if (double.IsInfinity(widthConstraint))
        {
            widthConstraint = 350.0;
        }

        if (double.IsInfinity(heightConstraint))
        {
            heightConstraint = 350.0;
        }

        base.DesiredSize = new Size(widthConstraint, heightConstraint);
        return base.DesiredSize;
    }

    bool ICSignaturePad.StartInteraction()
    {
        CancelEventArgs cancelEventArgs = new CancelEventArgs();
        this.DrawStarted?.Invoke(this, cancelEventArgs);
        return !cancelEventArgs.Cancel;
    }

    void ICSignaturePad.EndInteraction()
    {
        this.DrawCompleted?.Invoke(this, EventArgs.Empty);
    }

    private static object MaximumStrokeThicknessDefaultCreator(BindableObject bindable)
    {
        return 5.0;
    }

    private static object MinimumStrokThicknessDefaultCreator(BindableObject bindable)
    {
        return 3.0;
    }

    ResourceDictionary IParentThemeElement.GetThemeDictionary()
    {
        return new CSignaturePadResources();
    }

    void IThemeElement.OnControlThemeChanged(string oldTheme, string newTheme)
    {
    }

    void IThemeElement.OnCommonThemeChanged(string oldTheme, string newTheme)
    {
    }

    public new Brush Background
    {
        get
        {
            return (Color)GetValue(BackgroundProperty);
        }
        set
        {
            base.Background = value;
            SetValue(BackgroundProperty, value);
        }
    }

    //void ICSignaturePad.set_Background(Brush brush)
    //{
    //    base.Background = brush;
    //}
}
