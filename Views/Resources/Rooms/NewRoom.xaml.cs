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

    private readonly IRoomService _roomService;
    public ObservableCollection<RoomType> RoomTypes { get; set; }
    public NewRoom(IRoomService roomService)
    {
        InitializeComponent();
        RoomTypes = new ObservableCollection<RoomType>(Enum.GetValues(typeof(RoomType)).Cast<RoomType>());
        _roomService = roomService;
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
            if(Validator.IsValidRoom(NoOfRoomsEntry.Text, RoomTypePicker.SelectedIndex)) 
            {
                //TODO: Change the default room initials to the one sent by the user
                _roomService.AddRooms((RoomType)RoomTypePicker.SelectedItem, Int32.Parse(NoOfRoomsEntry.Text));
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
