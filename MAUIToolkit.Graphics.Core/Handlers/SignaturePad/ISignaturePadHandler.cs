namespace MAUIToolkit.Graphics.Core.SignaturePad;

public interface ISignaturePadHandler : IViewHandler, IElementHandler
{
    new ISignaturePad VirtualView { get; }

    new NativeSignaturePad PlatformView { get; }
}
