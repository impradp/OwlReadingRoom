using CommunityToolkit.Maui.Views;
using Microsoft.Extensions.Configuration;
using OwlReadingRoom.Components;
using OwlReadingRoom.Configurations;
using OwlReadingRoom.Models;
using OwlReadingRoom.Services.Resources;
using OwlReadingRoom.Utils;
using OwlReadingRoom.ViewModels;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace OwlReadingRoom.Views.Resources.Rooms;

public partial class RoomListView : ContentView, INotifyPropertyChanged
{
    private List<RoomListViewModel> _rooms;
    private readonly IServiceProvider _serviceProvider;
    private readonly IRoomService _roomService;
    private readonly IPhysicalResourceService _resourceService;
    private readonly Func<RoomListViewModel, UpdateRoom> _updateRoomFactory;
    private readonly Func<RoomListViewModel, DeskLayout> _deskLayoutFactory;
    private bool _shouldShowEditButton;
    private bool _shouldShowEyeButton;
    public RoomListView(IServiceProvider serviceProvider, IRoomService roomService, IPhysicalResourceService resourceService, Func<RoomListViewModel, UpdateRoom> updateRoomFactory, Func<RoomListViewModel, DeskLayout> deskLayoutFactory)
    {
        _serviceProvider = serviceProvider;
        _roomService = roomService;
        _resourceService = resourceService;
        InitializeComponent();
        BindingContext = this;
        LoadRoomData();
        _updateRoomFactory = updateRoomFactory;
        _deskLayoutFactory = deskLayoutFactory;
    }

    public List<RoomListViewModel> Rooms
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

    public bool ShouldShowEditButton
    {
        get => _shouldShowEditButton;
        set
        {
            if (_shouldShowEditButton != value)
            {
                _shouldShowEditButton = value;
                OnPropertyChanged();
            }
        }
    }


    public bool ShouldShowEyeButton
    {
        get => _shouldShowEyeButton;
        set
        {
            if (_shouldShowEyeButton != value)
            {
                _shouldShowEyeButton = value;
                OnPropertyChanged();
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
        try
        {
            #region Fetch actions flag from environment variables

            IConfiguration configuration = _serviceProvider.GetService<IConfiguration>();
            ActionFeatures actions = ConfigurationHandler.GetActionFeatures(configuration, "RoomActions");
            ShouldShowEditButton = actions.Edit;
            ShouldShowEyeButton = actions.View;

            #endregion

            Rooms = _resourceService.fetchRooms();
        }
        catch (Exception ex)
        {
            ExceptionHandler.HandleException("Loading rooms", ex);
        }
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
            var editRoomPopup = _updateRoomFactory(room);
            editRoomPopup.RoomUpdated += OnRoomUpdated;
            Application.Current.MainPage.ShowPopup(editRoomPopup);
        }
    }

    private void OnRoomUpdated(object sender, EventArgs e)
    {
        LoadRoomData();
    }

    private async void OnRoomEyeClicked(object sender, EventArgs e)
    {

        try
        {
            var button = sender as ActionButtonsView;
            var room = button?.BindingContext as RoomListViewModel;
            if (room != null)
            {
                var deskLayoutPopup = _deskLayoutFactory(room);
                await Application.Current.MainPage.ShowPopupAsync(deskLayoutPopup);
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.HandleException("Displaying layout.", ex);
        }

    }


    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
