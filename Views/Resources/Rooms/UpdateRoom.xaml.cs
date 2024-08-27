using CommunityToolkit.Maui.Views;
using OwlReadingRoom.Components.AlertDialog;
using OwlReadingRoom.Utils;
using OwlReadingRoom.ViewModels;

namespace OwlReadingRoom.Views.Resources.Rooms;

public partial class UpdateRoom : Popup
{
    public event EventHandler RoomUpdated;
    public RoomListViewModel Room;
    public UpdateRoom(RoomListViewModel room)
    {
        InitializeComponent();
        Room = room;
        BindingContext = this;
        PopulateFields();
    }

    private void PopulateFields()
    {
        IdEntry.Text = Room.Id.ToString();
        NameEntry.Text = Room.Name;
        NoOfDeskEntry.Text = Room.TotalDesks.ToString();
        EditRoomHeaderLabel.Text = String.Concat("Edit Room #", Room.Id);
        // Populate other fields as needed
    }

    private void OnCloseClicked(object sender, EventArgs e)
    {
        Close();
    }

    private void OnClearClicked(object sender, EventArgs e)
    {
        NameEntry.Text = "";
        NoOfDeskEntry.Text = "";
    }

    private async void OnUpdateClicked(object sender, EventArgs e)
    {
        try
        {
            //TODO: Validate the incoming room data
            //TODO: Update the Room details and create desks if necessary
            RoomUpdated?.Invoke(this, EventArgs.Empty);
            await CloseAsync();
            AlertService.Instance.ShowAlert("Info", "Room updated successfully.", AlertType.Info);

        }
        catch (Exception ex)
        {
            ExceptionHandler.HandleException("Updating new room details", ex);
        }
    }

}
