
using CommunityToolkit.Maui.Views;
using OwlReadingRoom.Services.Resources;
using OwlReadingRoom.ViewModels;
using OwlReadingRoom.Views.Resources.Rooms.Plans;

namespace OwlReadingRoom.Views.Resources.Rooms;

public partial class DeskLayout : Popup
{
    private IPhysicalResourceService _resourceSerivce;
    public RoomListViewModel Room { get; set; }
    private ACRoomPlan _acRoomPlan;
    private NonACRoomPlan _nonAcRoomPlan;
    private readonly IServiceProvider _serviceProvider;
    private List<DeskInfoViewModel> _desks;

    public DeskLayout(RoomListViewModel room, IPhysicalResourceService resourceSerivce, IServiceProvider serviceProvider)
    {
        InitializeComponent();
        Room = room;
        _resourceSerivce = resourceSerivce;
        _serviceProvider = serviceProvider;
        LoadDesks();
        BindingContext = this;
    }

    private void OnCloseButtonClicked(object sender, EventArgs e)
    {
        Close();
    }
    /// <summary>
    /// Loads all desks information for the selected room.
    /// </summary>
    private void LoadDesks()
    {
        _desks = _resourceSerivce.GetDeskInfoPerRoom(Room.Id);

        switch (Room.RoomType)
        {
            case "AC Room":
                _acRoomPlan = ActivatorUtilities.CreateInstance<ACRoomPlan>(_serviceProvider, _desks);
                DynamicLayoutArea.Content = _acRoomPlan;
                break;
                //Fix: room should be Room.
            case "Non-AC room":
                _nonAcRoomPlan = ActivatorUtilities.CreateInstance<NonACRoomPlan>(_serviceProvider, _desks);
                _nonAcRoomPlan.Desks = _desks;
                DynamicLayoutArea.Content = _nonAcRoomPlan;
                break;
            default:
                // No plan detected.
                break;

        }
    }
}
