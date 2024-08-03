using MAUIToolkit.Core.Internals.Platforms;
using Microsoft.Maui.Handlers;

namespace MAUIToolkit.Core.Handlers;

internal partial class OverlayContainerHandler : ViewHandler<WindowOverlayContainer, object>
{
    internal OverlayContainerHandler(IPropertyMapper mapper, CommandMapper? commandMapper = null)
        : base(mapper, commandMapper)
    {
    }

    protected override object CreatePlatformView()
    {
        throw new NotImplementedException();
    }

    public OverlayContainerHandler()
    : base((IPropertyMapper)ViewHandler.ViewMapper, (CommandMapper?)null)
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
}
