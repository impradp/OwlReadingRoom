using CommunityToolkit.Maui.Views;
using OwlReadingRoom.Components.AlertDialog;
using OwlReadingRoom.Utils;

namespace OwlReadingRoom.Views.Resources;

public partial class NewPackage : Popup
{
    public event EventHandler<EventArgs> PackageCreated;
    public NewPackage()
    {
        InitializeComponent();
        BindingContext = this;
    }

    private void OnRoomTypeSelectedIndexChanged(object sender, EventArgs e)
    {
        RoomTypeLabel.IsVisible = RoomTypePicker.SelectedIndex == -1;
    }

    private void OnCloseClicked(object sender, EventArgs e)
    {
        Close();
    }

    private void OnClearClicked(object sender, EventArgs e)
    {

        RoomTypeLabel.Text = "Select";
        RoomTypePicker.SelectedIndex = -1;
        PackageName.Text = "";
        AmountEntry.Text = "";
        DaysEntry.Text = "";
    }

    private async void OnCreateClicked(object sender, EventArgs e)
    {
        try
        {
            //TODO: Validate the incoming room data
            //TODO: Save the Room details
            PackageCreated?.Invoke(this, EventArgs.Empty);
            await CloseAsync();
            AlertService.Instance.ShowAlert("Success", "New package created successfully.", AlertType.Success);

        }
        catch (Exception ex)
        {
            ExceptionHandler.HandleException("Saving mew room details", ex);
        }
    }

}
