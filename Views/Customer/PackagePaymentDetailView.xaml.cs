using CommunityToolkit.Maui.Views;
using OwlReadingRoom.Services;
using OwlReadingRoom.Services.Resources;
using OwlReadingRoom.Utils;
using OwlReadingRoom.ViewModels;
using OwlReadingRoom.Views.Resources.Rooms;

namespace OwlReadingRoom.Views.Customer;

public partial class PackagePaymentDetailView : ContentView
{
    public string SelectedDesk { get; set; }
    public List<RoomListViewModel> RoomOptions { get; private set; }
    public List<PackageListViewModel> PackageOptions { get; private set; }

    private PackageListViewModel _selectedPackage;
    private RoomListViewModel _selectedRoom;
    private DateTime _packageStartDate;
    private DateTime _packageEndDate;
    private IPhysicalResourceService _resourceService;
    private readonly IServiceProvider _serviceProvider;

    public PackageAndPaymentEditViewModel PackagePaymentDetail { get; private set; }
    public bool IsEditable { get; private set; }

    public PackagePaymentDetailView(PackageAndPaymentEditViewModel packageAndPaymentDetail,
        IPhysicalResourceService resourceService, IServiceProvider serviceProvider)
    {
        InitializeComponent();
        PackagePaymentDetail = packageAndPaymentDetail;
        _resourceService = resourceService;
        _serviceProvider = serviceProvider;
        IsEditable = PackagePaymentDetail.Id == null;
        LoadResources();
        LoadExistingData();
        BindingContext = this;
    }

    /// <summary>
    /// Loads all the resources like packages, rooms and underlying desks for selection.
    /// </summary>
    private void LoadResources()
    {
        IPackageService packageService = _serviceProvider.GetService<IPackageService>();
        PackageOptions = packageService.GetPackageList();
        OnPropertyChanged(nameof(PackageOptions));
    }

    /// <summary>
    /// Loads the existing data into the selected object.
    /// </summary>
    private void LoadExistingData()
    {
        if (PackagePaymentDetail != null)
        {
            //TODO: Fetch Booking Details
            //TODO: Fetch existing package
            //TODO: Fetch existing rooms and use it as selected packages/rooms and desks

            SelectedPackage = PackagePaymentDetail.Package;
            SelectedRoom = PackagePaymentDetail.Room;
            SelectedDesk = PackagePaymentDetail.DeskName;
            PackageStartDate = PackagePaymentDetail.StartDate ?? DateTime.Today;
            PackageEndDate = PackagePaymentDetail.EndDate ?? DateTime.Today.AddDays(SelectedPackage?.Days ?? 0);

            // Update UI elements
            OnPropertyChanged(nameof(SelectedPackage));
            OnPropertyChanged(nameof(SelectedRoom));
            OnPropertyChanged(nameof(SelectedDesk));
            OnPropertyChanged(nameof(PackageStartDate));
            OnPropertyChanged(nameof(PackageEndDate));
            OnPropertyChanged(nameof(IsEditable));
        }
    }

    #region Event Handler
    public PackageListViewModel SelectedPackage
    {
        get => _selectedPackage;
        set
        {
            if (IsEditable)
            {
                _selectedPackage = value;
                OnPropertyChanged();
                LoadRoomsForSelectedPackage();
                UpdatePackageEndDate();
            }
        }
    }

    public RoomListViewModel SelectedRoom
    {
        get => _selectedRoom;
        set
        {
            if (IsEditable)
            {
                _selectedRoom = value;
                OnPropertyChanged();
                LoadDesksForSelectedRoom();
            }
        }
    }

    public DateTime PackageStartDate
    {
        get => _packageStartDate;
        set
        {
            if (IsEditable)
            {
                _packageStartDate = value;
                OnPropertyChanged();
                UpdatePackageEndDate();
            }
        }
    }
    public DateTime PackageEndDate
    {
        get => _packageEndDate;
        set
        {
            _packageEndDate = value;
            OnPropertyChanged();
        }
    }
    #endregion


    /// <summary>
    /// Updates the package end date upon package selection
    /// </summary>
    private void UpdatePackageEndDate()
    {
        if (SelectedPackage != null)
        {
            PackageEndDate = PackageStartDate.AddDays(SelectedPackage.Days);
        }
    }


    /// <summary>
    /// Loads the associated rooms for selected package.
    /// </summary>
    private void LoadRoomsForSelectedPackage()
    {
        if (SelectedPackage != null)
        {
            RoomOptions = _resourceService.FetchRoomsByType(SelectedPackage.RoomType);
            OnPropertyChanged(nameof(RoomOptions));

            // Clear the previously selected room only if it's not matching the current package type
            if (SelectedRoom != null && !RoomOptions.Contains(SelectedRoom))
            {
                SelectedRoom = null;
                DeskNameEntry.Text = null;
            }
        }
    }

    /// <summary>
    /// Loads the associated desks for the selected room.
    /// </summary>
    private void LoadDesksForSelectedRoom()
    {
        // If there's no selected desk or the selected room has changed, clear the desk
        if (string.IsNullOrEmpty(SelectedDesk) || (SelectedRoom != null && SelectedDesk != PackagePaymentDetail.DeskName))
        {
            DeskNameEntry.Text = null;
        }
    }

    /// <summary>
    /// Handles the create package and payment information.
    /// </summary>
    /// <param name="sender">The create button that triggers this event.</param>
    /// <param name="e">The event argument passed on to execute this function.</param>
    private async void OnCreateClicked(object sender, EventArgs e)
    {
        // Update PackagePaymentDetail with the current values
        PackagePaymentDetail.Package = SelectedPackage;
        PackagePaymentDetail.Room = SelectedRoom;
        PackagePaymentDetail.DeskName = SelectedDesk;
        PackagePaymentDetail.StartDate = PackageStartDate;
        PackagePaymentDetail.EndDate = PackageEndDate;

        // TODO: Add validation logic here

        // TODO: Save the updated PackagePaymentDetail

        CustomAlert.ShowAlert("Success", "Package details saved successfully.", "OK");
    }

    /// <summary>
    /// Handles the desk select event for the package.
    /// </summary>
    /// <param name="sender">The desk entry that triggers the desk select event.</param>
    /// <param name="e">The pramaters needed to update the selected desk information.</param>
    private async void OnDeskSelectClicked(object sender, EventArgs e)
    {
        try
        {
            if (SelectedRoom is null)
            {
                await CustomAlert.ShowAlert("Error", "Please select room before selecting desks.", "OK");
                return;
            }

            SelectedRoom.IsSelectable = true;
            Func<RoomListViewModel, DeskSelectView> _deskLayoutFactory = _serviceProvider.GetService<Func<RoomListViewModel, DeskSelectView>>();
            var deskLayoutPopup = _deskLayoutFactory(SelectedRoom);
            deskLayoutPopup.DeskSelected += OnDeskSelected;
            await Application.Current.MainPage.ShowPopupAsync(deskLayoutPopup);
        }
        catch (Exception ex)
        {
            ExceptionHandler.HandleException("Selecting desks", ex);
        }
    }

    /// <summary>
    /// Handles the desk select event after the desks has been clicked. 
    /// </summary>
    /// <param name="sender">The subscribed event triggered through the desk select process.</param>
    /// <param name="deskName">The name of the desk passed as an select event carried out in popup.</param>
    private void OnDeskSelected(object sender, string deskName)
    {
        SelectedDesk = deskName;
        DeskNameEntry.Text = deskName;
    }

    /// <summary>
    /// Handles the clear click event to erase all the entered information.
    /// </summary>
    /// <param name="sender">The button that triggered the clear event.</param>
    /// <param name="e">The parameters needed to clear the form information.</param>
    private void OnClearClicked(object sender, EventArgs e)
    {
        if (IsEditable)
        {
            // Reset all fields to their default values
            SelectedPackage = null;
            SelectedRoom = null;
            SelectedDesk = null;
            PackageStartDate = DateTime.Today;
            PackageEndDate = DateTime.Today;
            DeskNameEntry.Text = null;
            // Reset other fields as necessary
            OnPropertyChanged(nameof(SelectedPackage));
            OnPropertyChanged(nameof(SelectedRoom));
            OnPropertyChanged(nameof(SelectedDesk));
            OnPropertyChanged(nameof(PackageStartDate));
            OnPropertyChanged(nameof(PackageEndDate));
        }
        else
        {
            CustomAlert.ShowAlert("Info", "Cannot clear details for existing packages.", "OK");
        }
    }
}
