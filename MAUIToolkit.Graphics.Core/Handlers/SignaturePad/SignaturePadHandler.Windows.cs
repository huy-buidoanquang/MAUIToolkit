using Microsoft.Maui.Handlers;
using ViewHandler = Microsoft.Maui.Handlers.ViewHandler;

namespace MAUIToolkit.Graphics.Core.SignaturePad;

public partial class SignaturePadHandler : ViewHandler<ISignaturePad, NativeSignaturePad>, ISignaturePadHandler, IViewHandler, IElementHandler
{
    public static PropertyMapper<ISignaturePad, SignaturePadHandler> Mapper = new PropertyMapper<ISignaturePad, SignaturePadHandler>((PropertyMapper)Microsoft.Maui.Handlers.ViewHandler.ViewMapper)
    {
        ["MaximumStrokeThickness"] = MapMaximumStrokeThickness,
        ["MinimumStrokeThickness"] = MapMinimumStrokeThickness,
        ["StrokeColor"] = MapStrokeColor
    };

    //
    // Summary:
    //     Maps the cross-platform methods to the native methods.
    public static CommandMapper<ISignaturePad, ISignaturePadHandler> CommandMapper = new CommandMapper<ISignaturePad, ISignaturePadHandler>(Microsoft.Maui.Handlers.ViewHandler.ViewCommandMapper) { ["Clear"] = MapClear };

    ISignaturePad ISignaturePadHandler.VirtualView => base.VirtualView;

    NativeSignaturePad ISignaturePadHandler.PlatformView => base.PlatformView;

    //
    // Summary:
    //     Initializes a new instance of the SignaturePadHandler class.
    public SignaturePadHandler()
        : base((IPropertyMapper)Mapper, (CommandMapper?)CommandMapper)
    {
    }

    protected override void ConnectHandler(NativeSignaturePad platformView)
    {
        platformView.Connect(base.VirtualView);
        base.ConnectHandler(platformView);
    }

    protected override void DisconnectHandler(NativeSignaturePad platformView)
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

    protected override NativeSignaturePad CreatePlatformView()
    {
        return new NativeSignaturePad();
    }

    //
    // Summary:
    //     Updates the drawn signature maximum stroke thickness.
    //
    // Parameters:
    //   handler:
    //
    //   virtualView:
    public static void MapMaximumStrokeThickness(SignaturePadHandler handler, ISignaturePad virtualView)
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
    public static void MapMinimumStrokeThickness(SignaturePadHandler handler, ISignaturePad virtualView)
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
    public static void MapStrokeColor(SignaturePadHandler handler, ISignaturePad virtualView)
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
    public static void MapClear(ISignaturePadHandler handler, ISignaturePad virtualView, object? arg)
    {
        handler.PlatformView.Clear();
    }

    public override void UpdateValue(string property)
    {
        base.UpdateValue(property);
        if (property == "Background")
        {
            base.PlatformView.UpdateBackground(base.VirtualView);
        }
    }
}
