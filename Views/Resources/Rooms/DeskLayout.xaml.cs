
using CommunityToolkit.Maui.Views;
using OwlReadingRoom.Models.Enums;
using OwlReadingRoom.Services.Resources;
using OwlReadingRoom.Utils;
using OwlReadingRoom.ViewModels;

namespace OwlReadingRoom.Views.Resources.Rooms;

public partial class DeskLayout : Popup
{
    private IPhysicalResourceService _resourceSerivce;
    public RoomListViewModel Room { get; set; }
    public DeskLayout(RoomListViewModel room, IPhysicalResourceService resourceSerivce)
    {
        InitializeComponent();
        Room = room;
        _resourceSerivce = resourceSerivce;
        SetDesks();
        BindingContext = this;
    }

    public List<DeskInfoViewModel> Desks { get; set; }

    private void OnCloseButtonClicked(object sender, EventArgs e)
    {
        Close();
    }

    private void SetDesks()
    {
        //TODO: Fetch desk info based on room id
        // Populate this list with actual desk data
        Desks =  _resourceSerivce.GetDeskInfoPerRoom(Room.Id);
    }
}
