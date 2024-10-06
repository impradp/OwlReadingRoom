using CommunityToolkit.Maui.Core.Primitives;
using iText.StyledXmlParser.Jsoup.Nodes;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Utils;
using OwlReadingRoom.Services.Constants;
using OwlReadingRoom.Utils;
using SkiaSharp;
using System.IO;
using System.IO.Compression;
using System.Reflection;

namespace OwlReadingRoom.Services.Email;

public class EmailService : IEmailService
{
    private readonly SmtpSettings _smtpSettings;
    public EmailService(SmtpSettings smtpSettings)
    {
        _smtpSettings = smtpSettings;
    }
    public async Task SendEmailAsync(string subject, BodyBuilder bodyBuilder)
    {
        try
        {
            // Create the email message
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_smtpSettings.SenderName, _smtpSettings.Username));
            message.To.Add(new MailboxAddress(_smtpSettings.ReceiverName, _smtpSettings.Receiver));
            message.Subject = subject;

            // Set the message body
            message.Body = bodyBuilder.ToMessageBody();
            // Send the email
            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_smtpSettings.Server, _smtpSettings.Port, _smtpSettings.UseSsl ? SecureSocketOptions.SslOnConnect : SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(_smtpSettings.Username, _smtpSettings.Password);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }

            Console.WriteLine("Package expiry email sent successfully.");
        }
        catch (Exception ex)
        {
            ExceptionHandler.HandleException("Sending pacakge expiry notification via email", ex);
        }

    }
    

    public async Task SendEmailAsync(MemoryStream attachmentMemoryStream, string subject, string body, string attachmentFileName)
    {
        try
        {
            //Set position for memory reader to beginning to stream
            attachmentMemoryStream.Position = 0;

            // Create the email message
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_smtpSettings.SenderName, _smtpSettings.Username));
            message.To.Add(new MailboxAddress(_smtpSettings.ReceiverName, _smtpSettings.Receiver));
            message.Subject = subject;

            // Create the message body and attachment using BodyBuilder
            var builder = new BodyBuilder
            {
                TextBody = body
            };

            // Add the attachment
            await builder.Attachments.AddAsync(attachmentFileName, attachmentMemoryStream, ContentType.Parse("application/zip"));

            // Set the message body
            message.Body = builder.ToMessageBody();

            // Send the email
            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_smtpSettings.Server, _smtpSettings.Port, _smtpSettings.UseSsl ? SecureSocketOptions.SslOnConnect : SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(_smtpSettings.Username, _smtpSettings.Password);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }

            Console.WriteLine("Email sent successfully.");
        }
        catch (Exception ex)
        {
            ExceptionHandler.HandleException("Sending backup attachments via email", ex);
        }

    }

}
