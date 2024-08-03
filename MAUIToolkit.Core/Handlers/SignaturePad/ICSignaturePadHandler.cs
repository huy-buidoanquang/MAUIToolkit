using MAUIToolkit.Core.Platforms;

namespace MAUIToolkit.Core.Handlers;

public interface ICSignaturePadHandler : IViewHandler, IElementHandler
{
    new ICSignaturePad VirtualView { get; }

    new NativePlatformSignaturePad PlatformView { get; }
}
