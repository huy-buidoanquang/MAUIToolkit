using MAUIToolkit.Core.Hosting;
using Microsoft.Extensions.Logging;

namespace MAUIToolkit.Sample
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            //mock comment
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureMAUIToolkitCore()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
