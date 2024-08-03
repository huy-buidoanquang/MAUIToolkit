using Microsoft.Maui.Handlers;

namespace MAUIToolkit.Core.Handlers;

public partial class CSignaturePadHandler : ViewHandler<ICSignaturePad, object>
{
    public CSignaturePadHandler(IPropertyMapper mapper, CommandMapper? commandMapper = null) : base(mapper, commandMapper)
    {
    }

    protected override object CreatePlatformView() => throw new NotImplementedException();
}
