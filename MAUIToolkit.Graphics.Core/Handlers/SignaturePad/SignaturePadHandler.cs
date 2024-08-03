namespace MAUIToolkit.Graphics.Core.SignaturePad;

public partial class SignaturePadHandler
{
    public static IPropertyMapper<ISignaturePad, SignaturePadHandler> SignaturePadMapper =
        new PropertyMapper<ISignaturePad, SignaturePadHandler>(Microsoft.Maui.Handlers.ViewHandler.ViewMapper);

    public static CommandMapper<ISignaturePad, SignaturePadHandler> SignaturePadCommandMapper =
        new CommandMapper<ISignaturePad, SignaturePadHandler>(Microsoft.Maui.Handlers.ViewHandler.ViewCommandMapper)
        {
            //[nameof(ISignaturePad.Invalidate)] = MapInvalidate
        };

    //public SignaturePadHandler()
    //    : base(SignaturePadMapper, SignaturePadCommandMapper)
    //{

    //}

    public SignaturePadHandler(IPropertyMapper? mapper = null, CommandMapper? commandMapper = null)
        : base(mapper ?? SignaturePadMapper, commandMapper ?? SignaturePadCommandMapper)
    {

    }
}
