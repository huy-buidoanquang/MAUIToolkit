using Microsoft.Maui.Handlers;

namespace MAUIToolkit.Graphics.Core.Handlers.GraphicsView;

public partial class GraphicsViewHandler : ViewHandler<IGraphicsView, object>
{
    protected override object CreatePlatformView() => throw new NotImplementedException();
}
