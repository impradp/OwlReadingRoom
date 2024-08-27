using CommunityToolkit.Maui.Views;
using OwlReadingRoom.Models.Enums;
using OwlReadingRoom.Utils;
using OwlReadingRoom.ViewModels;

namespace OwlReadingRoom.Views.Resources.Rooms;

public partial class DeskLayout : Popup
{
    public RoomListViewModel Room { get; set; }
    public DeskLayout(RoomListViewModel room)
    {
        InitializeComponent();
        SetDesks();
        Room = room;
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

        Desks = new List<DeskInfoViewModel>
            {
                new DeskInfoViewModel { Name = "Gauri-01", Status = DeskStatus.Available, Color =  Utility.GetBackGroundColorByDeskStatus(DeskStatus.Available), TextColor = Utility.GetTextColorByDeskStatus(DeskStatus.Available)},
                new DeskInfoViewModel { Name = "Gauri-02", Status = DeskStatus.NotAvailable, Color = Utility.GetBackGroundColorByDeskStatus(DeskStatus.NotAvailable), TextColor = Utility.GetTextColorByDeskStatus(DeskStatus.NotAvailable) ,Message="Under Maintainence"},
                new DeskInfoViewModel { Name = "Gauri-03 ", Status = DeskStatus.Reserved, Color = Utility.GetBackGroundColorByDeskStatus(DeskStatus.Reserved), TextColor = Utility.GetTextColorByDeskStatus(DeskStatus.Reserved) ,Message="Reserved by Simon from 20/12/2023 to 20/1/2024"},
                // ... add more desks
            };
    }
}
