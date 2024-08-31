using OwlReadingRoom.Components.AlertDialog;
using OwlReadingRoom.ViewModels;

namespace OwlReadingRoom.Views.Customer;

public partial class PersonalDetailView : ContentView
{
    private CustomerPackageViewModel _customer;
    public PersonalDetailView(CustomerPackageViewModel customer)
    {
        InitializeComponent();
        _customer = customer;
    }

    private async void OnCreateClicked(object sender, EventArgs e)
    {
        //TODO: Update any necessary info from form 
        //TODO: Update CustomerPackageViewModel accordingly.
        AlertService.Instance.ShowAlert("Success", "Customer details saved successfully.");
    }

}
