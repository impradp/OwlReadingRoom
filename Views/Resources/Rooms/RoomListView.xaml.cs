using CommunityToolkit.Maui.Views;
using OwlReadingRoom.Components;
using OwlReadingRoom.Models;
using OwlReadingRoom.Services.Resources;
using OwlReadingRoom.ViewModels;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace OwlReadingRoom.Views.Resources.Rooms;

public partial class RoomListView : ContentView, INotifyPropertyChanged
{
    private ObservableCollection<RoomListViewModel> _rooms;
    private readonly IServiceProvider _serviceProvider;
    private readonly IRoomService _roomService;
    private readonly IPhysicalResourceService _resourceService;
    public RoomListView(IServiceProvider serviceProvider, IRoomService roomService, IPhysicalResourceService resourceService)
    {
        _serviceProvider = serviceProvider;
        _roomService = roomService;
        _resourceService = resourceService;
        InitializeComponent();
        BindingContext = this;
        LoadRoomData();
        
    }

    public ObservableCollection<RoomListViewModel> Rooms
    {
        get => _rooms;
        set
        {
            if (_rooms != value)
            {
                _rooms = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(HasRooms));
                OnPropertyChanged(nameof(IsEmptyState));
            }
        }
    }

    public bool HasRooms => Rooms != null && Rooms.Count > 0;
    public bool IsEmptyState => !HasRooms;

    private void OnNewRoomButtonClicked(object sender, EventArgs e)
    {
        var newRoom = _serviceProvider.GetService<NewRoom>();
        newRoom.RoomCreated += OnRoomCreated;
        Application.Current.MainPage.ShowPopup(newRoom);
    }

    private void OnRoomCreated(object sender, EventArgs e)
    {
        LoadRoomData();
    }

    private void LoadRoomData()
    {

        Rooms = new ObservableCollection<RoomListViewModel>(_resourceService.fetchRooms());
        //TODO: Fetch created room list
        //TODO: Set to Observable Collection of Rooms
    }

    private void OnRoomEditClicked(object sender, EventArgs e)
    {
        var button = sender as ActionButtonsView;
        var room = button?.BindingContext as RoomListViewModel;
        if (room != null)
        {
            //TODO: Open popup dialog for update
            var editRoomPopup = new UpdateRoom(room);
            editRoomPopup.RoomUpdated += OnRoomUpdated;
            Application.Current.MainPage.ShowPopup(editRoomPopup);
        }
    }

    private void OnRoomUpdated(object sender, EventArgs e)
    {
        LoadRoomData();
    }

    private void OnRoomEyeClicked(object sender, EventArgs e)
    {
        var button = sender as ActionButtonsView;
        var room = button?.BindingContext as Room;
        if (room != null)
        {
            //TODO: Open popup dialog for deletion
        }
    }


    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
