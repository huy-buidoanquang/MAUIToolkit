namespace MAUIToolkit.Graphics.Core;

public interface IGraphicsHandler : IGraphicsControlInteraction, IViewHandler, IDrawable, IInvalidatable
{
    DrawMapper DrawMapper { get; }

    string[] LayerDrawingOrder();
    void Resized(RectF bounds);
}