using CommunityToolkit.Maui.Views;
using OwlReadingRoom.Components.AlertDialog;
using OwlReadingRoom.DTOs;
using OwlReadingRoom.Services;
using OwlReadingRoom.Utils;

namespace OwlReadingRoom.Views.Customer;

public partial class NewCustomer : Popup
{
    private readonly IBookingService _bookingService;
    public event EventHandler<EventArgs> CustomerPackageSaved;
    public NewCustomer(IPackageService packageService, IBookingService bookingService)
    {
        InitializeComponent();
        BindingContext = this;
        _bookingService = bookingService;
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
            CreateButton.IsEnabled = false;

            if (Validator.IsValidNewCustomer(FullNameEntry.Text, ContactNumberEntry.Text, false, null, null, ""))
            {
                _bookingService.PerformInitialBooking(new MinimumCustomerDetail
                {
                    FullName = FullNameEntry.Text,
                    ContactNumber = String.Format("+977-{0}", ContactNumberEntry.Text),
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
        finally
        {
            CreateButton.IsEnabled = true;
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
            // Only allow digits (removes any other characters)
            string filteredText = new string(e.NewTextValue.Where(ch => char.IsDigit(ch)).ToArray());

            // Check if the filtered text has more than 10 digits
            if (filteredText.Length > 10)
            {
                // If more than 10 digits, revert to old value
                ContactNumberEntry.Text = e.OldTextValue;
            }
            else if (filteredText != e.NewTextValue)
            {
                // If the filtered text is different from input (meaning non-numeric chars were removed)
                ContactNumberEntry.Text = filteredText;
            }
        }
    }

}
