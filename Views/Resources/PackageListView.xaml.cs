using CommunityToolkit.Maui.Views;
using OwlReadingRoom.Components;
using OwlReadingRoom.Models;
using OwlReadingRoom.ViewModels;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace OwlReadingRoom.Views.Resources;

public partial class PackageListView : ContentView, INotifyPropertyChanged
{
    private ObservableCollection<PackageListViewModel> _packages;

    public PackageListView()
    {
        InitializeComponent();
        BindingContext = this;
        LoadPackageData();
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

    private void OnNewPackageButtonClicked(object sender, EventArgs e)
    {
        var newPackage = new NewPackage();
        newPackage.PackageCreated += OnPackageCreated;
        Application.Current.MainPage.ShowPopup(newPackage);
    }

    private void OnPackageCreated(object sender, EventArgs e)
    {
        LoadPackageData();
    }

    private void LoadPackageData()
    {
        //TODO: Fetch created packages list
        //TODO: Set to Observable Collection of Packages
    }

    private void OnPackageEditClicked(object sender, EventArgs e)
    {
        var button = sender as ActionButtonsView;
        var package = button?.BindingContext as PackageType;
        if (package != null)
        {
            //TODO: Open popup dialog for update
        }
    }

    private void OnPackageEyeClicked(object sender, EventArgs e)
    {
        var button = sender as ActionButtonsView;
        var package = button?.BindingContext as PackageType;
        if (package != null)
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
