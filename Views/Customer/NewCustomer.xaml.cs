using CommunityToolkit.Maui.Views;
using OwlReadingRoom.Events;
using OwlReadingRoom.Models;
using OwlReadingRoom.Services;
using OwlReadingRoom.Utils;
using OwlReadingRoom.ViewModels;
using System.Windows.Input;
using OwlReadingRoom.DTOs;

namespace OwlReadingRoom.Views.Customer;

public partial class NewCustomer : Popup
{
    public ICommand UploadCommand { get;  set; }
    private readonly IPackageService _packageService;
    private readonly IBookingService _bookingService;
    public List<PackageType> PackageTypes { get; set; }
    public PackageType SelectedPackage { get; set; }
    public event EventHandler<CustomerSavedEventArgs> CustomerPackageSaved;
    public NewCustomer(IPackageService packageService, IBookingService bookingService)
    {
        InitializeComponent();
        _packageService = packageService;
        LoadPackageTypes();
        UploadCommand = new Command(async () => await ExecuteUploadCommand());
        BindingContext = this;
        _bookingService = bookingService;
    }

    private void LoadPackageTypes()
    {
        PackageTypes = _packageService.GetPackages();
    }

    private async Task ExecuteUploadCommand()
    {
        try
        {
            FileResult? result = await FilePicker.PickAsync(new PickOptions
            {
                PickerTitle = "Please select a file",
                FileTypes = FilePickerFileType.Images
            });

            if (Validator.IsValidDocument(result))
            {
                FilePathEntry.Text = result.FileName;
                FilePathEntryFullPath.Text = result.FullPath;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.HandleException("Uploading document", ex);
        }
    }

    private void OnPackageTypeSelectedIndexChanged(object sender, EventArgs e)
    {
        PackageTypeLabel.IsVisible = PackageTypePicker.SelectedIndex == -1;
    }

    private void OnPaymentTypeSelectedIndexChanged(object sender, EventArgs e)
    {
        PaymentTypeLabel.IsVisible = PaymentTypePicker.SelectedIndex == -1;
    }

    private void OnCloseClicked(object sender, EventArgs e)
    {
        Close();
    }

    private void OnClearClicked(object sender, EventArgs e)
    {
        // Clear all inputs
        FullNameEntry.Text = string.Empty;
        ContactNumberEntry.Text = string.Empty;
        PackageTypePicker.SelectedIndex = -1;
        FilePathEntry.Text = string.Empty;
        FilePathEntryFullPath.Text = string.Empty;
        AddressEditor.Text = string.Empty;
        PaymentTypePicker.SelectedIndex = -1;

        // Reset labels if necessary
        PackageTypeLabel.Text = "Select";
        PaymentTypeLabel.Text = "Select";
    }

    private async void OnCreateClicked(object sender, EventArgs e)
    {
        try
        {
            if (Validator.IsValidNewCustomer(FullNameEntry.Text, ContactNumberEntry.Text, PackageTypePicker.SelectedIndex, PaymentTypePicker.SelectedIndex, FilePathEntryFullPath.Text))
            {
                string customerFolderName = SanitizeCustomerFolderName(ContactNumberEntry.Text);
                string destinationFolderPath = Path.Combine(FileSystem.AppDataDirectory, "Assets", "Documents", customerFolderName);

                EnsureDirectoryExists(destinationFolderPath);

                string documentFilePath = CopyFileToCustomerFolder(FilePathEntryFullPath.Text, destinationFolderPath);

                //TODO: Create customer object
                _bookingService.RegisterWithMinimumDetails(new MinimumCustomerDetail
                {
                    FullName = FullNameEntry.Text,
                    DocumentPath = documentFilePath,
                    ContactNumber = ContactNumberEntry.Text,
                    PackageType = SelectedPackage.Id,
                    PaymentType = PaymentTypePicker.SelectedIndex
                });

                CustomerPackageViewModel savedCustomerPackage = new CustomerPackageViewModel();

                await CloseAsync();

                //TODO: Redirect to package detail update and payment selection page
                CustomerPackageSaved?.Invoke(this, new CustomerSavedEventArgs(savedCustomerPackage));
            }

        }
        catch (Exception ex)
        {
            ExceptionHandler.HandleException("Saving customer details", ex);
        }
    }

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


    private string SanitizeCustomerFolderName(string contactNumber)
    {
        return contactNumber.Replace(" ", "").Replace("+", "").Replace("-", "");
    }

    private void EnsureDirectoryExists(string directoryPath)
    {
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }
    }

    private string CopyFileToCustomerFolder(string sourceFilePath, string destinationFolderPath)
    {
        string fileName = Path.GetFileName(sourceFilePath);
        string destinationFilePath = Path.Combine(destinationFolderPath, fileName);

        File.Copy(sourceFilePath, destinationFilePath, true);

        return destinationFilePath;
    }

}
