using CommunityToolkit.Maui.Views;
using OwlReadingRoom.Services.Resources;
using OwlReadingRoom.Utils;
using OwlReadingRoom.ViewModels;
using OwlReadingRoom.Views.Resources.Rooms.Plans;
using static OwlReadingRoom.Services.Constants.AppConstants;

namespace OwlReadingRoom.Views.Resources.Rooms;

public partial class DeskSelectView : Popup
{
    private ACRoomPlan _acRoomPlan;
    private NonACRoomPlan _nonAcRoomPlan;
    private List<DeskInfoViewModel> _desks;
    public RoomListViewModel Room { get; set; }
    public event EventHandler<string> DeskSelected;
    private IPhysicalResourceService _resourceSerivce;
    private readonly IServiceProvider _serviceProvider;
    private string SelectedDeskName;

    public DeskSelectView(RoomListViewModel room, IPhysicalResourceService resourceSerivce, IServiceProvider serviceProvider)
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
        if (!String.IsNullOrEmpty(SelectedDeskName))
        {
            DeskSelected?.Invoke(this, SelectedDeskName);
        }
        Close();
    }

    /// <summary>
    /// Invoked when the popup is dismissed by tapping outside of the popup.
    /// This is an override method for the existing method in popup community toolkit.
    /// </summary>
    /// <param name="token">The event token passed when a popup is invoked</param>
    /// <returns>Completed task for closing the modal popup.</returns>
    protected override async Task OnDismissedByTappingOutsideOfPopup(CancellationToken token = default(CancellationToken))
    {
        if (!String.IsNullOrEmpty(SelectedDeskName))
        {
            DeskSelected?.Invoke(this, SelectedDeskName);
        }
        await OnClosed(ResultWhenUserTapsOutsideOfPopup, wasDismissedByTappingOutsideOfPopup: true, token);
    }
    /// <summary>
    /// Loads all desks information for the selected room.
    /// </summary>
    private void LoadDesks()
    {
        try
        {
            DeskSelectLabel.Text = Room.RoomType + "(" + Room.Name + ")";
            _desks = _resourceSerivce.GetDeskInfoPerRoom(Room.Id);

            switch (Room.RoomType)
            {
                case RoomConstants.AcRoom:
                    _acRoomPlan = ActivatorUtilities.CreateInstance<ACRoomPlan>(_serviceProvider, _desks, false);
                    _acRoomPlan.Desks = _desks;
                    _acRoomPlan.IsSelectable = Room.IsSelectable;
                    _acRoomPlan.DeskSelected += OnDeskSelected;
                    DynamicLayoutArea.Content = _acRoomPlan;
                    break;
                case RoomConstants.NonAcRoom:
                    _nonAcRoomPlan = ActivatorUtilities.CreateInstance<NonACRoomPlan>(_serviceProvider, _desks, false);
                    _nonAcRoomPlan.Desks = _desks;
                    _nonAcRoomPlan.IsSelectable = Room.IsSelectable;
                    _nonAcRoomPlan.DeskSelected += OnDeskSelected;
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

    /// <summary>
    /// Handles the desk selected desk event.
    /// </summary>
    /// <param name="sender">The button that triggered this event.</param>
    /// <param name="deskName">The name of the desk that is selected and is passed as a event param.</param>
    private void OnDeskSelected(object sender, string deskName)
    {
        SelectedDeskName = deskName;
    }
}