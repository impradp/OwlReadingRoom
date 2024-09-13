using CommunityToolkit.Maui.Views;
using OwlReadingRoom.Components.AlertDialog;
using OwlReadingRoom.Models;
using OwlReadingRoom.Services.Resources;
using OwlReadingRoom.Utils;
using System.Collections.ObjectModel;
namespace OwlReadingRoom.Views.Resources.Rooms;

public partial class NewRoom : Popup
{
    public event EventHandler<EventArgs> RoomCreated;

    private readonly IPhysicalResourceService _resourceService;
    public ObservableCollection<RoomType> RoomTypes { get; set; }
    public NewRoom(IPhysicalResourceService resourceService)
    {
        InitializeComponent();
        RoomTypes = new ObservableCollection<RoomType>(Enum.GetValues(typeof(RoomType)).Cast<RoomType>());
        _resourceService = resourceService;
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
        NoOfRoomsEntry.Text = "";
    }

    /// <summary>
    /// Creates the room and underlying desks for the created room.
    /// </summary>
    /// <param name="sender">The button that triggered this event.</param>
    /// <param name="e">The event arguments passed down to save this information.</param>
    private async void OnCreateClicked(object sender, EventArgs e)
    {
        try
        {
            if (Validator.IsValidRoom(NoOfRoomsEntry.Text, RoomTypePicker.SelectedIndex))
            {
                //TODO: Change the default room initials to the one sent by the user
                _resourceService.AddRooms((RoomType)RoomTypePicker.SelectedItem, Int32.Parse(NoOfRoomsEntry.Text));

                //TODO: Create desks for rooms using Room Plan
                // If RoomType is AC Room then create the desks for AC Room using Room Plan.

                AlertService.Instance.ShowAlert("Success", "New room created successfully.", AlertType.Success);
                RoomCreated?.Invoke(this, EventArgs.Empty);
                await CloseAsync();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.HandleException("Saving new room details", ex);
        }
    }

}
