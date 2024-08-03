namespace MAUIToolkit.Core.Handlers;

public partial class CSignaturePadHandler
{
    public static IPropertyMapper<ICSignaturePad, CSignaturePadHandler> SignaturePadMapper =
        new PropertyMapper<ICSignaturePad, CSignaturePadHandler>(ViewMapper);

    public static CommandMapper<ICSignaturePad, CSignaturePadHandler> SignaturePadCommandMapper =
        new CommandMapper<ICSignaturePad, CSignaturePadHandler>(ViewCommandMapper)
        {
        };
    public CSignaturePadHandler(IPropertyMapper? mapper = null, CommandMapper? commandMapper = null)
        : base(mapper ?? SignaturePadMapper, commandMapper ?? SignaturePadCommandMapper)
    {

    }
}
