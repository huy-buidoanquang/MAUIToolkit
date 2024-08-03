#nullable disable
using System.ComponentModel;

namespace MAUIToolkit.Graphics.Core;

public abstract class GraphicsView : View, IGraphicsView, IGraphicsLayerManager
{
    public GraphicsView()
    {
        GraphicsLayers = new List<string>();
    }

    public virtual List<string> GraphicsLayers { get; }

    public event EventHandler Loaded;
    public event EventHandler Unloaded;

    public event EventHandler TouchDown;
    public event EventHandler TouchMove;
    public event EventHandler TouchUp;

    public event EventHandler<MultiTouchEventArgs> StartHoverInteraction;
    public event EventHandler<MultiTouchEventArgs> MoveHoverInteraction;
    public event EventHandler EndHoverInteraction;
    public event EventHandler<MultiTouchEventArgs> StartInteraction;
    public event EventHandler<MultiTouchEventArgs> DragInteraction;
    public event EventHandler<MultiTouchEventArgs> EndInteraction;
    public event EventHandler CancelInteraction;

    [EditorBrowsable(EditorBrowsableState.Never)]
    public event EventHandler Invalidated;

    public void Invalidate()
    {
        Handler?.Invoke(nameof(IGraphicsView.Invalidate));

        Invalidated?.Invoke(this, EventArgs.Empty);
    }

    public virtual void Draw(ICanvas canvas, RectF dirtyRect)
    {
        foreach (var layer in GraphicsLayers)
            DrawLayer(layer, canvas, dirtyRect);
    }

    public virtual void DrawLayer(string layer, ICanvas canvas, RectF dirtyRect)
    {

    }

    public virtual void Load()
    {
        Loaded?.Invoke(this, EventArgs.Empty);
    }

    public virtual void Unload()
    {
        Unloaded?.Invoke(this, EventArgs.Empty);
    }

    public virtual void OnCancelInteraction() => CancelInteraction?.Invoke(this, EventArgs.Empty);

    void IGraphicsView.OnCancelInteraction() => OnCancelInteraction();

    public virtual void OnDragInteraction(PointF[] points) => DragInteraction?.Invoke(this, new MultiTouchEventArgs(points, true));

    void IGraphicsView.OnDragInteraction(PointF[] points) => OnDragInteraction(points);

    public virtual void OnEndHoverInteraction() => EndHoverInteraction?.Invoke(this, EventArgs.Empty);

    void IGraphicsView.OnEndHoverInteraction() => OnEndHoverInteraction();

    public virtual void OnEndInteraction(PointF[] points, bool isInsideBounds) => EndInteraction?.Invoke(this, new MultiTouchEventArgs(points, isInsideBounds));

    void IGraphicsView.OnEndInteraction(PointF[] points, bool isInsideBounds) => OnEndInteraction(points, isInsideBounds);

    public virtual void OnStartHoverInteraction(PointF[] points) => StartHoverInteraction?.Invoke(this, new MultiTouchEventArgs(points, true));

    void IGraphicsView.OnStartHoverInteraction(PointF[] points) => OnStartHoverInteraction(points);

    public virtual void OnMoveHoverInteraction(PointF[] points) => MoveHoverInteraction?.Invoke(this, new MultiTouchEventArgs(points, true));

    void IGraphicsView.OnMoveHoverInteraction(PointF[] points) => OnMoveHoverInteraction(points);

    public virtual void OnStartInteraction(PointF[] points) => StartInteraction?.Invoke(this, new MultiTouchEventArgs(points, true));

    void IGraphicsView.OnStartInteraction(PointF[] points) => StartInteraction?.Invoke(this, new MultiTouchEventArgs(points, true));

    public virtual void OnTouchDown(Point point) => TouchDown?.Invoke(this, EventArgs.Empty);

    public virtual void OnTouchMove(Point point) => TouchMove?.Invoke(this, EventArgs.Empty);

    public virtual void OnTouchUp(Point point) => TouchUp?.Invoke(this, EventArgs.Empty);

    public int GetLayerIndex(string layerName)
    {
        for (int i = 0; i < GraphicsLayers.Count(); i++)
            if (GraphicsLayers.ElementAt(i) == layerName)
                return i;

        return -1;
    }

    public void AddLayer(string layer)
    {
        GraphicsLayers.Add(layer);

        Invalidate();
    }

    public void AddLayer(int index, string layer)
    {
        GraphicsLayers.Insert(index, layer);

        Invalidate();
    }

    public void RemoveLayer(string layerName)
    {
        for (int i = 0; i < GraphicsLayers.Count(); i++)
        {
            if (GraphicsLayers.ElementAt(i) == layerName)
            {
                GraphicsLayers.RemoveAt(i);

                Invalidate();

                break;
            }
        }
    }

    public void RemoveLayer(int index)
    {
        GraphicsLayers.RemoveAt(index);

        Invalidate();
    }
}

public class TouchEventArgs : EventArgs
{
    public TouchEventArgs()
    {
    }

    public TouchEventArgs(PointF point)
    {
        PointF = point;
    }

    public PointF PointF { get; private set; }
}

public class MultiTouchEventArgs : EventArgs
{
    public MultiTouchEventArgs()
    {

    }

    public MultiTouchEventArgs(PointF[] points, bool isInsideBounds)
    {
        Touches = points;
        IsInsideBounds = isInsideBounds;
    }

    /// <summary>
    /// This is only used for EndInteraction;
    /// </summary>
    public bool IsInsideBounds { get; private set; }

    public PointF[] Touches { get; private set; }
}