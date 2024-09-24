using CommunityToolkit.Maui;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OwlReadingRoom.Proxy;
using OwlReadingRoom.Services;
using OwlReadingRoom.Services.Database;
using OwlReadingRoom.Services.Email;
using OwlReadingRoom.Services.Repository;
using OwlReadingRoom.Services.Resources;
using OwlReadingRoom.Services.Transactions;
using OwlReadingRoom.Utils;
using OwlReadingRoom.ViewModels;
using OwlReadingRoom.Views;
using OwlReadingRoom.Views.Customer;
using OwlReadingRoom.Views.Resources.Package;
using OwlReadingRoom.Views.Resources.Rooms;
using System.Reflection;
using OwlReadingRoom.Views.Resources.Rooms.Plans;
using OwlReadingRoom.Services.Booking;


#if WINDOWS
using OwlReadingRoom.Platforms.Windows.Services;
#endif

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
                    fonts.AddFont("Inika-Bold.ttf", "Inika-Bold");
                    fonts.AddFont("Inika-Regular.ttf", "Inika");
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
            builder.Services.AddTransient<RoomListView>();
            builder.Services.AddTransient<NewRoom>();
            builder.Services.AddTransient<PackageListView>();
            builder.Services.AddTransient<CustomerListView>();
            builder.Services.AddTransient<NewPackage>();
            builder.Services.AddTransient<ACRoomPlan>();
            builder.Services.AddTransient<NonACRoomPlan>();
            builder.Services.AddTransient<Func<RoomListViewModel, UpdateRoom>>(sp =>
            {
                var resourceService = sp.GetRequiredService<IPhysicalResourceService>();
                return room => new UpdateRoom(room, resourceService);
            });
            builder.Services.AddTransient<Func<RoomListViewModel, DeskLayout>>(sp =>
            {
                var resourceService = sp.GetRequiredService<IPhysicalResourceService>();
                var serviceProvider = sp.GetRequiredService<IServiceProvider>();
                return room => new DeskLayout(room, resourceService, serviceProvider);
            });

            builder.Services.AddTransient<Func<RoomListViewModel, DeskSelectView>>(sp =>
            {
                var resourceService = sp.GetRequiredService<IPhysicalResourceService>();
                var serviceProvider = sp.GetRequiredService<IServiceProvider>();
                return room => new DeskSelectView(room, resourceService, serviceProvider);
            });
            builder.Services.AddTransient<Func<CustomerPackageViewModel, CustomerDetailsView>>(sp =>
            {
                var customerDetailService = sp.GetRequiredService<ICustomerDetailsService>();
                return customerPackageViewModel => new CustomerDetailsView(customerPackageViewModel, customerDetailService);
            });
            builder.Services.AddSingleton(sp =>
            {
                var databaseConnectionService = sp.GetService<IDatabaseConnectionService>();
                var resourceService = ActivatorUtilities.CreateInstance<ResourceService>(sp);
                return TransactionalProxy<IPhysicalResourceService>.CreateProxy(resourceService, databaseConnectionService);
            });

            builder.Services.AddSingleton(sp =>
            {
                var databaseConnectionService = sp.GetService<IDatabaseConnectionService>();
                var packageService = ActivatorUtilities.CreateInstance<PackageService>(sp);
                return TransactionalProxy<IPackageService>.CreateProxy(packageService, databaseConnectionService);
            });

            builder.Services.AddSingleton(sp =>
            {
                var databaseConnectionService = sp.GetService<IDatabaseConnectionService>();
                var bookingService = ActivatorUtilities.CreateInstance<BookingDetailsService>(sp);
                return TransactionalProxy<IBookingService>.CreateProxy(bookingService, databaseConnectionService);
            });

            builder.Services.AddSingleton(sp =>
            {
                var databaseConnectionService = sp.GetService<IDatabaseConnectionService>();
                var customerService = ActivatorUtilities.CreateInstance<CustomerService>(sp);
                return TransactionalProxy<ICustomerService>.CreateProxy(customerService, databaseConnectionService);
            });

            builder.Services.AddSingleton(sp =>
            {
                var databaseConnectionService = sp.GetService<IDatabaseConnectionService>();
                var customerService = ActivatorUtilities.CreateInstance<CustomerDetailsService>(sp);
                return TransactionalProxy<ICustomerDetailsService>.CreateProxy(customerService, databaseConnectionService);
            });

            // New Package Booking Strategy
            builder.Services.AddSingleton<IBookingStrategy>(sp =>
            {
                var databaseConnectionService = sp.GetService<IDatabaseConnectionService>();
                var newPackageBookingStrategy = ActivatorUtilities.CreateInstance<NewPackageBookingStrategy>(sp);
                return TransactionalProxy<IBookingStrategy>.CreateProxy(newPackageBookingStrategy, databaseConnectionService);
            });

            // Package Change Booking Strategy
            builder.Services.AddSingleton<IBookingStrategy>(sp =>
            {
                var databaseConnectionService = sp.GetService<IDatabaseConnectionService>();
                var packageChangeBookingStrategy = ActivatorUtilities.CreateInstance<PackageChangeBookingStrategy>(sp);
                return TransactionalProxy<IBookingStrategy>.CreateProxy(packageChangeBookingStrategy, databaseConnectionService);
            });

            // Default Booking Strategy
            builder.Services.AddSingleton<IBookingStrategy>(sp =>
            {
                var databaseConnectionService = sp.GetService<IDatabaseConnectionService>();
                var defaultBookingStrategy = ActivatorUtilities.CreateInstance<DefaultBookingStrategy>(sp);
                return TransactionalProxy<IBookingStrategy>.CreateProxy(defaultBookingStrategy, databaseConnectionService);
            });

            builder.Services.AddTransient<IBookingProcessor, BookingProcessor>();
            builder.Services.AddTransient<Func<PackageListViewModel, UpdatePackage>>(sp =>
            {
                var packageService = sp.GetRequiredService<IPackageService>();
                return package => new UpdatePackage(package, packageService);
            });
            //services

#if WINDOWS
            builder.Services.AddSingleton<IPdfService,PdfService>();
#endif
            builder.Services.AddSingleton<IUserService, UserService>();
            builder.Services.AddSingleton<IEmailService, EmailService>();
            builder.Services.AddSingleton<ITransactionService, TransactionService>();
            builder.Services.AddSingleton<IRoomService, RoomService>();
            builder.Services.AddSingleton<IDeskService, DeskService>();

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
