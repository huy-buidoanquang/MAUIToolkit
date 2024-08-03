using MAUIToolkit.Core.Internals.Platforms;
using Microsoft.Maui.Handlers;

namespace MAUIToolkit.Core.Handlers;

internal partial class OverlayContainerHandler : ViewHandler<WindowOverlayContainer, WindowOverlayStack>
{
    public OverlayContainerHandler()
    : base((IPropertyMapper)CViewHandler.ViewMapper, (CommandMapper?)null)
    {
    }

    //
    // Parameters:
    //   mapper:
    //
    //   commandMapper:
    public OverlayContainerHandler(PropertyMapper mapper, CommandMapper commandMapper)
        : base((IPropertyMapper)mapper, commandMapper)
    {
    }

    internal OverlayContainerHandler(IPropertyMapper mapper, CommandMapper? commandMapper = null)
        : base(mapper, commandMapper)
    {
    }

    protected override WindowOverlayStack CreatePlatformView()
    {
        return new WindowOverlayStack();
    }
}
