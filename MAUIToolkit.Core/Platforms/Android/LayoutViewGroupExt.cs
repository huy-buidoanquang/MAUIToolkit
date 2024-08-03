using Android.Content;
using Android.Graphics;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Views.Accessibility;
using AndroidX.Core.View;
using Java.Interop;
using MAUIToolkit.Core.Controls;
using MAUIToolkit.Core.Platforms;
using Microsoft.Maui.Graphics.Platform;
using Microsoft.Maui.Platform;

namespace MAUIToolkit.Core.Platforms;

internal class LayoutViewGroupExt : LayoutViewGroup, AccessibilityManager.IAccessibilityStateChangeListener, IJavaObject, IDisposable, IJavaPeerable
{
    private int _width;

    private int _height;

    private PlatformCanvas? _canvas;

    private ScalingCanvas? _scalingCanvas;

    private IDrawable? _drawable;

    private float _scale = 1f;

    private Microsoft.Maui.Graphics.Color? _backgroundColor;

    private readonly Context _context;

    private DrawingOrder drawingOrder = DrawingOrder.NoDraw;

    private readonly Android.Graphics.Rect _clipRect = new Android.Graphics.Rect();

    private readonly CView? _mauiView;

    //
    // Summary:
    //     Holds the accessibility delegate instance to handle the accessibility hovering.
    private CustomAccessibilityDelegate? customAccessibilityDelegate;

    internal Func<double, double, Microsoft.Maui.Graphics.Size>? CrossPlatformMeasure { get; set; }

    internal Func<Microsoft.Maui.Graphics.Rect, Microsoft.Maui.Graphics.Size>? CrossPlatformArrange { get; set; }

    public Microsoft.Maui.Graphics.Color? BackgroundColor
    {
        get
        {
            return _backgroundColor;
        }
        set
        {
            _backgroundColor = value;
            Invalidate();
        }
    }

    internal DrawingOrder DrawingOrder
    {
        get
        {
            return drawingOrder;
        }
        set
        {
            drawingOrder = value;
            UpdateDrawable();
            customAccessibilityDelegate?.UpdateChildOrder(drawingOrder != DrawingOrder.BelowContent);
        }
    }

    internal IDrawable? Drawable
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

    //
    // Parameters:
    //   context:
    public LayoutViewGroupExt(Context context)
        : base(context)
    {
        Initialize();
        _context = context;
    }

    //
    // Parameters:
    //   context:
    //
    //   drawable:
    public LayoutViewGroupExt(Context context, Microsoft.Maui.Controls.View drawable)
        : base(context)
    {
        Initialize();
        _context = context;
        Drawable = drawable as IDrawable;
        _mauiView = drawable as CView;
        AddAccessibility(context, drawable);
    }

    public LayoutViewGroupExt(nint javaReference, JniHandleOwnership transfer)
        : base(javaReference, transfer)
    {
        Context context = base.Context;
        ArgumentNullException.ThrowIfNull(context, "context");
        _context = context;
    }

    //
    // Parameters:
    //   context:
    //
    //   attrs:
    public LayoutViewGroupExt(Context context, IAttributeSet attrs)
        : base(context, attrs)
    {
        Initialize();
        _context = context;
    }

    //
    // Parameters:
    //   context:
    //
    //   attrs:
    //
    //   drawable:
    public LayoutViewGroupExt(Context context, IAttributeSet attrs, IDrawable? drawable = null)
        : base(context, attrs)
    {
        _context = context;
        Drawable = drawable;
    }

    //
    // Parameters:
    //   context:
    //
    //   attrs:
    //
    //   defStyleAttr:
    public LayoutViewGroupExt(Context context, IAttributeSet attrs, int defStyleAttr)
        : base(context, attrs, defStyleAttr)
    {
        Initialize();
        _context = context;
    }

    //
    // Parameters:
    //   context:
    //
    //   attrs:
    //
    //   defStyleAttr:
    //
    //   defStyleRes:
    public LayoutViewGroupExt(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes)
        : base(context, attrs, defStyleAttr, defStyleRes)
    {
        Initialize();
        _context = context;
    }

    private void Initialize()
    {
        SetWillNotDraw(willNotDraw: true);
        if (Resources != null && Resources.DisplayMetrics != null)
        {
            _scale = Resources.DisplayMetrics.Density;
        }
    }

    private void UpdateDrawable()
    {
        if (DrawingOrder == DrawingOrder.NoDraw)
        {
            SetWillNotDraw(willNotDraw: true);
            if (_canvas != null)
            {
                _canvas.Dispose();
                _canvas = null;
            }

            if (_scalingCanvas != null)
            {
                _scalingCanvas = null;
            }
        }
        else
        {
            SetWillNotDraw(willNotDraw: false);
            if (_canvas == null)
            {
                _canvas = new PlatformCanvas(_context);
            }

            if (_scalingCanvas == null)
            {
                _scalingCanvas = new ScalingCanvas(_canvas);
            }
        }
    }

    //
    // Summary:
    //     Invalidates the semantics nodes.
    internal void InvalidateSemantics()
    {
        AccessibilityManager accessibilityManager = (AccessibilityManager)(base.Context?.GetSystemService("accessibility"));
        if (accessibilityManager != null && accessibilityManager.IsEnabled)
        {
            customAccessibilityDelegate?.InvalidateSemantics();
        }
    }

    //
    // Parameters:
    //   canvas:
    protected override void DispatchDraw(Canvas? canvas)
    {
        if (canvas != null)
        {
            if (DrawingOrder == DrawingOrder.AboveContent)
            {
                base.DispatchDraw(canvas);
                DrawContent(canvas);
            }
            else
            {
                DrawContent(canvas);
                base.DispatchDraw(canvas);
            }
        }
    }

    //
    // Parameters:
    //   e:
    protected override bool DispatchHoverEvent(MotionEvent? e)
    {
        if (base.Context != null)
        {
            AccessibilityManager accessibilityManager = (AccessibilityManager)base.Context.GetSystemService("accessibility");
            if (accessibilityManager != null && accessibilityManager.IsEnabled && customAccessibilityDelegate != null && customAccessibilityDelegate.DispatchHoverEvent(e))
            {
                return true;
            }
        }

        return base.DispatchHoverEvent(e);
    }

    //
    // Summary:
    //     Triggered when the accessibility status changed.
    //
    // Parameters:
    //   enabled:
    //     The accessibility enabled.
    void AccessibilityManager.IAccessibilityStateChangeListener.OnAccessibilityStateChanged(bool enabled)
    {
        if (enabled && (customAccessibilityDelegate == null || ViewCompat.GetAccessibilityDelegate(this) != customAccessibilityDelegate))
        {
            Microsoft.Maui.Controls.View view = (Microsoft.Maui.Controls.View)_drawable;
            if (view != null)
            {
                customAccessibilityDelegate = new CustomAccessibilityDelegate(this, view, DrawingOrder != DrawingOrder.BelowContent);
                ViewCompat.SetAccessibilityDelegate(this, customAccessibilityDelegate);
            }
        }
    }

    private void AddAccessibility(Context context, Microsoft.Maui.Controls.View view)
    {
        AccessibilityManager accessibilityManager = (AccessibilityManager)context.GetSystemService("accessibility");
        accessibilityManager?.AddAccessibilityStateChangeListener(this);
        if (accessibilityManager != null && accessibilityManager.IsEnabled && (_mauiView == null || !_mauiView.GetType().ToString().Contains("MAUIToolkit.Core.Controls.DataGrid")))
        {
            customAccessibilityDelegate = new CustomAccessibilityDelegate(this, view, DrawingOrder != DrawingOrder.BelowContent);
            ViewCompat.SetAccessibilityDelegate(this, customAccessibilityDelegate);
        }
    }

    private void DrawContent(Canvas? androidCanvas)
    {
        if (_drawable == null)
        {
            return;
        }

        Microsoft.Maui.Graphics.RectF rectF = new Microsoft.Maui.Graphics.RectF(0f, 0f, _width, _height);
        if (_canvas != null)
        {
            _canvas.Canvas = androidCanvas;
            if (_backgroundColor != null)
            {
                _canvas.FillColor = _backgroundColor;
                _canvas.FillRectangle(rectF);
                _canvas.FillColor = Colors.White;
            }

            _scalingCanvas?.ResetState();
            _scalingCanvas?.Scale(_scale, _scale);
            rectF.Height /= _scale;
            rectF.Width /= _scale;
            _drawable.Draw(_scalingCanvas, rectF);
            _canvas.Canvas = null;
        }
    }

    //
    // Parameters:
    //   width:
    //
    //   height:
    //
    //   oldWidth:
    //
    //   oldHeight:
    protected override void OnSizeChanged(int width, int height, int oldWidth, int oldHeight)
    {
        base.OnSizeChanged(width, height, oldWidth, oldHeight);
        _width = width;
        _height = height;
    }

    internal void OnMeasureBase(int widthMeasureSpec, int heightMeasureSpec)
    {
        base.OnMeasure(widthMeasureSpec, heightMeasureSpec);
    }

    //
    // Parameters:
    //   widthMeasureSpec:
    //
    //   heightMeasureSpec:
    protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
    {
        if (CrossPlatformMeasure == null)
        {
            base.OnMeasure(widthMeasureSpec, heightMeasureSpec);
            return;
        }

        double num = widthMeasureSpec.ToDouble(_context);
        double num2 = heightMeasureSpec.ToDouble(_context);
        MeasureSpecMode mode = MeasureSpec.GetMode(widthMeasureSpec);
        MeasureSpecMode mode2 = MeasureSpec.GetMode(heightMeasureSpec);
        Microsoft.Maui.Graphics.Size size = CrossPlatformMeasure(num, num2);
        double dp = ((mode == MeasureSpecMode.Exactly) ? num : size.Width);
        double dp2 = ((mode2 == MeasureSpecMode.Exactly) ? num2 : size.Height);
        float val = _context.ToPixels(dp);
        float val2 = _context.ToPixels(dp2);
        val = Math.Max(MinimumWidth, val);
        val2 = Math.Max(MinimumHeight, val2);
        SetMeasuredDimension((int)val, (int)val2);
    }

    //
    // Parameters:
    //   changed:
    //
    //   l:
    //
    //   t:
    //
    //   r:
    //
    //   b:
    protected override void OnLayout(bool changed, int l, int t, int r, int b)
    {
        if (CrossPlatformArrange != null && _context != null)
        {
            Microsoft.Maui.Graphics.Rect arg = _context.ToCrossPlatformRectInReferenceFrame(l, t, r, b);
            CrossPlatformArrange(arg);
            if (base.ClipsToBounds)
            {
                _clipRect.Right = r - l;
                _clipRect.Bottom = b - t;
                ClipBounds = _clipRect;
            }
            else
            {
                ClipBounds = null;
            }
        }
    }

    //
    // Summary:
    //     Overrides the event interception behavior to support pull-to-refresh functionality.
    //
    //
    // Parameters:
    //   ev:
    //     The MotionEvent to intercept.
    //
    // Returns:
    //     True if the event should be intercepted; otherwise, returns base result.
    //
    // Remarks:
    //     MAUIToolkit.Core.Internals.PullToRefreshExt will not receive touch in touch
    //     event if its pullable content handles touch, hence we need to intercept the touch
    //     event to ensure proper handling.
    public override bool OnInterceptTouchEvent(MotionEvent? ev)
    {
        if (Drawable is PullToRefreshExt pullToRefreshExt && pullToRefreshExt.OnInterceptTouchEvent(ev))
        {
            return true;
        }

        if (Drawable is NavigationDrawerExt navigationDrawerExt && navigationDrawerExt.OnInterceptTouchEvent(ev))
        {
            return true;
        }

        if (Drawable is TabViewExt tabViewExt && tabViewExt.OnInterceptTouchEvent(ev))
        {
            return true;
        }

        return base.OnInterceptTouchEvent(ev);
    }

    //
    // Summary:
    //     This method is called when the view is detached from a window.
    protected override void OnDetachedFromWindow()
    {
        base.OnDetachedFromWindow();
        ((AccessibilityManager)(_context?.GetSystemService("accessibility")))?.RemoveAccessibilityStateChangeListener(this);
    }

    //
    // Summary:
    //     This Method is called upon request layout from it virtual view or one of it descendent
    //     view.
    public override void RequestLayout()
    {
        if (Drawable is INotifyMeasureInvalidated notifyMeasureInvalidated)
        {
            notifyMeasureInvalidated.MeasureInvaidated();
        }

        base.RequestLayout();
    }
}
