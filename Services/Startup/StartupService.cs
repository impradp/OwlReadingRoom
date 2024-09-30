using OwlReadingRoom.Services.Constants;
using OwlReadingRoom.Services.Email;
using OwlReadingRoom.Services.Startup;
using System.IO.Compression;
using System.Text.Json;

public class StartupService : IStartupService
{
    private readonly IEmailService _emailService;
    private readonly string _logFolder;
    private const string BackupLogFile = "backup_log.json";
    private const string Subject = "Owl Reading Room : Weekly Backup";
    private const string Body = "Please find the backup for the Owl Reading Room attached here within this mail.";
    private const string AttachmentFileName = "OwlReadingRoom.zip";
    private const string LogFolderName = "Log";
    private const int MinimumDaysBetweenBackups = 5;

    public StartupService(IEmailService emailService)
    {
        _emailService = emailService;
        _logFolder = Path.Combine(FileSystem.AppDataDirectory, LogFolderName);
    }

    public async Task InitiateWeeklyBackupAsync()
    {
        EnsureLogFolderExists();
        string logFilePath = Path.Combine(_logFolder, BackupLogFile);

        var backupLog = await LoadBackupLogAsync(logFilePath);
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
                await SaveBackupLogAsync(logFilePath, backupLog);

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
    private async Task<Dictionary<string, bool>> LoadBackupLogAsync(string filePath)
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
    private async Task SaveBackupLogAsync(string filePath, Dictionary<string, bool> backupLog)
    {
        string jsonContent = JsonSerializer.Serialize(backupLog, new JsonSerializerOptions
        {
            WriteIndented = true
        });
        await File.WriteAllTextAsync(filePath, jsonContent);
    }
}