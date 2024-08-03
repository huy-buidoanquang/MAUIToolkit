using CoreAnimation;
using CoreGraphics;
using Foundation;
using MAUIToolkit.Core.Handlers;
using MAUIToolkit.Core.Internals;
using Microsoft.Maui.Platform;
using UIKit;

namespace MAUIToolkit.Core.Platforms;

public class NativePlatformSignaturePad : UIView
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

    private bool isPressed;

    private CGColor strokeColor = null;

    private CAShapeLayer rootLayer = null;

    private UIBezierPath bezierPath = null;

    private NSError? error;

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

    public NativePlatformSignaturePad()
    {
        ClipsToBounds = true;
    }

    public override void TouchesBegan(NSSet touches, UIEvent? evt)
    {
        if (virtualView == null || !virtualView.IsEnabled)
        {
            UserInteractionEnabled = false;
        }
        else if (touches.AnyObject is UITouch uITouch && virtualView.StartInteraction())
        {
            isPressed = true;
            CGPoint cGPoint = uITouch.LocationInView(this);
            OnInteractionStart((float)cGPoint.X, (float)cGPoint.Y);
        }
    }

    public override void TouchesMoved(NSSet touches, UIEvent? evt)
    {
        if ((virtualView == null || virtualView.IsEnabled) && touches.AnyObject is UITouch uITouch && isPressed)
        {
            CGPoint cGPoint = uITouch.LocationInView(this);
            OnInteractionMove((float)cGPoint.X, (float)cGPoint.Y);
        }
    }

    public override void TouchesEnded(NSSet touches, UIEvent? evt)
    {
        if (virtualView == null || virtualView.IsEnabled)
        {
            if (touches.AnyObject is UITouch uITouch && isPressed)
            {
                virtualView?.EndInteraction();
                CGPoint cGPoint = uITouch.LocationInView(this);
                OnInteractionEnd((float)cGPoint.X, (float)cGPoint.Y);
            }

            isPressed = false;
        }
    }

    internal void Connect(ICSignaturePad mauiView)
    {
        virtualView = mauiView;
        BackgroundColor = UIColor.Clear;
        AddRootLayer();
        bezierPath = new UIBezierPath();
    }

    internal void Disconnect()
    {
        virtualView = null;
        RemoveAllSublayers();
        rootLayer.RemoveFromSuperLayer();
        rootLayer.Dispose();
        bezierPath.Dispose();
        strokeColor.Dispose();
        error?.Dispose();
    }

    internal void UpdateMinimumStrokeThickness(ICSignaturePad mauiView)
    {
        minimumStrokeWidth = (float)mauiView.MinimumStrokeThickness;
        previousStrokeWidth = (minimumStrokeWidth + maximumStrokeWidth) / 2f;
        Redraw();
    }

    internal void UpdateMaximumStrokeThickness(ICSignaturePad mauiView)
    {
        maximumStrokeWidth = (float)mauiView.MaximumStrokeThickness;
        previousStrokeWidth = (minimumStrokeWidth + maximumStrokeWidth) / 2f;
        Redraw();
    }

    internal void UpdateStrokeColor(ICSignaturePad mauiView)
    {
        strokeColor = mauiView.StrokeColor.ToCGColor();
        Redraw();
    }

    internal void UpdateBackground(ICSignaturePad virtualView)
    {
        this.UpdateBackground(virtualView.Background);
    }

    internal ImageSource? ToImageSource()
    {
        UIGraphics.BeginImageContextWithOptions(new CGSize(Frame.Width, Frame.Height), opaque: false, 0);
        CGColor backgroundColor = Layer.BackgroundColor;
        Layer.BackgroundColor = UIColor.Clear.CGColor;
        Layer.RenderInContext(UIGraphics.GetCurrentContext());
        UIImage imageFromCurrentImageContext = UIGraphics.GetImageFromCurrentImageContext();
        UIGraphics.EndImageContext();
        Layer.BackgroundColor = backgroundColor;
        NSData formattedImage = imageFromCurrentImageContext.AsPNG();
        if (formattedImage != null)
        {
            return ImageSource.FromStream(() => formattedImage.AsStream());
        }

        return null;
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

    private void AddBezier(Bezier curve, float startWidth, float endWidth)
    {
        float widthDelta = endWidth - startWidth;
        float num = (float)Math.Ceiling(curve.Length());
        bezierPath.RemoveAllPoints();
        if (bezierPath != null)
        {
            CGPoint point = default(CGPoint);
            for (int i = 0; (float)i < num; i++)
            {
                ComputePointDetails(curve, startWidth, widthDelta, num, i, out var x, out var y, out var width);
                bezierPath.LineWidth = width;
                point.X = x;
                point.Y = y;
                bezierPath.MoveTo(point);
                bezierPath.AddLineTo(point);
            }
        }

        AddRootLayerCopy();
    }

    private void DrawPoint(float x, float y, float width)
    {
        bezierPath.RemoveAllPoints();
        if (bezierPath != null)
        {
            bezierPath.LineWidth = width;
            CGPoint point = default(CGPoint);
            point.X = x;
            point.Y = y;
            bezierPath.MoveTo(point);
            bezierPath.AddLineTo(point);
            AddRootLayerCopy();
        }
    }

    private void AddRootLayerCopy()
    {
        rootLayer.Path = bezierPath.CGPath;
        rootLayer.LineJoin = CAShapeLayer.JoinRound;
        rootLayer.LineCap = CAShapeLayer.CapRound;
        rootLayer.LineWidth = bezierPath.LineWidth;
        rootLayer.Opacity = 1f;
        rootLayer.StrokeColor = strokeColor;
        using NSData nSData = NSKeyedArchiver.GetArchivedData(rootLayer, requiresSecureCoding: false, out error);
        CAShapeLayer cAShapeLayer = null;
        if (nSData != null)
        {
            cAShapeLayer = NSKeyedUnarchiver.GetUnarchivedObject(rootLayer.Class, nSData, out error) as CAShapeLayer;
        }

        if (cAShapeLayer != null)
        {
            Layer.AddSublayer(cAShapeLayer);
        }
    }

    private void AddRootLayer()
    {
        rootLayer = new CAShapeLayer();
        Layer.AddSublayer(rootLayer);
    }

    private void RemoveAllSublayers()
    {
        if (Layer.Sublayers != null)
        {
            CALayer[] sublayers = Layer.Sublayers;
            foreach (CALayer cALayer in sublayers)
            {
                cALayer.RemoveFromSuperLayer();
                cALayer.Dispose();
            }
        }
    }

    private void Invalidate()
    {
        SetNeedsDisplay();
    }

    private void WipeOut()
    {
        RemoveAllSublayers();
        bezierPath.RemoveAllPoints();
        AddRootLayer();
    }
}
