using MAUIToolkit.Core.Handlers;
using MAUIToolkit.Core.Internals;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI.Composition;
using Microsoft.Graphics.DirectX;
using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Platform;
using Microsoft.UI.Composition;
using Microsoft.UI.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Hosting;
using Microsoft.UI.Xaml.Input;
using System.Numerics;
using Windows.Graphics;
using Windows.Storage.Streams;
using Color = Windows.UI.Color;
using Colors = Microsoft.UI.Colors;
using Size = Windows.Foundation.Size;

namespace MAUIToolkit.Core.Platforms;

public class NativePlatformSignaturePad : UserControl
{
    private readonly List<List<TimedPoint>> drawPointsCache = new List<List<TimedPoint>>();

    private readonly List<TimedPoint> cachePoints = new List<TimedPoint>();

    private readonly List<TimedPoint> memoryPoints = new List<TimedPoint>();

    private readonly float velocityWeight = 0.9f;

    private float previousTouchVelocity = 0f;

    private float previousStrokeWidth;

    private float minimumStrokeWidth;

    private float maximumStrokeWidth;

    private Bezier bezier = new Bezier();

    private TimedPoint firstControlTimedPoint = new TimedPoint();

    private TimedPoint secondControlTimedPoint = new TimedPoint();

    private List<TimedPoint> currentInteractionCyclePoints = null;

    private List<List<float>> pointsCollection = new List<List<float>>();

    private List<float> currentCylcePoints = new List<float>();

    private ICSignaturePad? virtualView;

    private int pointerID = -1;

    private bool isPressed;

    private SpriteVisual? visual;

    private Compositor? compositor;

    private CompositionSurfaceBrush? viewBrush;

    private CompositionDrawingSurface? viewSurface;

    private CompositionGraphicsDevice? graphicsDevice;

    private CanvasRenderTarget? renderTarget;

    private Color strokeColor;

    private Color backgroundColor;

    private float dpi;

    internal List<List<float>> GetPointsCollection()
    {
        return pointsCollection;
    }

    private void OnInteractionStart(float x, float y)
    {
        currentCylcePoints = new List<float>();
        currentCylcePoints.Add(x);
        currentCylcePoints.Add(y);
        memoryPoints.Clear();
        cachePoints.Clear();
        currentInteractionCyclePoints = new List<TimedPoint>();
        drawPointsCache.Add(currentInteractionCyclePoints);
        TimedPoint newPoint = GetNewPoint(x, y);
        currentInteractionCyclePoints.Add(newPoint);
        AddPoint(newPoint);
    }

    private void OnInteractionMove(float x, float y)
    {
        currentCylcePoints.Add(x);
        currentCylcePoints.Add(y);
        TimedPoint newPoint = GetNewPoint(x, y);
        currentInteractionCyclePoints.Add(newPoint);
        AddPoint(newPoint);
    }

    private void OnInteractionEnd(float x, float y)
    {
        currentCylcePoints.Add(x);
        currentCylcePoints.Add(y);
        pointsCollection.Add(currentCylcePoints);
        TimedPoint newPoint = GetNewPoint(x, y);
        currentInteractionCyclePoints.Add(newPoint);
        AddPoint(newPoint);
    }

    private void AddPoint(TimedPoint newPoint)
    {
        memoryPoints.Add(newPoint);
        int count = memoryPoints.Count;
        if (count > 3)
        {
            CalculateCurveControlPoints(memoryPoints[0], memoryPoints[1], memoryPoints[2]);
            TimedPoint timedPoint = secondControlTimedPoint;
            RecyclePoint(firstControlTimedPoint);
            CalculateCurveControlPoints(memoryPoints[1], memoryPoints[2], memoryPoints[3]);
            TimedPoint timedPoint2 = firstControlTimedPoint;
            RecyclePoint(secondControlTimedPoint);
            bezier.Update(memoryPoints[1], timedPoint, timedPoint2, memoryPoints[2]);
            TimedPoint startPoint = bezier.StartPoint;
            TimedPoint endPoint = bezier.EndPoint;
            float num = endPoint.VelocityFrom(startPoint);
            num = (float.IsNaN(num) ? 0f : num);
            num = velocityWeight * num + (1f - velocityWeight) * previousTouchVelocity;
            float strokeWidth = GetStrokeWidth(num);
            AddBezier(bezier, previousStrokeWidth, strokeWidth);
            previousTouchVelocity = num;
            previousStrokeWidth = strokeWidth;
            RecyclePoint(memoryPoints[0]);
            memoryPoints.RemoveAt(0);
            RecyclePoint(timedPoint);
            RecyclePoint(timedPoint2);
        }
        else if (count == 1)
        {
            TimedPoint timedPoint3 = memoryPoints[0];
            memoryPoints.Add(GetNewPoint(timedPoint3.X, timedPoint3.Y));
            DrawPoint(timedPoint3.X, timedPoint3.Y, (maximumStrokeWidth + minimumStrokeWidth) / 2f);
        }
    }

    private float GetStrokeWidth(float velocity)
    {
        return Math.Max(maximumStrokeWidth / (velocity + 1f), minimumStrokeWidth);
    }

    private void RecyclePoint(TimedPoint point)
    {
        cachePoints.Add(point.Copy());
    }

    private void CalculateCurveControlPoints(TimedPoint s1, TimedPoint s2, TimedPoint s3)
    {
        float num = s1.X - s2.X;
        float num2 = s1.Y - s2.Y;
        float num3 = s2.X - s3.X;
        float num4 = s2.Y - s3.Y;
        float num5 = (s1.X + s2.X) / 2f;
        float num6 = (s1.Y + s2.Y) / 2f;
        float num7 = (s2.X + s3.X) / 2f;
        float num8 = (s2.Y + s3.Y) / 2f;
        float num9 = (float)Math.Sqrt(num * num + num2 * num2);
        float num10 = (float)Math.Sqrt(num3 * num3 + num4 * num4);
        float num11 = num5 - num7;
        float num12 = num6 - num8;
        float num13 = num10 / (num9 + num10);
        if (float.IsNaN(num13))
        {
            num13 = 0f;
        }

        float num14 = num7 + num11 * num13;
        float num15 = num8 + num12 * num13;
        float num16 = s2.X - num14;
        float num17 = s2.Y - num15;
        firstControlTimedPoint = GetNewPoint(num5 + num16, num6 + num17);
        secondControlTimedPoint = GetNewPoint(num7 + num16, num8 + num17);
    }

    private TimedPoint GetNewPoint(float x, float y)
    {
        int count = cachePoints.Count;
        TimedPoint timedPoint;
        if (count == 0)
        {
            timedPoint = new TimedPoint();
        }
        else
        {
            int index = count - 1;
            timedPoint = cachePoints[index];
            cachePoints.RemoveAt(index);
        }

        timedPoint.Update(x, y);
        return timedPoint;
    }

    private void ComputePointDetails(Bezier curve, float startWidth, float widthDelta, float drawSteps, int i, out float x, out float y, out float width)
    {
        float num = (float)i / drawSteps;
        float num2 = num * num;
        float num3 = num2 * num;
        float num4 = 1f - num;
        float num5 = num4 * num4;
        float num6 = num5 * num4;
        x = num6 * curve.StartPoint.X;
        x += 3f * num5 * num * curve.FirstControlPoint.X;
        x += 3f * num4 * num2 * curve.SecondControlPoint.X;
        x += num3 * curve.EndPoint.X;
        y = num6 * curve.StartPoint.Y;
        y += 3f * num5 * num * curve.FirstControlPoint.Y;
        y += 3f * num4 * num2 * curve.SecondControlPoint.Y;
        y += num3 * curve.EndPoint.Y;
        width = startWidth + num3 * widthDelta;
    }

    private void Redraw()
    {
        int count = drawPointsCache.Count;
        if (count <= 0)
        {
            return;
        }

        ResetPoints();
        WipeOut();
        for (int i = 0; i < count; i++)
        {
            cachePoints.Clear();
            memoryPoints.Clear();
            List<TimedPoint> list = drawPointsCache[i];
            int count2 = drawPointsCache[i].Count;
            for (int j = 0; j < count2; j++)
            {
                AddPoint(list[j]);
                Invalidate();
            }
        }
    }

    private void ResetPoints()
    {
        cachePoints.Clear();
        memoryPoints.Clear();
        bezier = new Bezier();
        previousTouchVelocity = 0f;
        previousStrokeWidth = (minimumStrokeWidth + maximumStrokeWidth) / 2f;
    }

    private void Reset()
    {
        ResetPoints();
        pointsCollection.Clear();
        currentCylcePoints.Clear();
    }

    //
    // Summary:
    //     Initializes a new instance of the MAUIToolkit.Core.Platforms.PlatformSignaturePad
    //     class.
    public NativePlatformSignaturePad()
    {
        base.Content = new Microsoft.UI.Xaml.Controls.Grid
        {
            Background = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.Transparent)
        };
    }

    internal void Connect(ICSignaturePad mauiView)
    {
        virtualView = mauiView;
        base.SizeChanged += OnSizeChanged;
        base.PointerPressed += OnPointerPressed;
        base.PointerMoved += OnPointerMoved;
        base.PointerReleased += OnPointerReleased;
    }

    internal void Disconnect()
    {
        virtualView = null;
        base.SizeChanged -= OnSizeChanged;
        base.PointerPressed -= OnPointerPressed;
        base.PointerMoved -= OnPointerMoved;
        base.PointerReleased -= OnPointerReleased;
        visual?.Dispose();
        compositor?.Dispose();
        viewBrush?.Dispose();
        viewSurface?.Dispose();
        graphicsDevice?.Dispose();
        renderTarget?.Dispose();
    }

    internal void UpdateMaximumStrokeThickness(ICSignaturePad virtualView)
    {
        maximumStrokeWidth = (float)virtualView.MaximumStrokeThickness;
        previousStrokeWidth = (minimumStrokeWidth + maximumStrokeWidth) / 2f;
        Redraw();
    }

    internal void UpdateMinimumStrokeThickness(ICSignaturePad virtualView)
    {
        minimumStrokeWidth = (float)virtualView.MinimumStrokeThickness;
        previousStrokeWidth = (minimumStrokeWidth + maximumStrokeWidth) / 2f;
        Redraw();
    }

    internal void UpdateStrokeColor(ICSignaturePad virtualView)
    {
        strokeColor = virtualView.StrokeColor.ToWindowsColor();
        Redraw();
    }

    internal void UpdateBackground(ICSignaturePad virtualView)
    {
        Microsoft.UI.Xaml.Media.Brush brush = virtualView.Background?.ToBrush();
        if (brush is Microsoft.UI.Xaml.Media.SolidColorBrush solidColorBrush && (object)solidColorBrush != null)
        {
            _ = solidColorBrush.Color;
            if (true)
            {
                backgroundColor = solidColorBrush.Color;
                goto IL_0050;
            }
        }

        backgroundColor = Colors.Transparent;
        goto IL_0050;
    IL_0050:
        Invalidate();
    }

    internal Microsoft.Maui.Controls.ImageSource? ToImageSource()
    {
        InMemoryRandomAccessStream inMemoryRandomAccessStream = new InMemoryRandomAccessStream();
        renderTarget?.SaveAsync(inMemoryRandomAccessStream, CanvasBitmapFileFormat.Png).GetAwaiter().GetResult();
        return Microsoft.Maui.Controls.ImageSource.FromStream(((IRandomAccessStream)inMemoryRandomAccessStream).AsStream);
    }

    internal void Clear()
    {
        Reset();
        foreach (List<TimedPoint> item in drawPointsCache)
        {
            item.Clear();
        }

        WipeOut();
    }

    private void OnSizeChanged(object sender, SizeChangedEventArgs e)
    {
        if (visual == null)
        {
            CreateRenderTarget(e.NewSize);
            return;
        }

        visual.IsVisible = true;
        visual.Size = base.ActualSize;
        viewSurface?.Resize(new SizeInt32
        {
            Width = (int)base.ActualSize.X,
            Height = (int)base.ActualSize.Y
        });
        viewBrush = compositor?.CreateSurfaceBrush(viewSurface);
        visual.Brush = viewBrush;
        Invalidate();
        dpi = (float)(96.0 * base.XamlRoot.RasterizationScale);
        if (renderTarget != null)
        {
            renderTarget = new CanvasRenderTarget(CanvasDevice.GetSharedDevice(), base.ActualSize.X, base.ActualSize.Y, dpi);
        }

        if (object.Equals(e.PreviousSize, e.NewSize))
        {
            return;
        }

        foreach (List<TimedPoint> item in drawPointsCache)
        {
            memoryPoints.Clear();
            foreach (TimedPoint item2 in item)
            {
                AddPoint(item2);
            }
        }
    }

    private void CreateRenderTarget(Size size)
    {
        if (compositor == null)
        {
            compositor = ElementCompositionPreview.GetElementVisual(this).Compositor;
        }

        if (visual == null)
        {
            visual = compositor.CreateSpriteVisual();
            ElementCompositionPreview.SetElementChildVisual(this, visual);
        }

        visual.IsVisible = true;
        if (graphicsDevice == null)
        {
            graphicsDevice = CanvasComposition.CreateCompositionGraphicsDevice(compositor, CanvasDevice.GetSharedDevice());
        }

        if (viewSurface == null)
        {
            viewSurface = graphicsDevice.CreateDrawingSurface(size, DirectXPixelFormat.B8G8R8A8UIntNormalized, DirectXAlphaMode.Premultiplied);
        }

        visual.Size = new Vector2((float)size.Width, (float)size.Height);
        viewBrush = compositor.CreateSurfaceBrush(viewSurface);
        visual.Brush = viewBrush;
        dpi = (float)(96.0 * base.XamlRoot.RasterizationScale);
        renderTarget = new CanvasRenderTarget(CanvasDevice.GetSharedDevice(), (float)size.Width, (float)size.Height, dpi);
        Invalidate();
    }

    private void OnPointerPressed(object sender, PointerRoutedEventArgs e)
    {
        if (virtualView != null && virtualView.StartInteraction() && pointerID == -1)
        {
            isPressed = true;
            pointerID = (int)e.Pointer.PointerId;
            CapturePointer(e.Pointer);
            memoryPoints.Clear();
            PointerPoint currentPoint = e.GetCurrentPoint(this);
            OnInteractionStart((float)currentPoint.Position.X, (float)currentPoint.Position.Y);
        }
    }

    private void OnPointerMoved(object sender, PointerRoutedEventArgs e)
    {
        if (isPressed && e.Pointer.PointerId == pointerID)
        {
            PointerPoint currentPoint = e.GetCurrentPoint(this);
            OnInteractionMove((float)currentPoint.Position.X, (float)currentPoint.Position.Y);
        }
    }

    private void OnPointerReleased(object sender, PointerRoutedEventArgs e)
    {
        if (isPressed && e.Pointer.PointerId == pointerID)
        {
            pointerID = -1;
            isPressed = false;
            PointerPoint currentPoint = e.GetCurrentPoint(this);
            OnInteractionEnd((float)currentPoint.Position.X, (float)currentPoint.Position.Y);
            virtualView?.EndInteraction();
        }
    }

    private void AddBezier(Bezier curve, float startWidth, float endWidth)
    {
        if (!(renderTarget != null))
        {
            return;
        }

        float widthDelta = endWidth - startWidth;
        float num = (float)Math.Ceiling(curve.Length());
        using (CanvasDrawingSession canvasDrawingSession = renderTarget.CreateDrawingSession())
        {
            canvasDrawingSession.Antialiasing = CanvasAntialiasing.Antialiased;
            for (int i = 0; (float)i < num; i++)
            {
                ComputePointDetails(curve, startWidth, widthDelta, num, i, out var x, out var y, out var width);
                canvasDrawingSession.FillCircle(x, y, width, strokeColor);
            }
        }

        Invalidate();
    }

    private void DrawPoint(float x, float y, float width)
    {
        if (renderTarget != null)
        {
            using (CanvasDrawingSession canvasDrawingSession = renderTarget.CreateDrawingSession())
            {
                canvasDrawingSession.Antialiasing = CanvasAntialiasing.Antialiased;
                canvasDrawingSession.FillCircle(x, y, width, strokeColor);
            }

            Invalidate();
        }
    }

    private void Invalidate()
    {
        if (viewSurface != null)
        {
            using (CanvasDrawingSession canvasDrawingSession = CanvasComposition.CreateDrawingSession(viewSurface))
            {
                canvasDrawingSession.Antialiasing = CanvasAntialiasing.Antialiased;
                canvasDrawingSession.Clear(backgroundColor);
                canvasDrawingSession.DrawImage(renderTarget);
            }
        }
    }

    private void WipeOut()
    {
        if (renderTarget != null)
        {
            using (CanvasDrawingSession canvasDrawingSession = renderTarget.CreateDrawingSession())
            {
                canvasDrawingSession.Antialiasing = CanvasAntialiasing.Antialiased;
                canvasDrawingSession.Clear(Colors.Transparent);
                Invalidate();
            }
        }
    }
}
