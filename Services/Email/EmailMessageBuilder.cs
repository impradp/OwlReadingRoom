using MimeKit;

namespace OwlReadingRoom.Services.Email;

public static class EmailMessageBuilder
{
    public static MimeMessage BuildEmailMessage(string from, string to, string subject, string bodyText, string attachmentPath)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(from, from));
        message.To.Add(new MailboxAddress(to, to));
        message.Subject = subject;

        var builder = new BodyBuilder();
        builder.TextBody = bodyText;
        if(attachmentPath != null)
        {
            builder.Attachments.Add(attachmentPath);
        }
        
        message.Body = builder.ToMessageBody();
        return message;
    }
}
