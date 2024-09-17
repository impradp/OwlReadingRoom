using Microsoft.Extensions.Configuration;
using OwlReadingRoom.Services.Email;

namespace OwlReadingRoom.Configurations
{
    public static class ConfigurationHandler
    {
        //public static SmtpSettings GetSmtpSettings(IConfiguration configuration)
        //{
        //    var settings = configuration.GetRequiredSection("SmtpSettings");
        //    return new SmtpSettings
        //    {
        //        Server = settings.GetSection("Server").Value,
        //        Port = int.TryParse(settings.GetSection("Port").Value, out int port) ? port : 0,
        //        Username = settings.GetSection("Username").Value,
        //        Password = settings.GetSection("Password").Value,
        //        Receiver = settings.GetSection("Receiver").Value,
        //        UseSsl = bool.TryParse(settings.GetSection("UseSsl").Value, out bool useSsl) ? useSsl : false
        //    };
        //}

        public static ActionFeatures GetActionFeatures(IConfiguration configuration, string actionIdentifier)
        {
            var actionIdentifierFeatures = configuration.GetRequiredSection("Features").GetRequiredSection(actionIdentifier);
            return new ActionFeatures
            {
                Delete = Boolean.Parse(actionIdentifierFeatures.GetSection("Delete").Value),
                Edit = Boolean.Parse(actionIdentifierFeatures.GetSection("Edit").Value),
                View = Boolean.Parse(actionIdentifierFeatures.GetSection("View").Value)
            };
        }
    }
}
