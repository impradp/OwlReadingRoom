using CommunityToolkit.Maui.Views;
using OwlReadingRoom.Components.AlertDialog;
using OwlReadingRoom.Models;
using OwlReadingRoom.Services;
using OwlReadingRoom.Utils;
using System.Collections.ObjectModel;

namespace OwlReadingRoom.Views.Resources.Package;

public partial class NewPackage : Popup
{
    public event EventHandler<EventArgs> PackageCreated;

    private readonly IPackageService _packageService;

    public ObservableCollection<RoomType> RoomTypes { get; set; }
    public NewPackage(IPackageService packageService)
    {
        InitializeComponent();
        RoomTypes = new ObservableCollection<RoomType>(Enum.GetValues(typeof(RoomType)).Cast<RoomType>());
        BindingContext = this;
        _packageService = packageService;
    }

    private void OnRoomTypeSelectedIndexChanged(object sender, EventArgs e)
    {
        RoomTypeLabel.IsVisible = RoomTypePicker.SelectedIndex == -1;
    }

    private void OnCloseClicked(object sender, EventArgs e)
    {
        Close();
    }

    private void OnClearClicked(object sender, EventArgs e)
    {

        RoomTypeLabel.Text = "Select";
        RoomTypePicker.SelectedIndex = -1;
        PackageName.Text = "";
        AmountEntry.Text = "";
        DaysEntry.Text = "";
    }

    private async void OnCreateClicked(object sender, EventArgs e)
    {
        try
        {
            CreateButton.IsEnabled = false;
            if (Validator.isValidPackage(PackageName.Text, DaysEntry.Text, AmountEntry.Text, RoomTypePicker.SelectedIndex))
            {
                _packageService.SavePackage(new PackageType
                {
                    Days = Int32.Parse(DaysEntry.Text),
                    Name = PackageName.Text,
                    Price = Double.Parse(AmountEntry.Text),
                    RoomType = (RoomType)RoomTypePicker.SelectedItem
                });
                PackageCreated?.Invoke(this, EventArgs.Empty);
                await CloseAsync();
                AlertService.Instance.ShowAlert("Success", "New package created successfully.", AlertType.Success);
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.HandleException("Saving mew room details", ex);
        }
        finally
        {
            CreateButton.IsEnabled = true;
        }
    }

}
