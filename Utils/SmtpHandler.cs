using Auth0.OidcClient;
using Microsoft.Extensions.Configuration;
using OwlReadingRoom.Services.Email;

namespace OwlReadingRoom.Utils;

public static class SmtpHandler
{
    public static SmtpSettings GetSmtpSettings(IConfiguration configuration)
    {
        var settings = configuration.GetRequiredSection("SmtpSettings");
        return new SmtpSettings
        {
            Server = settings.GetSection("Server").Value,
            Port =  int.TryParse(settings.GetSection("Port").Value, out int port) ? port : 0,
            Username = settings.GetSection("Username").Value,
            Password = settings.GetSection("Password").Value,
            Receiver = settings.GetSection("Receiver").Value,
            UseSsl = bool.TryParse(settings.GetSection("UseSsl").Value, out bool useSsl) ? useSsl : false  
        };
    }
}
