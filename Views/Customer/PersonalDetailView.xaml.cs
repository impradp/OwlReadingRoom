using OwlReadingRoom.Components.AlertDialog;
using OwlReadingRoom.Services;
using OwlReadingRoom.Services.Exceptions;
using OwlReadingRoom.Utils;
using OwlReadingRoom.ViewModels;

namespace OwlReadingRoom.Views.Customer;

public partial class PersonalDetailView : ContentView
{
    private readonly PersonalDetailEditViewModel _viewModel;
    private readonly ICustomerService _customerService;
    public PersonalDetailView(PersonalDetailEditViewModel viewModel, ICustomerService customerService)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
        this._customerService = customerService;
    }

    private async void OnCreateClicked(object sender, EventArgs e)
    {
        try
        {
            CreateButton.IsEnabled = false;
            _customerService.UpdateCustomer(_viewModel);
            AlertService.Instance.ShowAlert("Success", "Customer details saved successfully.");
        }
        catch (DuplicateEntryException exe)
        {
            await CustomAlert.ShowAlert("Error", exe.Message, "OK");
        }
        catch (Exception ex)
        {
            ExceptionHandler.HandleException("updating personal details", ex);
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
