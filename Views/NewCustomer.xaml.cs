using CommunityToolkit.Maui.Views;
using System.IO;
using Microsoft.Maui.Storage;
using System.Windows.Input;
using System.Net;
using OwlReadingRoom.Utils;
using OwlReadingRoom.Models;
using OwlReadingRoom.Services.Database;
using OwlReadingRoom.Services.Repository;

namespace OwlReadingRoom.Views;

public partial class NewCustomer : Popup
{

    public ICommand UploadCommand { get; private set; }
    private IRepository<PersonalDetail> _personalDetailService;

    public NewCustomer(IRepository<PersonalDetail> personalDetailService)
    {
        InitializeComponent();
        _personalDetailService = personalDetailService;
        UploadCommand = new Command(async () => await ExecuteUploadCommand());
        BindingContext = this;
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
            await CustomAlert.ShowAlert("Error", $"An error occurred: {ex.Message}", "OK");
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
                //TODO: Validate unique mobile number
                string sourceFilePath = FilePathEntryFullPath.Text;
                string customerFolderName = ContactNumberEntry.Text.Replace(" ", "").Replace("+", "").Replace("-", "");
                string destinationFolderPath = Path.Combine(FileSystem.AppDataDirectory, "Assets", "Documents", customerFolderName);

                // Create the customer's folder if it doesn't exist
                if (!Directory.Exists(destinationFolderPath))
                {
                    Directory.CreateDirectory(destinationFolderPath);
                }

                string fileName = Path.GetFileName(sourceFilePath);
                string destinationFilePath = Path.Combine(destinationFolderPath, fileName);

                // Copy the file
                File.Copy(sourceFilePath, destinationFilePath, true);

                //TODO: Create customer object
                PersonalDetail personalDetail = new PersonalDetail();
                personalDetail.FullName = FullNameEntry.Text;
                int personalDetailId = await _personalDetailService.SaveItemAsync(personalDetail);

                await CloseAsync();

                //TODO: Redirect to package detail update and payment selection page
            }

        }
        catch (Exception ex)
        {
            await CustomAlert.ShowAlert("Error", $"An error occurred while saving customer details: {ex.Message}", "OK");
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

}
