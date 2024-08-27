using OwlReadingRoom.Events;

namespace OwlReadingRoom.Components.AlertDialog;

public partial class AlertView : ContentView
{
    public AlertView()
    {
        InitializeComponent();
        AlertService.Instance.OnAlertRequested += OnAlertRequested;
    }

    private async void OnAlertRequested(object sender, AlertEventArgs e)
    {
        TitleLabel.Text = e.Title;
        MessageLabel.Text = e.Message;
        SetAlertStyle(e.Type);

        AlertContainer.IsVisible = true;
        await Task.Delay(e.DurationInSeconds * 1000);
        AlertContainer.IsVisible = false;
    }

    private void SetAlertStyle(AlertType type)
    {
        Color color;
        string iconSource;
        Color backgroundColor;

        switch (type)
        {
            case AlertType.Success:
                color = Color.FromArgb("#22C55E");
                iconSource = "success_icon.png";
                backgroundColor = Color.FromArgb("#F6FDF9");
                break;
            case AlertType.Warning:
                color = Color.FromArgb("#F59E0B");
                iconSource = "warning_icon.png";
                backgroundColor = Color.FromArgb("#fdf0da");
                break;
            case AlertType.Error:
                color = Color.FromArgb("#EF4444");
                iconSource = "error_icon.png";
                backgroundColor = Color.FromArgb("#FEF8F8");
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type));
        }

        TitleLabel.TextColor = color;
        MessageLabel.TextColor = color;
        AlertIcon.Source = iconSource;
        CloseIconColor.TintColor = color;
        FrameBackgroundColor.BackgroundColor = backgroundColor;
    }

    private void OnCloseAlertTapped(object sender, EventArgs e)
    {
        AlertContainer.IsVisible = false;
    }
}
