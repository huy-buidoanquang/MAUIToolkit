namespace MAUIToolkit.Graphics.Core.SignaturePad;

public partial class SignaturePadHandler : ViewHandler<ISignaturePad, object>
{
    public SignaturePadHandler(IPropertyMapper mapper, CommandMapper? commandMapper = null) : base(mapper, commandMapper)
    {
    }

    protected override object CreatePlatformView() => throw new NotImplementedException();
}
