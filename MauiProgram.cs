using CommunityToolkit.Maui;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OwlReadingRoom.Services;
using OwlReadingRoom.Services.Database;
using OwlReadingRoom.Services.Email;
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
            builder.Services.AddSingleton<IDatabaseConnectionService, DatabaseConnectionService>();
            builder.Services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
            // View Models
            builder.Services.AddTransient<MainView>();
            builder.Services.AddTransient<NewCustomer>();

            //services
            builder.Services.AddSingleton<IPackageService, PackageService>();
            builder.Services.AddSingleton<IBookingService, BookingService>();
            builder.Services.AddSingleton<ICustomerService, CustomerService>();
            builder.Services.AddSingleton<IUserService, UserService>();
            builder.Services.AddSingleton<IEmailService, EmailService>();
            builder.Services.AddSingleton(sp =>
            {
                var config = sp.GetRequiredService<IConfiguration>();
                return Auth0Handler.GetAuth0Client(config);
            });
            builder.Services.AddSingleton(sp =>
            {
                var config = sp.GetRequiredService<IConfiguration>();
                return SmtpHandler.GetSmtpSettings(config);
            });
            return builder;
        }
    }
}
