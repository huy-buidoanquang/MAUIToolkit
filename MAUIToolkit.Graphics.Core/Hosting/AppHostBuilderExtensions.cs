using MAUIToolkit.Graphics.Core.SignaturePad;

namespace MAUIToolkit.Graphics.Core.Hosting;

public static class AppHostBuilderExtensions
{
    public static MauiAppBuilder ConfigureGraphicsControls(this MauiAppBuilder builder, DrawableType drawableType = DrawableType.Material)
    {
        builder.ConfigureMauiHandlers(handlers =>
        {
            handlers.AddGraphicsControlsHandlers(drawableType);
        });

        return builder;
    }

    public static IMauiHandlersCollection AddGraphicsControlsHandlers(this IMauiHandlersCollection handlersCollection, DrawableType drawableType = DrawableType.Material)
    {
        handlersCollection.AddTransient(typeof(ISignaturePad), typeof(SignaturePadHandler));
        handlersCollection.AddTransient(typeof(GraphicsView), typeof(GraphicsViewHandler));

        return handlersCollection;
    }
}