using CommunityToolkit.Maui.Views;
using OwlReadingRoom.Components.AlertDialog;
using OwlReadingRoom.Services.Resources;
using OwlReadingRoom.Utils;
using OwlReadingRoom.ViewModels;

namespace OwlReadingRoom.Views.Resources.Package;

public partial class UpdatePackage : Popup
{
    public PackageListViewModel Package;
    public event EventHandler PackageUpdated;
    private readonly IPhysicalResourceService _resourceService;

    public UpdatePackage(PackageListViewModel package, IPhysicalResourceService resourceService)
    {
        InitializeComponent();
        Package = package;
        BindingContext = this;
        PopulateFields();
        _resourceService = resourceService;

    }

    /// <summary>
    /// Populates the package details in the modal poup.
    /// </summary>
    private void PopulateFields()
    {
        IdEntry.Text = Package.Id.ToString();
        NameEntry.Text = Package.Name;
        RoomTypeEntry.Text = Package.RoomType;
        AmountEntry.Text = Package.Price.ToString();
        DaysEntry.Text = Package.Days.ToString();
        EditPackageHeaderLabel.Text = String.Concat("Edit Package #", Package.Id);
        // Populate other fields as needed
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
            //TODO: Validate And Update
            await CloseAsync();
            AlertService.Instance.ShowAlert("Info", "Package updated successfully.", AlertType.Info);
        }
        catch (Exception ex)
        {
            ExceptionHandler.HandleException("Updating new package details", ex);
        }
    }
}
