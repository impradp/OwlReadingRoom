using CommunityToolkit.Maui;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OwlReadingRoom.Services.Database;
using OwlReadingRoom.Services.Repository;
using OwlReadingRoom.Utils;
using System.Reflection;

namespace OwlReadingRoom
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .RegisterServices()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("Inter-Regular.ttf", "Inter");
                    fonts.AddFont("Inter-Bold.ttf", "Inter-Bold");
                    fonts.AddFont("Inter-Medium.ttf", "Inter-Medium");
                    fonts.AddFont("Inter-SemiBold.ttf", "Inter-SemiBold");
                });

            Assembly assembly = Assembly.GetExecutingAssembly();

            using (Stream? stream = assembly.GetManifestResourceStream("OwlReadingRoom.appsettings.json"))
            {
                if (stream != null)
                {
                    var config = new ConfigurationBuilder()
                        .AddJsonStream(stream)
                        .Build();
                    builder.Configuration.AddConfiguration(config);
                }
                else
                {
                    CustomAlert.ShowAlert("Warning", "Appsettings.json not found or could not be loaded.", "OK");
                }
            }

            builder.Services.AddTransient<AuthenticationPage>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }

        public static MauiAppBuilder RegisterServices(this MauiAppBuilder builder)
        {
            DatabaseConnectionService databaseConnectionService = new DatabaseConnectionService();

            builder.Services.AddSingleton<IDatabaseConnectionService>(databaseConnectionService);
            builder.Services.AddTransient(typeof(IRepository<>), typeof(Repository<>));

            return builder;
        }
    }
}
