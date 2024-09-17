using CommunityToolkit.Maui.Views;
using Microsoft.Extensions.Configuration;
using OwlReadingRoom.Components;
using OwlReadingRoom.Components.AlertDialog;
using OwlReadingRoom.Configurations;
using OwlReadingRoom.Services;
using OwlReadingRoom.Utils;
using OwlReadingRoom.ViewModels;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace OwlReadingRoom.Views.Resources.Package;

public partial class PackageListView : ContentView, INotifyPropertyChanged
{
    private bool _shouldShowEditButton;
    private bool _shouldShowTrashButton;
    private readonly IPackageService _packageService;
    private readonly IServiceProvider _serviceProvider;
    public event PropertyChangedEventHandler PropertyChanged;
    private ObservableCollection<PackageListViewModel> _packages;
    private readonly Func<PackageListViewModel, UpdatePackage> _updatePackageFactory;


    public PackageListView(IServiceProvider serviceProvider, IPackageService packageService, Func<PackageListViewModel, UpdatePackage> updatePackageFactory)
    {
        InitializeComponent();
        _serviceProvider = serviceProvider;
        _packageService = packageService;
        BindingContext = this;
        LoadPackageData();
        _updatePackageFactory = updatePackageFactory;
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

    public bool ShouldShowTrashButton
    {
        get => _shouldShowTrashButton;
        set
        {
            if (_shouldShowTrashButton != value)
            {
                _shouldShowTrashButton = value;
                OnPropertyChanged();
            }
        }
    }

    public ObservableCollection<PackageListViewModel> Packages
    {
        get => _packages;
        set
        {
            if (_packages != value)
            {
                _packages = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(HasPackages));
                OnPropertyChanged(nameof(IsEmptyState));
            }
        }
    }

    public bool HasPackages => Packages != null && Packages.Count > 0;
    public bool IsEmptyState => !HasPackages;

    /// <summary>
    /// Opens the modal pop up for new package creation.
    /// </summary>
    /// <param name="sender">The create package button that triggered the click event.</param>
    /// <param name="e">The event argument passed on to this function.</param>
    private void OnNewPackageButtonClicked(object sender, EventArgs e)
    {
        var newPackage = _serviceProvider.GetService<NewPackage>();
        newPackage.PackageCreated += OnPackageCreated;
        Application.Current.MainPage.ShowPopup(newPackage);
    }

    /// <summary>
    /// Handles the event for package load functionality once the new package is created.
    /// </summary>
    /// <param name="sender">The event handler that subscribed to this function.</param>
    /// <param name="e">The parameters provided to load packages.</param>
    private void OnPackageCreated(object sender, EventArgs e)
    {
        LoadPackageData();
    }

    /// <summary>
    ///  Loads the latest package information from the database.
    /// </summary>
    private void LoadPackageData()
    {
        try
        {
            #region Fetch actions flag from environment variables

            IConfiguration configuration = _serviceProvider.GetService<IConfiguration>();
            ActionFeatures actions = ConfigurationHandler.GetActionFeatures(configuration, "PackageActions");
            ShouldShowEditButton = actions.Edit;
            ShouldShowTrashButton = actions.Delete;

            #endregion
            Packages = new ObservableCollection<PackageListViewModel>(_packageService.GetPackageList());
        }
        catch (Exception ex)
        {
            ExceptionHandler.HandleException("Loading packages", ex);
        }
    }

    /// <summary>
    /// Handles the package edit functionality.
    /// </summary>
    /// <param name="sender">The button that opens the update package popup.</param>
    /// <param name="e">The paramters sent on this event for package update functionality.</param>
    private void OnPackageEditClicked(object sender, EventArgs e)
    {
        try
        {
            var button = sender as ActionButtonsView;
            var package = button?.BindingContext as PackageListViewModel;
            if (package != null)
            {
                //TODO: Open popup dialog for update
                var editPackagePopup = _updatePackageFactory(package);
                editPackagePopup.PackageUpdated += OnPackageUpdated;
                Application.Current.MainPage.ShowPopup(editPackagePopup);
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.HandleException("Preparing package for update", ex);
        }

    }

    /// <summary>
    /// Handles the package deletion functionality.
    /// </summary>
    /// <param name="sender">The button that triggers the deletion function in the backend.</param>
    /// <param name="e">The arguments passed on to this delete event.</param>
    private async void OnPackageTrashClicked(object sender, EventArgs e)
    {
        var button = sender as ActionButtonsView;
        var package = button?.BindingContext as PackageListViewModel;
        if (package != null)
        {
            var deletePackagePopup = new DeletePackageDialog(package);
            deletePackagePopup.DeleteConfirmed += OnDeletePackageConfirmed;
            await Application.Current.MainPage.ShowPopupAsync(deletePackagePopup);
        }
    }

    /// <summary>
    /// Handles the confirmation of package deletion.
    /// </summary>
    /// <param name="sender">The button that confirmed the package deletion.</param>
    /// <param name="package">The required parameters passed on to the event for package deletion.</param>
    private async void OnDeletePackageConfirmed(object sender, PackageListViewModel package)
    {
        try
        {
            //TODO: Delete Package from Database
            LoadPackageData();
            AlertService.Instance.ShowAlert("Success", "Package deleted successfully.", AlertType.Success);
        }
        catch (Exception ex)
        {
            ExceptionHandler.HandleException("Deleting package", ex);
        }
    }

    /// <summary>
    /// Handles the post package update context, by reloading the package data from the database.
    /// </summary>
    /// <param name="sender">The vent handler that subscribed to this event.</param>
    /// <param name="e">The parameters sent out to this function as a part of event subscription.</param>
    private void OnPackageUpdated(object sender, EventArgs e)
    {
        LoadPackageData();
    }

    /// <summary>
    /// Handles the property change event provided by INotifyPropertyChanged manager.
    /// </summary>
    /// <param name="propertyName">The property whose value has been changed.</param>
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
