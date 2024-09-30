namespace OwlReadingRoom.Services.Email;

public interface IEmailService
{
    /// <summary>
    /// Sends an email with attachments.
    /// </summary>
    /// <param name="smtpSettings">The object containing the smtp credentials</param>
    /// <param name="attachmentMemoryStream">The attachment memory streams </param>
    /// <param name="subject">The subject of the email to be sent.</param>
    /// <param name="body">The body of the email.</param>
    /// <param name="attachmentFileName">The name of the attachment to be sent.</param>
    /// <returns>The completed task for email trigger.</returns>
    Task SendEmailAsync(MemoryStream attachmentMemoryStream, string subject, string body, string attachmentFileName);
}
