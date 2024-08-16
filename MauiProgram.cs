using CommunityToolkit.Maui;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OwlReadingRoom.Services.Database;
using OwlReadingRoom.Services.Repository;
using OwlReadingRoom.Utils;
using OwlReadingRoom.Views;
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
            builder.Services.AddSingleton<IDatabaseConnectionService,DatabaseConnectionService>();
            builder.Services.AddTransient(typeof(IRepository<>), typeof(Repository<>));

            // More services registered here.
            builder.Services.AddTransient<NewCustomer>();
            builder.Services.AddTransient<MainView>();
            builder.Services.AddTransient<AuthenticationPage>();

            builder.Services.AddSingleton(sp =>
            {
                var config = sp.GetRequiredService<IConfiguration>();
                return Auth0Handler.GetAuth0Client(config);
            });

            return builder;
        }
    }
}
