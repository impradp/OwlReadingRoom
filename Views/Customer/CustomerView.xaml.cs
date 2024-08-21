using OwlReadingRoom.ViewModels;
using System.Collections.ObjectModel;

namespace OwlReadingRoom.Views.Customer;

public partial class CustomerView : ContentView
{
    public ObservableCollection<CustomerPackageViewModel> Customers { get; set; }
    public CustomerView(ObservableCollection<CustomerPackageViewModel> customers)
    {
        InitializeComponent();
        Customers = customers;
        BindingContext = this;
    }
}
