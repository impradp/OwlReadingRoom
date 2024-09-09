namespace OwlReadingRoom.Utils;

public static class DocumentHandler
{
    public static string SanitizeCustomerFolderName(string contactNumber)
    {
        return contactNumber.Replace(" ", "").Replace("+", "").Replace("-", "");
    }

    public static void EnsureDirectoryExists(string directoryPath)
    {
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }
    }

    public static string CopyFileToCustomerFolder(string sourceFilePath, string destinationFolderPath)
    {
        string fileName = Path.GetFileName(sourceFilePath);
        string destinationFilePath = Path.Combine(destinationFolderPath, fileName);

        File.Copy(sourceFilePath, destinationFilePath, true);

        return destinationFilePath;
    }
}
