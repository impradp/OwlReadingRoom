using System.Diagnostics;

namespace OwlReadingRoom.Utils
{
    class ExceptionHandler
    {
        public static void HandleException(string context, Exception ex)
        {
            // Log the exception
            Debug.WriteLine($"Error in {context}: {ex.Message}");
            Debug.WriteLine($"Stack Trace: {ex.StackTrace}");

            // Optionally, show an alert to the user
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await CustomAlert.ShowAlert("Error", $"An error occurred while {context.ToLower()}. Please try again.", "OK");
            });
        }
    }
}
