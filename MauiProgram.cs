using Microsoft.Extensions.Logging;
using QanooniRishta.Services;

namespace QanooniRishta;

public static class MauiProgram
{
    public static IServiceProvider ServiceProvider { get; private set; }

    public static MauiApp CreateMauiApp()
    {
        SQLitePCL.Batteries_V2.Init(); // Required for SQLite

        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });

        // Register services
        builder.Services.AddSingleton<SqlLiteDatabaseService>();

        builder.Services.AddMauiBlazorWebView();

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif

        // Build once, assign and return
        var app = builder.Build();
        ServiceProvider = app.Services;
        return app;
    }
}
