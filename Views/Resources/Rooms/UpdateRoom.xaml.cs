using CommunityToolkit.Maui.Views;
using OwlReadingRoom.Components.AlertDialog;
using OwlReadingRoom.Services.Resources;
using OwlReadingRoom.Utils;
using OwlReadingRoom.ViewModels;

namespace OwlReadingRoom.Views.Resources.Rooms;

public partial class UpdateRoom : Popup
{
    private readonly IPhysicalResourceService _resourceService;
    public event EventHandler RoomUpdated;
    public RoomListViewModel Room;
    public UpdateRoom(RoomListViewModel room, IPhysicalResourceService resourceService)
    {
        InitializeComponent();
        Room = room;
        BindingContext = this;
        PopulateFields();
        _resourceService = resourceService;
    }

    private void PopulateFields()
    {
        IdEntry.Text = Room.Id.ToString();
        NameEntry.Text = Room.Name;
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
    }

    private async void OnUpdateClicked(object sender, EventArgs e)
    {
        try
        {
            if (Validator.isValidRoomName(NameEntry.Text))
            {
                _resourceService.UpdateRoom(Room, NameEntry.Text);
                RoomUpdated?.Invoke(this, EventArgs.Empty);
                await CloseAsync();
                AlertService.Instance.ShowAlert("Info", "Room updated successfully.", AlertType.Info);
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.HandleException("Updating new room details", ex);
        }
    }

}
