using OwlReadingRoom.Events;
using OwlReadingRoom.ViewModels;

namespace OwlReadingRoom.Views.Customer;

public partial class PackagePaymentDetailView : ContentView
{
    public event EventHandler<PackagePaymentSavedEventArgs> PackagePaymentDetailSaved;
    public PackagePaymentDetailView()
    {
        InitializeComponent();
    }

    private async void OnCreateClicked(object sender, EventArgs e)
    {
        //TODO: Update any necessary info from form 
        //TODO: Update CustomerPackageViewModel accordingly.
    }

    private async void OnMedicalInfoClicked(object sender, EventArgs e)
    {
        //TODO: Update any necessary info from form 
        //TODO: Update CustomerPackageViewModel accordingly.
        CustomerPackageViewModel updatedModel = new CustomerPackageViewModel();
        PackagePaymentDetailSaved?.Invoke(this, new PackagePaymentSavedEventArgs(updatedModel));
    }
}
