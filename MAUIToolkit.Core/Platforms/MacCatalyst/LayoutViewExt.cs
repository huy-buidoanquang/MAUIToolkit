using CoreGraphics;
using Foundation;
using MAUIToolkit.Core.Controls;
using MAUIToolkit.Core.Platforms;
using MAUIToolkit.Core.Primitives;
using MAUIToolkit.Core.Semantics;
using Microsoft.Maui.Graphics.Platform;
using Microsoft.Maui.Platform;
using ObjCRuntime;
using System.Runtime.InteropServices;
using UIKit;

namespace MAUIToolkit.Core.Platform;

public class LayoutViewExt : LayoutView, IUIAccessibilityContainer, INativeObject, IDisposable
{
    private IGraphicsRenderer? _renderer;

    private CGColorSpace? _colorSpace;

    private IDrawable? _drawable;

    private CGRect _lastBounds;

    private DrawingOrder drawingOrder = DrawingOrder.NoDraw;

    private NativePlatformGraphicsView? nativeGraphicsView;

    private readonly View? mauiView;

    //
    // Summary:
    //     Used to hold the previous bounds value. The value used to update the semantics
    //     while the size changed.
    private CGRect? availableBounds;

    //
    // Summary:
    //     Holds the accessibility notification objects.
    private List<NSObject>? notifications;

    //
    // Summary:
    //     Returns a boolean value indicating whether this object can become the first responder.
    public override bool CanBecomeFirstResponder => mauiView is IKeyboardListener && (mauiView as IKeyboardListener).CanBecomeFirstResponder;

    internal Func<double, double, Size>? CrossPlatformMeasure { get; set; }

    internal Func<Rect, Size>? CrossPlatformArrange { get; set; }

    internal DrawingOrder DrawingOrder
    {
        get
        {
            return drawingOrder;
        }
        set
        {
            drawingOrder = value;
            InitializeNativeGraphicsView();
            SetNeedsDisplay();
        }
    }

    internal IGraphicsRenderer? Renderer
    {
        get
        {
            return _renderer;
        }
        set
        {
            UpdateRenderer(value);
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
            if (_renderer != null)
            {
                _renderer.Drawable = _drawable;
                _renderer.Invalidate();
            }
        }
    }

    public override CGRect Bounds
    {
        get
        {
            return base.Bounds;
        }
        set
        {
            CGRect lastBounds = value;
            if (_lastBounds.Width != lastBounds.Width || _lastBounds.Height != lastBounds.Height)
            {
                base.Bounds = value;
                _renderer?.SizeChanged((float)lastBounds.Width, (float)lastBounds.Height);
                _renderer?.Invalidate();
                UpdateGraphicsViewBounds();
                InvalidateSemantics();
                _lastBounds = lastBounds;
                SetNeedsDisplay();
            }
        }
    }

    protected virtual CGSize PatternPhase
    {
        get
        {
            NFloat x = Frame.X;
            NFloat y = Frame.Y;
            return new CGSize(x, y);
        }
    }

    //
    // Summary:
    //     This event occurs while new touches occurred in a view.
    internal event EventHandler<UIViewTouchEventArgs>? Pressed;

    //
    // Summary:
    //     This event occurs when one or more touches associated with an event changed.
    internal event EventHandler<UIViewTouchEventArgs>? Moved;

    //
    // Summary:
    //     This event occurs when one or more fingers are raised from a view
    internal event EventHandler<UIViewTouchEventArgs>? Released;

    //
    // Parameters:
    //   drawable:
    //
    //   renderer:
    public LayoutViewExt(IDrawable? drawable = null, IGraphicsRenderer? renderer = null)
    {
        Drawable = drawable;
        mauiView = (View)drawable;
        Renderer = renderer;
        BackgroundColor = UIColor.Clear;
        Opaque = false;
        notifications = new List<NSObject>
        {
            NSNotificationCenter.DefaultCenter.AddObserver(UIView.VoiceOverStatusDidChangeNotification, OnObserveNotification),
            NSNotificationCenter.DefaultCenter.AddObserver(UIView.SwitchControlStatusDidChangeNotification, OnObserveNotification),
            NSNotificationCenter.DefaultCenter.AddObserver(UIView.InvertColorsStatusDidChangeNotification, OnObserveNotification),
            NSNotificationCenter.DefaultCenter.AddObserver(UIView.ReduceMotionStatusDidChangeNotification, OnObserveNotification),
            NSNotificationCenter.DefaultCenter.AddObserver(UIView.BoldTextStatusDidChangeNotification, OnObserveNotification),
            NSNotificationCenter.DefaultCenter.AddObserver(UIView.DarkerSystemColorsStatusDidChangeNotification, OnObserveNotification),
            NSNotificationCenter.DefaultCenter.AddObserver(UIView.GuidedAccessStatusDidChangeNotification, OnObserveNotification),
            NSNotificationCenter.DefaultCenter.AddObserver(UIView.SpeakScreenStatusDidChangeNotification, OnObserveNotification)
        };
    }

    public LayoutViewExt(nint aPtr)
    {
        BackgroundColor = UIColor.Clear;
    }

    private void UpdateRenderer(IGraphicsRenderer? graphicsRenderer = null)
    {
        if (_renderer != null)
        {
            _renderer.Drawable = null;
            _renderer.GraphicsView = null;
            _renderer.Dispose();
            _renderer = null;
        }

        if (DrawingOrder == DrawingOrder.BelowContent)
        {
            _renderer = graphicsRenderer ?? new DirectRenderer();
            _renderer.GraphicsView = new PlatformGraphicsView();
            _renderer.Drawable = Drawable;
            _renderer.SizeChanged((float)Bounds.Width, (float)Bounds.Height);
        }
    }

    internal void InvalidateDrawable()
    {
        _renderer?.Invalidate();
        nativeGraphicsView?.InvalidateDrawable();
        SetNeedsDisplay();
    }

    //
    // Parameters:
    //   x:
    //
    //   y:
    //
    //   w:
    //
    //   h:
    public void InvalidateDrawable(float x, float y, float w, float h)
    {
        _renderer?.Invalidate(x, y, w, h);
    }

    internal void InitializeNativeGraphicsView()
    {
        UpdateRenderer();
        if (DrawingOrder == DrawingOrder.AboveContent || DrawingOrder == DrawingOrder.AboveContentWithTouch)
        {
            if (nativeGraphicsView == null)
            {
                nativeGraphicsView = new NativePlatformGraphicsView((CView)mauiView)
                {
                    BackgroundColor = UIColor.Clear,
                    Drawable = Drawable
                };
                if (DrawingOrder == DrawingOrder.AboveContent)
                {
                    nativeGraphicsView.UserInteractionEnabled = false;
                }
            }

            Add(nativeGraphicsView);
        }
        else if (nativeGraphicsView != null)
        {
            nativeGraphicsView.RemoveFromSuperview();
            SetNeedsDisplay();
        }

        InvalidateSemantics();
    }

    public override void LayoutSubviews()
    {
        base.LayoutSubviews();
        Rect arg = AdjustForSafeArea(Bounds).ToRectangle();
        CrossPlatformMeasure?.Invoke(arg.Width, arg.Height);
        CrossPlatformArrange?.Invoke(arg);
        UpdateGraphicsViewBounds();
    }

    //
    // Parameters:
    //   size:
    public override CGSize SizeThatFits(CGSize size)
    {
        if (CrossPlatformMeasure == null)
        {
            return base.SizeThatFits(size);
        }

        NFloat width = size.Width;
        NFloat height = size.Height;
        Size size2 = CrossPlatformMeasure(width, height);
        return size2.ToCGSize();
    }

    //
    // Summary:
    //     This Method is called upon SetNeedsLayout from its virtual view or one of it
    //     descendent view.
    public override void SetNeedsLayout()
    {
        if (Drawable is INotifyMeasureInvalidated notifyMeasureInvalidated)
        {
            notifyMeasureInvalidated.MeasureInvaidated();
        }

        base.SetNeedsLayout();
    }

    //
    // Parameters:
    //   dirtyRect:
    public override void Draw(CGRect dirtyRect)
    {
        if (DrawingOrder == DrawingOrder.BelowContent)
        {
            base.Draw(dirtyRect);
            CGContext currentContext = UIGraphics.GetCurrentContext();
            if (_drawable == null)
            {
                return;
            }

            if (_colorSpace == null)
            {
                _colorSpace = CGColorSpace.CreateDeviceRGB();
            }

            currentContext.SetFillColorSpace(_colorSpace);
            currentContext.SetStrokeColorSpace(_colorSpace);
            currentContext.SetPatternPhase(PatternPhase);
            _renderer?.Draw(currentContext, dirtyRect.AsRectangleF());
        }

        if (this.GetAccessibilityElements() == null || availableBounds != dirtyRect)
        {
            CreateSemantics();
            availableBounds = dirtyRect;
        }
    }

    //
    // Summary:
    //     Raised when touches began.
    //
    // Parameters:
    //   touches:
    //
    //   evt:
    public override void TouchesBegan(NSSet touches, UIEvent? evt)
    {
        base.TouchesBegan(touches, evt);
        if (touches.AnyObject is UITouch uITouch)
        {
            CGPoint cGPoint = uITouch.LocationInView(this);
            this.Pressed?.Invoke(this, new UIViewTouchEventArgs
            {
                Point = new Point((float)cGPoint.X, (float)cGPoint.Y)
            });
        }
    }

    //
    // Summary:
    //     Raised when touches moved.
    //
    // Parameters:
    //   touches:
    //
    //   evt:
    public override void TouchesMoved(NSSet touches, UIEvent? evt)
    {
        base.TouchesMoved(touches, evt);
        if (touches.AnyObject is UITouch uITouch)
        {
            CGPoint cGPoint = uITouch.LocationInView(this);
            this.Moved?.Invoke(this, new UIViewTouchEventArgs
            {
                Point = new Point((float)cGPoint.X, (float)cGPoint.Y)
            });
        }
    }

    //
    // Summary:
    //     Raised when touches ended.
    //
    // Parameters:
    //   touches:
    //
    //   evt:
    public override void TouchesEnded(NSSet touches, UIEvent? evt)
    {
        base.TouchesEnded(touches, evt);
        if (touches.AnyObject is UITouch uITouch)
        {
            CGPoint cGPoint = uITouch.LocationInView(this);
            this.Released?.Invoke(this, new UIViewTouchEventArgs
            {
                Point = new Point((float)cGPoint.X, (float)cGPoint.Y)
            });
        }
    }

    //
    // Summary:
    //     Raised when a button is pressed.
    //
    // Parameters:
    //   presses:
    //     A set of UIKit.UIPress instances that represent the new presses that occurred.
    //
    //
    //   evt:
    //     The event to which the presses belong.
    public override void PressesBegan(NSSet<UIPress> presses, UIPressesEvent evt)
    {
        //if (mauiView != null && !mauiView.HandleKeyPress(presses, evt))
        if (mauiView != null)
        {
            base.PressesBegan(presses, evt);
        }
    }

    //
    // Summary:
    //     Raised when a button is released.
    //
    // Parameters:
    //   presses:
    //     A set of UIKit.UIPress instances that represent the buttons that the user is
    //     no longer pressing.
    //
    //   evt:
    //     The event to which the presses belong.
    public override void PressesEnded(NSSet<UIPress> presses, UIPressesEvent evt)
    {
        //if (mauiView != null && !mauiView.HandleKeyRelease(presses, evt))
        if (mauiView != null)
        {
            base.PressesEnded(presses, evt);
        }
    }

    //
    // Parameters:
    //   disposing:
    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        if (!disposing)
        {
            return;
        }

        if (nativeGraphicsView != null)
        {
            nativeGraphicsView.SetAccessibilityElements(null);
            nativeGraphicsView.Dispose();
            nativeGraphicsView = null;
        }

        if (notifications != null)
        {
            for (int i = 0; i < notifications.Count; i++)
            {
                NSNotificationCenter.DefaultCenter.RemoveObserver(notifications[i]);
            }

            notifications = null;
        }

        this.SetAccessibilityElements(null);
    }

    private void UpdateGraphicsViewBounds()
    {
        if (nativeGraphicsView != null)
        {
            if (nativeGraphicsView.Bounds != Bounds)
            {
                nativeGraphicsView.Frame = Bounds;
            }

            nativeGraphicsView.InvalidateDrawable();
        }
    }

    private void OnObserveNotification(NSNotification notification)
    {
        InvalidateSemantics();
    }

    //
    // Summary:
    //     Invalidates the semantics nodes.
    internal void InvalidateSemantics()
    {
        CreateSemantics();
        nativeGraphicsView?.InvalidateSemantics();
    }

    //
    // Summary:
    //     Create the semantics mode for the view.
    private void CreateSemantics()
    {
        if (!UIAccessibility.IsVoiceOverRunning && !UIAccessibility.IsSwitchControlRunning && !UIAccessibility.IsSpeakScreenEnabled)
        {
            this.SetAccessibilityElements(null);
            return;
        }

        List<object> list = new List<object>();
        if (DrawingOrder == DrawingOrder.BelowContent)
        {
            List<SemanticsNode> list2 = null;
            if (mauiView is ISemanticsProvider)
            {
                list2 = ((ISemanticsProvider)mauiView).GetSemanticsNodes(Bounds.Width, Bounds.Height);
            }

            if (list2 != null)
            {
                CGRect accessibilityFrame = AccessibilityFrame;
                for (int i = 0; i < list2.Count; i++)
                {
                    SemanticsNode semanticsNode = list2[i];
                    CustomAccessibilityElement item = new CustomAccessibilityElement(this)
                    {
                        AccessibilityHint = semanticsNode.Text,
                        AccessibilityLabel = semanticsNode.Text,
                        AccessibilityIdentifier = semanticsNode.Id.ToString(),
                        IsAccessibilityElement = true,
                        AccessibilityTraits = (ulong)(semanticsNode.IsTouchEnabled ? 1 : 0),
                        AccessibilityFrame = UIAccessibility.ConvertFrameToScreenCoordinates(new CGRect(semanticsNode.Bounds.Left, semanticsNode.Bounds.Top, semanticsNode.Bounds.Width, semanticsNode.Bounds.Height), this),
                        Bounds = new CGRect(semanticsNode.Bounds.Left, semanticsNode.Bounds.Top, semanticsNode.Bounds.Width, semanticsNode.Bounds.Height),
                        Parent = this,
                        ParentBounds = accessibilityFrame
                    };
                    list.Add(item);
                }
            }
        }

        for (int j = 0; j < Subviews.Length; j++)
        {
            UIView item2 = Subviews[j];
            list.Add(item2);
        }

        this.SetAccessibilityElements(NSArray.FromObjects(list.ToArray()));
    }
}
