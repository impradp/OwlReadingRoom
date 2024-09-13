using OwlReadingRoom.Components.AlertDialog;
using OwlReadingRoom.ViewModels;

namespace OwlReadingRoom.Views.Customer;

public partial class PackagePaymentDetailView : ContentView
{
    private CustomerDetailViewModel _customer;
    public PackagePaymentDetailView(CustomerDetailViewModel customer)
    {
        InitializeComponent();
        _customer = customer;
    }

    private async void OnCreateClicked(object sender, EventArgs e)
    {
        //TODO: Update any necessary info from form 
        //TODO: Update CustomerPackageViewModel accordingly.
        AlertService.Instance.ShowAlert("Success", "Package details saved successfully.");
    }

    private async void OnMedicalInfoClicked(object sender, EventArgs e)
    {
        //TODO: Update any necessary info from form 
        //TODO: Update CustomerPackageViewModel accordingly.
        AlertService.Instance.ShowAlert("Success", "Medical details saved successfully.");
    }
}
