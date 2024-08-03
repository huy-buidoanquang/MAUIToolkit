using MAUIToolkit.Core.Platforms;
using Microsoft.Maui.Handlers;

namespace MAUIToolkit.Core.Handlers;

public partial class CSignaturePadHandler : ViewHandler<ICSignaturePad, NativePlatformSignaturePad>, ICSignaturePadHandler, IViewHandler, IElementHandler
{
    public static PropertyMapper<ICSignaturePad, CSignaturePadHandler> Mapper = new PropertyMapper<ICSignaturePad, CSignaturePadHandler>((PropertyMapper)Microsoft.Maui.Handlers.ViewHandler.ViewMapper)
    {
        ["MaximumStrokeThickness"] = MapMaximumStrokeThickness,
        ["MinimumStrokeThickness"] = MapMinimumStrokeThickness,
        ["StrokeColor"] = MapStrokeColor
    };

    //
    // Summary:
    //     Maps the cross-platform methods to the native methods.
    public static CommandMapper<ICSignaturePad, ICSignaturePadHandler> CommandMapper = new CommandMapper<ICSignaturePad, ICSignaturePadHandler>(Microsoft.Maui.Handlers.ViewHandler.ViewCommandMapper) { ["Clear"] = MapClear };

    ICSignaturePad ICSignaturePadHandler.VirtualView => base.VirtualView;

    NativePlatformSignaturePad ICSignaturePadHandler.PlatformView => base.PlatformView;

    protected override NativePlatformSignaturePad CreatePlatformView()
    {
        return new NativePlatformSignaturePad(base.Context);
    }

    //
    // Summary:
    //     Updates the drawn signature maximum stroke thickness.
    //
    // Parameters:
    //   handler:
    //
    //   virtualView:
    public static void MapMaximumStrokeThickness(CSignaturePadHandler handler, ICSignaturePad virtualView)
    {
        handler.PlatformView.UpdateMaximumStrokeThickness(virtualView);
    }

    //
    // Summary:
    //     Updates the drawn signature minimum stroke thickness.
    //
    // Parameters:
    //   handler:
    //
    //   virtualView:
    public static void MapMinimumStrokeThickness(CSignaturePadHandler handler, ICSignaturePad virtualView)
    {
        handler.PlatformView.UpdateMinimumStrokeThickness(virtualView);
    }

    //
    // Summary:
    //     Updates the drawn signature stroke color.
    //
    // Parameters:
    //   handler:
    //
    //   virtualView:
    public static void MapStrokeColor(CSignaturePadHandler handler, ICSignaturePad virtualView)
    {
        handler.PlatformView.UpdateStrokeColor(virtualView);
    }

    //
    // Summary:
    //     Clears the drawn signature.
    //
    // Parameters:
    //   handler:
    //
    //   virtualView:
    //
    //   arg:
    public static void MapClear(ICSignaturePadHandler handler, ICSignaturePad virtualView, object? arg)
    {
        handler.PlatformView.Clear();
    }

    //
    // Summary:
    //     Initializes a new instance of the SignaturePadHandler class.
    public CSignaturePadHandler()
        : base((IPropertyMapper)Mapper, (CommandMapper?)CommandMapper)
    {
    }

    protected override void ConnectHandler(NativePlatformSignaturePad platformView)
    {
        platformView.Connect(base.VirtualView);
        base.ConnectHandler(platformView);
    }

    protected override void DisconnectHandler(NativePlatformSignaturePad platformView)
    {
        platformView.Disconnect();
        base.DisconnectHandler(platformView);
    }

    //
    // Summary:
    //     Converts the drawn signature as an image.
    internal ImageSource? ToImageSource()
    {
        return base.PlatformView?.ToImageSource();
    }

    internal List<List<float>>? GetPointsCollection()
    {
        return base.PlatformView?.GetPointsCollection();
    }
}
