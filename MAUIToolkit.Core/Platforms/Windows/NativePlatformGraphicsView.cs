using MAUIToolkit.Core.Controls;
using MAUIToolkit.Core.Semantics;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Microsoft.Maui.Graphics.Win2D;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Automation.Peers;
using Microsoft.UI.Xaml.Controls;

namespace MAUIToolkit.Core.Platforms;

internal class NativePlatformGraphicsView : UserControl
{
    private CanvasControl? _canvasControl;

    private readonly W2DCanvas _canvas;

    private IDrawable _drawable;

    private RectF _dirty;

    internal CustomAutomationPeer? SemanticsAutomationPeer;

    private CView mauiView;

    public IDrawable Drawable
    {
        get
        {
            return _drawable;
        }
        set
        {
            _drawable = value;
            Invalidate();
        }
    }

    internal NativePlatformGraphicsView(CView mauiView)
    {
        _canvas = new W2DCanvas();
        this.mauiView = mauiView;
        base.Loaded += UserControl_Loaded;
        base.Unloaded += UserControl_Unloaded;
    }

    internal void Invalidate()
    {
        _canvasControl?.Invalidate();
    }

    private void UserControl_Loaded(object sender, RoutedEventArgs e)
    {
        _canvasControl = new CanvasControl();
        _canvasControl.Draw += OnDraw;
        base.Content = _canvasControl;
    }

    private void UserControl_Unloaded(object sender, RoutedEventArgs e)
    {
        if (_canvasControl != null && !_canvasControl.IsLoaded)
        {
            _canvasControl.RemoveFromVisualTree();
            _canvasControl = null;
        }

        SemanticsAutomationPeer = null;
    }

    private void OnDraw(CanvasControl sender, CanvasDrawEventArgs args)
    {
        if (_drawable != null)
        {
            _dirty.X = 0f;
            _dirty.Y = 0f;
            _dirty.Width = (float)sender.ActualWidth;
            _dirty.Height = (float)sender.ActualHeight;
            _canvas.Session = args.DrawingSession;
            _canvas.CanvasSize = new global::Windows.Foundation.Size(_dirty.Width, _dirty.Height);
            _drawable.Draw(_canvas, _dirty);
        }
    }

    protected override AutomationPeer OnCreateAutomationPeer()
    {
        SemanticsAutomationPeer = new CustomAutomationPeer(this, mauiView);
        return SemanticsAutomationPeer;
    }
}
