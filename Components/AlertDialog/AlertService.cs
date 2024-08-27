using OwlReadingRoom.Events;

namespace OwlReadingRoom.Components.AlertDialog
{
    public class AlertService
    {
        private static AlertService _instance;
        public static AlertService Instance => _instance ??= new AlertService();

        private AlertService() { }

        public event EventHandler<AlertEventArgs> OnAlertRequested;

        public void ShowAlert(string title, string message, AlertType type = AlertType.Success, int durationInSeconds = 3)
        {
            OnAlertRequested?.Invoke(this, new AlertEventArgs(title, message, type, durationInSeconds));
        }
    }
}
