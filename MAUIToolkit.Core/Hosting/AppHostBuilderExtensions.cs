using MAUIToolkit.Core.Handlers;
using MAUIToolkit.Core.Internals.Platforms;
using Microsoft.Maui.Controls.Compatibility.Hosting;

namespace MAUIToolkit.Core.Hosting;

public static class AppHostBuilderExtensions
{
    public static MauiAppBuilder ConfigureMAUIToolkitCore(this MauiAppBuilder builder)
    {
        builder.UseMauiCompatibility();
        builder.ConfigureMauiHandlers(delegate (IMauiHandlersCollection handlers)
        {
            handlers.AddHandler(typeof(IDrawableLayout), typeof(CViewHandler));
            handlers.AddHandler(typeof(WindowOverlayContainer), typeof(OverlayContainerHandler));
        });
        builder.ConfigureFonts(delegate (IFontCollection fonts)
        {
            fonts.AddEmbeddedResourceFont(typeof(AppHostBuilderExtensions).Assembly, "Maui Material Assets.ttf", "Maui Material Assets");
            fonts.AddEmbeddedResourceFont(typeof(AppHostBuilderExtensions).Assembly, "Boogaloo.ttf", "Boogaloo");
            fonts.AddEmbeddedResourceFont(typeof(AppHostBuilderExtensions).Assembly, "Handlee.ttf", "Handlee");
            fonts.AddEmbeddedResourceFont(typeof(AppHostBuilderExtensions).Assembly, "Kaushan Script.ttf", "Kaushan Script");
            fonts.AddEmbeddedResourceFont(typeof(AppHostBuilderExtensions).Assembly, "Pinyon Script.ttf", "Pinyon Script");
        });
        return builder;
    }
}
