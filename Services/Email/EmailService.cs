using MailKit.Net.Smtp;

namespace OwlReadingRoom.Services.Email;

public class EmailService : IEmailService
{
    private readonly SmtpSettings _smtpSettings;
    public EmailService(SmtpSettings smtpSettings)
    {
        _smtpSettings = smtpSettings;
    }

    public void SendEmail()
    {
        //TODO: Change the subjest, message and the folder location 
        var message = EmailMessageBuilder.BuildEmailMessage(_smtpSettings.Username,
            _smtpSettings.Receiver,
            "Subject",
            "Please find the attached ZIP file.",
            "");
        try
        {
            using (var client = new SmtpClient())
            {
                client.Connect(_smtpSettings.Server, _smtpSettings.Port, _smtpSettings.UseSsl ? MailKit.Security.SecureSocketOptions.SslOnConnect : MailKit.Security.SecureSocketOptions.StartTls);
                client.Authenticate(_smtpSettings.Username, _smtpSettings.Password);

                client.Send(message);
                client.Disconnect(true);
            }
            Console.WriteLine("Email sent successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to send email: {ex.Message}");
        }
    }
}
