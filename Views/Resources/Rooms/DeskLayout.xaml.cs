
using CommunityToolkit.Maui.Views;
using OwlReadingRoom.Services.Constants;
using OwlReadingRoom.Services.Resources;
using OwlReadingRoom.Utils;
using OwlReadingRoom.ViewModels;
using OwlReadingRoom.Views.Resources.Rooms.Plans;
using static OwlReadingRoom.Services.Constants.AppConstants;

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
        try
        {
            _desks = _resourceSerivce.GetDeskInfoPerRoom(Room.Id);

            switch (Room.RoomType)
            {
                case RoomConstants.AcRoom:
                    _acRoomPlan = ActivatorUtilities.CreateInstance<ACRoomPlan>(_serviceProvider, _desks, false);
                    _acRoomPlan.Desks = _desks;
                    _acRoomPlan.IsSelectable = Room.IsSelectable;
                    DynamicLayoutArea.Content = _acRoomPlan;
                    break;
                case RoomConstants.NonAcRoom:
                    _nonAcRoomPlan = ActivatorUtilities.CreateInstance<NonACRoomPlan>(_serviceProvider, _desks, false);
                    _nonAcRoomPlan.Desks = _desks;
                    _nonAcRoomPlan.IsSelectable = Room.IsSelectable;
                    DynamicLayoutArea.Content = _nonAcRoomPlan;
                    break;
                default:
                    // No plan detected.
                    break;

            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.HandleException("Loading into desks", ex);
        }

    }
}
