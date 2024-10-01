using Microsoft.Maui.Storage;
using MimeKit;
using MimeKit.Utils;
using OwlReadingRoom.DTOs;
using OwlReadingRoom.Services;
using OwlReadingRoom.Services.Constants;
using OwlReadingRoom.Services.Email;
using OwlReadingRoom.Services.Startup;
using System.Globalization;
using System.IO.Compression;
using System.Reflection;
using System.Text;
using System.Text.Json;

public class StartupService : IStartupService
{
    private readonly IEmailService _emailService;
    private readonly IBookingService _bookingService;
    private readonly string _logFolder;
    private const string BackupLogFile = "backup_log.json";
    private const string PackageExpiryNotifyLogFile = "package_expiry_log.json";
    private const string Subject = "Owl Reading Room : Weekly Backup";
    private const string Body = "Please find the backup for the Owl Reading Room attached here within this mail.";
    private const string AttachmentFileName = "OwlReadingRoom.zip";
    private const string LogFolderName = "Log";
    private const int MinimumDaysBetweenBackups = 5;
    private const int MinimumDaysBetweenPackageExpiryCheck = 1;

    public StartupService(IEmailService emailService, IBookingService bookingService)
    {
        _emailService = emailService;
        _logFolder = Path.Combine(FileSystem.AppDataDirectory, LogFolderName);
        _bookingService = bookingService;
    }

    public async Task InitiateWeeklyBackupAsync()
    {
        EnsureLogFolderExists();
        string logFilePath = Path.Combine(_logFolder, BackupLogFile);

        var backupLog = await LoadLogAsync(logFilePath);
        string today = DateTime.Now.ToString("yyyy-MM-dd");

        if (ShouldPerformBackup(backupLog))
        {
            try
            {
                using (var combinedStream = new MemoryStream())
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        using (var zipArchive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                        {
                            string tempFilePath = Path.GetTempFileName();
                            File.Copy(DataBaseConstants.DatabasePath, tempFilePath, true);
                            zipArchive.CreateEntryFromFile(tempFilePath, Path.GetFileName(DataBaseConstants.DatabasePath));
                        }
                        memoryStream.Position = 0;
                        await memoryStream.CopyToAsync(combinedStream);
                    }

                    using (var assetsStream = new MemoryStream())
                    {
                        string assetsPath = Path.Combine(FileSystem.AppDataDirectory, "Assets");
                        if (Directory.Exists(assetsPath))
                        {
                            await Task.Run(() => ZipFile.CreateFromDirectory(assetsPath, assetsStream, CompressionLevel.Optimal, false));

                            assetsStream.Position = 0;

                            // Combine the streams
                            using (var zipArchive = new ZipArchive(combinedStream, ZipArchiveMode.Update, true))
                            {
                                using (var entryStream = zipArchive.CreateEntry("Assets.zip").Open())
                                {
                                    await assetsStream.CopyToAsync(entryStream);
                                }
                            }
                        }
                    }

                    await _emailService.SendEmailAsync(combinedStream, Subject, Body, AttachmentFileName);

                    Console.WriteLine($"Weekly backup successfully mailed. {DateTime.Now}");
                }

                backupLog[today] = true;
                await SaveLogAsync(logFilePath, backupLog);

                Console.WriteLine($"Weekly backup initiated and logged for {today}.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send email: {ex.Message}");
            }
        }
        else
        {
            Console.WriteLine($"Backup not required at this time. Last backup was performed less than {MinimumDaysBetweenBackups} days ago.");
        }
    }

    /// <summary>
    /// Checks if the backup should be taken or not.
    /// </summary>
    /// <param name="backupLog">The dictionary containing date and flag for backup taken.</param>
    /// <returns>The boolean flag to initiate the backup process.</returns>
    private bool ShouldPerformBackup(Dictionary<string, bool> backupLog)
    {
        if (!backupLog.Any())
        {
            return true; // If no backups have been performed, we should do one
        }

        var lastBackupDate = backupLog.Keys.Max(k => DateTime.ParseExact(k, "yyyy-MM-dd", null));
        var daysSinceLastBackup = (DateTime.Now - lastBackupDate).Days;

        return daysSinceLastBackup >= MinimumDaysBetweenBackups;
    }

    /// <summary>
    /// Checks if the log folder exists or not.
    /// Creates the new log folder if it doesnot exist.
    /// </summary>
    private void EnsureLogFolderExists()
    {
        if (!Directory.Exists(_logFolder))
        {
            Directory.CreateDirectory(_logFolder);
        }
    }

    /// <summary>
    /// Deserializes the json log containing date and boolean flag to a dictionary.
    /// </summary>
    /// <param name="filePath">The file path of the json log.</param>
    /// <returns>The  dictionary containing the date and its corresponding boolean flag.</returns>
    private async Task<Dictionary<string, bool>> LoadLogAsync(string filePath)
    {
        if (!File.Exists(filePath))
        {
            return new Dictionary<string, bool>();
        }

        string jsonContent = await File.ReadAllTextAsync(filePath);
        return JsonSerializer.Deserialize<Dictionary<string, bool>>(jsonContent)
            ?? new Dictionary<string, bool>();
    }

    /// <summary>
    /// Saves the backup log into an existing json file.
    /// </summary>
    /// <param name="filePath">The file path of the json log.</param>
    /// <param name="backupLog">The dictionary conaining the date and boolean flag.</param>
    /// <returns></returns>
    private async Task SaveLogAsync(string filePath, Dictionary<string, bool> backupLog)
    {
        string jsonContent = JsonSerializer.Serialize(backupLog, new JsonSerializerOptions
        {
            WriteIndented = true
        });
        await File.WriteAllTextAsync(filePath, jsonContent);
    }

    public async Task InitiateDailyReservationEndNotifyAsync()
    {
        EnsureLogFolderExists();
        string logFilePath = Path.Combine(_logFolder, PackageExpiryNotifyLogFile);

        var backupLog = await LoadLogAsync(logFilePath);
        string today = DateTime.Now.ToString("yyyy-MM-dd");

        try
        {
            if (ShouldNotifyPackageExpiry(backupLog))
            {
                List<InactiveCustomerDetail> inactiveCustomerDetails = _bookingService.GetToBeInactiveCustomerList();
                if (inactiveCustomerDetails.Any())
                {
                    var bodyBuilder = await CreateBodyForPackageExpiryNotification(inactiveCustomerDetails);
                    await _emailService.SendEmailAsync("Upcoming Package Expiration Notification", bodyBuilder);

                    // Update backup log to indicate notification sent today
                    backupLog[today] = true;
                    await SaveLogAsync(logFilePath, backupLog);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to send email: {ex.Message}");
        }
    }

    /// <summary>
    /// Creates the HTML body for the package expiry notification email.
    /// It reads the email template, inserts customer information, and adds an embedded image.
    /// </summary>
    /// <param name="inactiveCustomerDetails">List of customers whose packages are about to expire.</param>
    /// <returns>A BodyBuilder object containing the email body and embedded resources.</returns>
    private async Task<BodyBuilder> CreateBodyForPackageExpiryNotification(List<InactiveCustomerDetail> inactiveCustomerDetails)
    {
        var bodyBuilder = new BodyBuilder();
        string emailTemplate = await ReadEmailTemplateAsync("customer_expire_template.html");

        var htmlBody = new StringBuilder(emailTemplate);
        var tableRows = new StringBuilder();

        // Generate the rows for each customer in the table
        foreach (var customer in inactiveCustomerDetails)
        {
            int? daysUntilExpiry = (customer.ExpiryDate - DateTime.Now)?.Days;
            tableRows.AppendLine($"<tr><td>{customer.FullName}</td><td>{customer.PackageName}</td><td>{daysUntilExpiry}</td></tr>");
        }

        // Set expiration threshold date and replace placeholders in the template
        var expirationThresholdDate = DateTime.Now.AddDays(AppConstants.Notification.DaysUntilExpiration)
                                                .ToString("dd MMM yyyy", CultureInfo.CreateSpecificCulture("en-US"));
        htmlBody.Replace("{Body}", tableRows.ToString())
                .Replace("{ExpirationThresholdDate}", expirationThresholdDate);

        await AddEmbeddedImageAsync(bodyBuilder, htmlBody, "OwlReadingRoom.Resources.Images.owl_logo.png");

        bodyBuilder.HtmlBody = htmlBody.ToString();
        return bodyBuilder;
    }

    /// <summary>
    /// Reads the email template from an embedded resource.
    /// </summary>
    /// <param name="fileName">The name of the template file.</param>
    /// <returns>The content of the template as a string.</returns>
    private static async Task<string> ReadEmailTemplateAsync(string fileName)
    {
        try
        {
            var assembly = Assembly.GetExecutingAssembly();
            using (var stream = assembly.GetManifestResourceStream($"OwlReadingRoom.Resources.EmailTemplate.{fileName}"))
            using (var reader = new StreamReader(stream ?? throw new FileNotFoundException($"Email template file not found: {fileName}")))
            {
                return await reader.ReadToEndAsync();
            }
        }
        catch (FileNotFoundException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Error reading email template: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Adds an embedded image to the email body and replaces the image placeholder with its Content-ID.
    /// </summary>
    /// <param name="bodyBuilder">The BodyBuilder object used to build the email.</param>
    /// <param name="htmlBody">The StringBuilder containing the HTML body of the email.</param>
    /// <param name="resourcePath">The resource path of the image.</param>
    private async Task AddEmbeddedImageAsync(BodyBuilder bodyBuilder, StringBuilder htmlBody, string resourcePath)
    {
        var assembly = Assembly.GetExecutingAssembly();
        using (var imageStream = assembly.GetManifestResourceStream(resourcePath))
        {
            if (imageStream == null) throw new FileNotFoundException($"Image resource not found in assembly: {resourcePath}");

            using (var memoryStream = new MemoryStream())
            {
                await imageStream.CopyToAsync(memoryStream);
                var imageData = memoryStream.ToArray();

                var image = new MimePart("image", "png")
                {
                    Content = new MimeContent(new MemoryStream(imageData)),
                    ContentDisposition = new ContentDisposition(ContentDisposition.Inline),
                    ContentTransferEncoding = ContentEncoding.Base64,
                    ContentId = MimeUtils.GenerateMessageId()
                };

                bodyBuilder.LinkedResources.Add(image);

                // Replace the image placeholder {0} with the generated Content-ID
                htmlBody.Replace("{0}", image.ContentId);
            }
        }
    }

    /// <summary>
    /// Determines whether to notify about package expiry based on the last notification date.
    /// </summary>
    /// <param name="packageExpiryNotificationLog">The log of past notifications keyed by date.</param>
    /// <returns>True if notification should be sent; otherwise, false.</returns>
    private bool ShouldNotifyPackageExpiry(Dictionary<string, bool> packageExpiryNotificationLog)
    {
        if (!packageExpiryNotificationLog.Any())
        {
            return true; // No prior notifications, proceed with sending one.
        }

        var lastNotifiedDate = packageExpiryNotificationLog.Keys.Max(k => DateTime.ParseExact(k, "yyyy-MM-dd", null));
        var daysSinceLastNotification = (DateTime.Now - lastNotifiedDate).Days;

        return daysSinceLastNotification >= MinimumDaysBetweenPackageExpiryCheck;
    }
}
