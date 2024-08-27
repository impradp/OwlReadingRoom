using CommunityToolkit.Maui.Views;
using OwlReadingRoom.Components;
using OwlReadingRoom.Models;
using OwlReadingRoom.ViewModels;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace OwlReadingRoom.Views.Resources.Rooms;

public partial class RoomListView : ContentView, INotifyPropertyChanged
{
    private ObservableCollection<RoomListViewModel> _rooms;

    public RoomListView()
    {
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
        var newRoom = new NewRoom();
        newRoom.RoomCreated += OnRoomCreated;
        Application.Current.MainPage.ShowPopup(newRoom);
    }

    private void OnRoomCreated(object sender, EventArgs e)
    {
        LoadRoomData();
    }

    private void LoadRoomData()
    {
        //TODO: Fetch created room list
        //TODO: Set to Observable Collection of Rooms
        Rooms = new ObservableCollection<RoomListViewModel>
        {
            new RoomListViewModel(){Id=1,Name="RM-101",RoomType="AC Room", AvailableDesks=6,TotalDesks=10},
            new RoomListViewModel(){Id=2,Name="RM-102",RoomType="Non AC Room", AvailableDesks=30,TotalDesks=50}
        };
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
        var room = button?.BindingContext as RoomListViewModel;
        if (room != null)
        {
            var deskLayoutPopup = new DeskLayout(room);
            Application.Current.MainPage.ShowPopup(deskLayoutPopup);
        }
    }


    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
