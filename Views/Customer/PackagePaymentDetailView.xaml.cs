using CommunityToolkit.Maui.Views;
using OwlReadingRoom.Components.AlertDialog;
using OwlReadingRoom.Models;
using OwlReadingRoom.Services;
using OwlReadingRoom.Services.Booking;
using OwlReadingRoom.Services.Resources;
using OwlReadingRoom.Services.Transactions;
using OwlReadingRoom.Utils;
using OwlReadingRoom.ViewModels;
using OwlReadingRoom.Views.Resources.Rooms;
using System.Diagnostics;

namespace OwlReadingRoom.Views.Customer;

public partial class PackagePaymentDetailView : ContentView
{
    public string SelectedDesk { get; set; }
    public List<RoomListViewModel> RoomOptions { get; private set; }
    public List<RoomListViewModel> ACRoomOptions { get; private set; }
    public List<RoomListViewModel> NonACRoomOptions { get; private set; }
    public List<PackageListViewModel> PackageOptions { get; private set; }

    private PackageListViewModel _selectedPackage;
    private RoomListViewModel _selectedRoom;
    private DateTime? _packageStartDate;
    private DateTime? _packageEndDate;
    private bool _hasExistingPackage;
    private IPhysicalResourceService _resourceService;
    private readonly IServiceProvider _serviceProvider;
    private readonly IBookingService _bookingService;
    private readonly IBookingProcessor _bookingProcessor;
    private readonly IDeskService _deskService;
    private readonly IPackageService _packageService;
    private readonly ITransactionService _transactionService;

    public PackageAndPaymentEditViewModel PackagePaymentDetail { get; private set; }
    public PackagePaymentDetailView(PackageAndPaymentEditViewModel packageAndPaymentDetail,
        IPhysicalResourceService resourceService, IServiceProvider serviceProvider)
    {
        InitializeComponent();
        PackagePaymentDetail = packageAndPaymentDetail;
        _resourceService = resourceService;
        _serviceProvider = serviceProvider;
        _bookingService = _serviceProvider.GetService<IBookingService>();
        _deskService = _serviceProvider.GetService<IDeskService>();
        _packageService = _serviceProvider.GetService<IPackageService>();
        _bookingProcessor = _serviceProvider.GetService<IBookingProcessor>();
        _transactionService = _serviceProvider.GetService<ITransactionService>();
        LoadResources();
        LoadExistingData();
        BindingContext = this;
    }

    /// <summary>
    /// Loads all the resources like packages, rooms and underlying desks for selection.
    /// </summary>
    private void LoadResources()
    {
        PackageOptions = _packageService.GetPackageList();
        ACRoomOptions = _resourceService.FetchRoomsByType(RoomType.AC);
        NonACRoomOptions = _resourceService.FetchRoomsByType(RoomType.NON_AC);
        PackageStartDate = DateTime.Now.Date;
        OnPropertyChanged(nameof(PackageOptions));
    }

    /// <summary>
    /// Loads the existing booking and package data into the view model.
    /// Retrieves booking details, package, desk, room, and transaction information 
    /// and updates the UI fields with the retrieved data.
    /// </summary>
    private void LoadExistingData()
    {
        var bookingInformation = _bookingService.GetBookingDetailsById(PackagePaymentDetail.Id);

        // Check if Package is assigned to the booking
        if (bookingInformation?.PackageId == null)
        {
            Debug.WriteLine($"Package not assigned for booking Id: {bookingInformation?.Id}");
            return;
        }

        // Load or create the selected package
        LoadPackage(bookingInformation.PackageId);

        // Load the selected desk and room
        LoadDeskAndRoom(bookingInformation.DeskId);

        // Set package start and end dates
        PackageStartDate = bookingInformation.ReservationStartDate ?? DateTime.Today;
        PackageEndDate = bookingInformation.ReservationEndDate ?? DateTime.Today.AddDays(SelectedPackage?.Days ?? 0);

        // Load transaction details
        LoadTransactionDetails(bookingInformation.Id);

        HasExistingPackage = true;
    }

    /// <summary>
    /// Loads the package associated with the given packageId.
    /// If the package is not found in the existing package options, retrieves it 
    /// from the package service, marks it as non-selectable, and adds it to the list.
    /// </summary>
    /// <param name="packageId">The ID of the package to be loaded.</param>
    private void LoadPackage(int? packageId)
    {
        SelectedPackage = PackageOptions.FirstOrDefault(p => p.Id == packageId);

        if (SelectedPackage == null && packageId.HasValue)
        {
            var packageListViewModel = _packageService.GetPackageListViewModelById(packageId.Value);
            packageListViewModel.IsSelectable = false;
            PackageOptions.Add(packageListViewModel);
            SelectedPackage = packageListViewModel;
        }
    }

    /// <summary>
    /// Loads the desk and room information based on the given deskId.
    /// Retrieves the desk details, then selects the corresponding room 
    /// from the available room options.
    /// </summary>
    /// <param name="deskId">The ID of the desk to be loaded.</param>
    private void LoadDeskAndRoom(int? deskId)
    {
        var desk = _deskService.GetDesk(deskId);
        if (desk == null)
        {
            Debug.WriteLine($"Desk not found for DeskId: {deskId}");
            return;
        }

        SelectedRoom = RoomOptions.FirstOrDefault(r => r.Id == desk.RoomId);
        SelectedDesk = desk.Name;
    }

    /// <summary>
    /// Loads the transaction details associated with the given bookingId.
    /// Retrieves locker and parking amounts and assigns them to the payment details.
    /// </summary>
    /// <param name="bookingId">The ID of the booking for which transaction details are loaded.</param>

    private void LoadTransactionDetails(int bookingId)
    {
        var transactionDetailViewModel = _transactionService.GetTransactionDetails(bookingId);
        if (transactionDetailViewModel != null)
        {
            PackagePaymentDetail.LockerAmount = transactionDetailViewModel.LockerAmount;
            PackagePaymentDetail.ParkingAmount = transactionDetailViewModel.ParkingAmount;
        }
    }

    #region Event Handler
    public PackageListViewModel SelectedPackage
    {
        get => _selectedPackage;
        set
        {
            if (_selectedPackage != value)
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
            if (_selectedRoom != value)
            {
                _selectedRoom = value;
                OnPropertyChanged();
                SelectedDesk = null;
                OnPropertyChanged(nameof(SelectedDesk));
            }
        }
    }

    public DateTime? PackageStartDate
    {
        get => _packageStartDate;
        set
        {
            if (_packageStartDate != value)
            {
                _packageStartDate = value;
                OnPropertyChanged();
                UpdatePackageEndDate();
            }
        }
    }
    public DateTime? PackageEndDate
    {
        get => _packageEndDate;
        set
        {
            if (_packageEndDate != value)
            {
                _packageEndDate = value;
                OnPropertyChanged();
            }

        }
    }
    #endregion

    public bool HasExistingPackage
    {
        get => _hasExistingPackage;
        set
        {
            if (_hasExistingPackage != value)
            {
                _hasExistingPackage = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsEditable));
            }
        }
    }

    public bool IsEditable => !HasExistingPackage;

    /// <summary>
    /// Updates the package end date upon package selection
    /// </summary>
    private void UpdatePackageEndDate()
    {
        if (SelectedPackage != null)
        {
            PackageEndDate = PackageStartDate?.AddDays(SelectedPackage.Days - 1);
        }
    }


    /// <summary>
    /// Loads the associated rooms for selected package.
    /// </summary>
    private void LoadRoomsForSelectedPackage()
    {
        if (SelectedPackage != null)
        {
            RoomOptions = RoomType.AC.Equals(SelectedPackage.RoomType) ? ACRoomOptions : NonACRoomOptions;

            // Clear the previously selected room only if it's not matching the current package type
            if (SelectedRoom != null && !RoomOptions.Contains(SelectedRoom))
            {
                SelectedRoom = null;
            }
            OnPropertyChanged(nameof(RoomOptions));
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
        PackagePaymentDetail.PackageStartDate = PackageStartDate;
        PackagePaymentDetail.PackageEndDate = PackageEndDate;

        try
        {
            if (Validator.isValidBooking(SelectedDesk, SelectedRoom))
            {
                _bookingProcessor.ProcessBooking(PackagePaymentDetail);

                AlertService.Instance.ShowAlert("Success", "Package details saved successfully.", AlertType.Success);
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.HandleException("booking a package for customer.", ex);
        }
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
            if (IsEditable)
            {
                SelectedRoom.IsSelectable = true;
            }

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

    private void OnCustomerSelected(object sender, EventArgs e)
    {
        var picker = sender as Picker;
        var selectedPackage = picker.SelectedItem as PackageListViewModel;

        if (selectedPackage != null && !selectedPackage.IsSelectable)
        {
            CustomAlert.ShowAlert("Error", $"{selectedPackage.Name} is not selectable.", "OK");
            picker.SelectedItem = null; // Reset selection
        }
    }
}
