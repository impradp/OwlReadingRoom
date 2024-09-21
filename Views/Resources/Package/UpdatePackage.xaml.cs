using CommunityToolkit.Maui.Views;
using OwlReadingRoom.Components.AlertDialog;
using OwlReadingRoom.Models;
using OwlReadingRoom.Services;
using OwlReadingRoom.Services.Resources;
using OwlReadingRoom.Utils;
using OwlReadingRoom.ViewModels;
using System.Collections.ObjectModel;

namespace OwlReadingRoom.Views.Resources.Package;

public partial class UpdatePackage : Popup
{
    
    public event EventHandler PackageUpdated;
    private readonly IPackageService _packageService;
    private PackageListViewModel _package;
    public ObservableCollection<RoomType> RoomTypes { get; set; }
    public PackageListViewModel Package
    {
        get => _package;
        set
        {
            _package = value;
            OnPropertyChanged(nameof(Package));
        }
    }

    public UpdatePackage(PackageListViewModel package, IPackageService packageService)
    {
        InitializeComponent();
        RoomTypes = new ObservableCollection<RoomType>(Enum.GetValues(typeof(RoomType)).Cast<RoomType>());
        Package = package;
        _packageService = packageService;
        BindingContext = this;

    }

    /// <summary>
    /// Handles the close event functionality.
    /// </summary>
    /// <param name="sender">The button that subscibed to this close event functionality.</param>
    /// <param name="e">The event parameters sent into this function by event manager.</param>
    private void OnCloseClicked(object sender, EventArgs e)
    {
        Close();
    }

    /// <summary>
    /// Handles the clear event functionality.
    /// </summary>
    /// <param name="sender">The button that subscibed to this clear event functionality.</param>
    /// <param name="e">The event parameters sent into this function by event manager.</param>
    private void OnClearClicked(object sender, EventArgs e)
    {
        NameEntry.Text = "";
    }

    /// <summary>
    /// Handles the update click event functionality.
    /// </summary>
    /// <param name="sender">The button that subscibed to this update event functionality.</param>
    /// <param name="e">The event parameters sent into this function by event manager.</param>
    private async void OnUpdateClicked(object sender, EventArgs e)
    {
        try
        {
            _packageService.UpdatePackage(Package);
            
            AlertService.Instance.ShowAlert("Info", "Package updated successfully.", AlertType.Info);
            PackageUpdated?.Invoke(this, EventArgs.Empty);
            await CloseAsync();
        }
        catch (Exception ex)
        {
            ExceptionHandler.HandleException("Updating new package details", ex);
        }
    }
}
