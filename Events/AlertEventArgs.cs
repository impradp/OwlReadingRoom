using OwlReadingRoom.Components.AlertDialog;

namespace OwlReadingRoom.Events
{
    public class AlertEventArgs : EventArgs
    {
        public string Title { get; }
        public string Message { get; }
        public AlertType Type { get; }
        public int DurationInSeconds { get; }

        public AlertEventArgs(string title, string message, AlertType type, int durationInSeconds)
        {
            Title = title;
            Message = message;
            Type = type;
            DurationInSeconds = durationInSeconds;
        }
    }
}
