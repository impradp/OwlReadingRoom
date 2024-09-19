using OwlReadingRoom.Components.AlertDialog;
using OwlReadingRoom.Services;
using OwlReadingRoom.Services.Exceptions;
using OwlReadingRoom.Utils;
using OwlReadingRoom.ViewModels;

namespace OwlReadingRoom.Views.Customer;

public partial class PersonalDetailView : ContentView
{
    private PersonalDetailEditViewModel _viewModel;
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

    }

}
