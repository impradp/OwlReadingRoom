using CommunityToolkit.Maui.Views;
using OwlReadingRoom.Utils;

namespace OwlReadingRoom.Views.Resources;

public partial class NewRoom : Popup
{
    public event EventHandler<EventArgs> RoomCreated;
    public NewRoom()
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
    }

    private async void OnCreateClicked(object sender, EventArgs e)
    {
        try
        {
            //TODO: Validate the incoming room data
            //TODO: Save the Room details
            RoomCreated?.Invoke(this, EventArgs.Empty);
            await CloseAsync();
        }
        catch (Exception ex)
        {
            ExceptionHandler.HandleException("Saving mew room details", ex);
        }
    }

}
