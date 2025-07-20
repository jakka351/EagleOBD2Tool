using Microsoft.Extensions.Logging;
using Eagle.Services;

#if WINDOWS
using Eagle.Platforms.Windows;
#endif

namespace Eagle;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });

#if WINDOWS
        builder.Services.AddSingleton<IBluetoothOBDService, BluetoothOBDServiceWindows>();
#endif

        builder.Services.AddTransient<MainPage>();
        builder.Services.AddTransient<HomePage>();
        builder.Services.AddTransient<OBD2>();

        return builder.Build();
    }
}
