namespace MAUIToolkit.Graphics.Core
{
	public interface IMixedGraphicsHandler : IGraphicsControlInteraction, IViewHandler, IDrawable, IInvalidatable
	{
		DrawMapper DrawMapper { get; }

		string[] LayerDrawingOrder();
		void Resized(RectF bounds);
	}
}