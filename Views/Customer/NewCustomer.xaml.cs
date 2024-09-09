using CommunityToolkit.Maui.Views;
using OwlReadingRoom.Components.AlertDialog;
using OwlReadingRoom.DTOs;
using OwlReadingRoom.Models;
using OwlReadingRoom.Services;
using OwlReadingRoom.Utils;

namespace OwlReadingRoom.Views.Customer;

public partial class NewCustomer : Popup
{
    private readonly IPackageService _packageService;
    private readonly IBookingService _bookingService;
    public List<PackageType> PackageTypes { get; set; }
    public event EventHandler<EventArgs> CustomerPackageSaved;
    public NewCustomer(IPackageService packageService, IBookingService bookingService)
    {
        InitializeComponent();
        _packageService = packageService;
        LoadPackageTypes();
        BindingContext = this;
        _bookingService = bookingService;
    }

    /// <summary>
    /// Loads the packages saved in the system.
    /// </summary>
    private void LoadPackageTypes()
    {
        PackageTypes = _packageService.GetPackages();
    }

    /// <summary>
    /// Handles the modal popup close for new customer creation.
    /// </summary>
    /// <param name="sender">The close button that trigger this action event.</param>
    /// <param name="e">The event arguments passed into this function to gracefully close the modal popup.</param>
    private void OnCloseClicked(object sender, EventArgs e)
    {
        Close();
    }

    /// <summary>
    /// Handles the function to clear values for each field in new customer creation modal popup.
    /// </summary>
    /// <param name="sender">The clear button that trigger this action event.</param>
    /// <param name="e">The event arguments passed into this function to gracefully clear the modal popup contents.</param>
    private void OnClearClicked(object sender, EventArgs e)
    {
        // Clear all inputs
        FullNameEntry.Text = string.Empty;
        ContactNumberEntry.Text = string.Empty;
        AddressEditor.Text = string.Empty;
    }

    /// <summary>
    /// Handles the creation of new customer.
    /// </summary>
    /// <param name="sender">The save button that trigger this creation event.</param>
    /// <param name="e">The event arguments passed into this function to save the new customer.</param>
    private async void OnCreateClicked(object sender, EventArgs e)
    {
        try
        {
            if (Validator.IsValidNewCustomer(FullNameEntry.Text, ContactNumberEntry.Text, false, null, null, ""))
            {
                //TODO: Create customer object
                _bookingService.RegisterWithMinimumDetails(new MinimumCustomerDetail
                {
                    FullName = FullNameEntry.Text,
                    ContactNumber = ContactNumberEntry.Text,
                    CurrentAddress = AddressEditor.Text
                });

                AlertService.Instance.ShowAlert("Success", "New customer registered successfully.", AlertType.Success);
                CustomerPackageSaved?.Invoke(this, EventArgs.Empty);
                await CloseAsync();
            }

        }
        catch (Exception ex)
        {
            ExceptionHandler.HandleException("Saving customer details", ex);
        }
    }

    /// <summary>
    /// Handles the validation aof contact number during customer registration.
    /// </summary>
    /// <param name="sender">The key change event that triggers this function.</param>
    /// <param name="e">The event argument passed on to this function for the validation of customer's contact number.</param>
    private void OnContactNumberTextChanged(object sender, TextChangedEventArgs e)
    {
        if (e.NewTextValue != null)
        {
            string filteredText = new string(e.NewTextValue.Where(ch => char.IsDigit(ch) || ch == '+' || ch == '-').ToArray());

            int digitCount = filteredText.Count(ch => char.IsDigit(ch));

            if (digitCount > 13 || filteredText.Length > 15)
            {
                ContactNumberEntry.Text = e.OldTextValue;
            }
            else if (filteredText != e.NewTextValue)
            {
                ContactNumberEntry.Text = filteredText;
            }
        }
    }

    /// <summary>
    /// Prepares the customer folder name in the AppData to store the documents of selected customer.
    /// </summary>
    /// <param name="contactNumber">The contact number with which the customer folder is created.</param>
    /// <returns></returns>
    private string SanitizeCustomerFolderName(string contactNumber)
    {
        return contactNumber.Replace(" ", "").Replace("+", "").Replace("-", "");
    }

    /// <summary>
    /// Validates the existence or else creates the new folder for customer document storage.
    /// </summary>
    /// <param name="directoryPath"></param>
    private void EnsureDirectoryExists(string directoryPath)
    {
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }
    }

    /// <summary>
    /// Copies the original file to customer folder to be readily used by the application.
    /// </summary>
    /// <param name="sourceFilePath">The original path of the file in the machine.</param>
    /// <param name="destinationFolderPath">The folder path in the application's AppData directory.</param>
    /// <returns></returns>
    private string CopyFileToCustomerFolder(string sourceFilePath, string destinationFolderPath)
    {
        string fileName = Path.GetFileName(sourceFilePath);
        string destinationFilePath = Path.Combine(destinationFolderPath, fileName);

        File.Copy(sourceFilePath, destinationFilePath, true);

        return destinationFilePath;
    }

}
