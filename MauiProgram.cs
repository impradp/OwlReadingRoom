using CommunityToolkit.Maui;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OwlReadingRoom.Model;
using OwlReadingRoom.Services.Database;
using OwlReadingRoom.Services.Repository;
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
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
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
                    // TODO: Handle the case where the stream is null
                    Console.WriteLine("Warning: appsettings.json not found or could not be loaded.");
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
            var databaseConnectionService = new DatabaseConnectionService();

            builder.Services.AddSingleton<IDatabaseConnectionService>(databaseConnectionService);
            builder.Services.AddTransient(typeof(IRepository<>), typeof(Repository<>));

            // More services registered here.

            return builder;
        }
    }
}
