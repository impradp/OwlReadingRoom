using OwlReadingRoom.Events;
using OwlReadingRoom.ViewModels;
using System.ComponentModel;

namespace OwlReadingRoom.Views.Customer;

public partial class CustomerDetailsView : ContentView, INotifyPropertyChanged
{
    public event EventHandler<CustomerSavedEventArgs> CustomerUpdateSelected;
    public event EventHandler<CustomerSavedEventArgs> CustomerReceiptSelected;
    private CustomerPackageViewModel _customer;
    private CustomerDetailViewModel Customer { get; set; }
    public CustomerDetailsView(CustomerPackageViewModel customer)
    {
        InitializeComponent();
        BindingContext = this;
        _customer = customer;
    }

    public CustomerPackageViewModel CustomerPackage
    {
        get => _customer;
        set
        {
            if (_customer != value)
            {
                _customer = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(HasDocuments));
                OnPropertyChanged(nameof(IsEmptyState));
            }
        }
    }

    public void LoadCustomerDetail()
    {
        //TODO: Load Customer Data from CustomerPackageInformation
        //TODO: Set HasDocument flag and IsEmptyState  as well.
    }

    public bool HasDocuments { get; set; }
    public bool IsEmptyState { get; set; }

    private void OnEditClicked(object sender, EventArgs e)
    {
        CustomerUpdateSelected?.Invoke(this, new CustomerSavedEventArgs(Customer));
    }

    private void OnReceiptClicked(object sender, EventArgs e)
    {
        CustomerReceiptSelected?.Invoke(this, new CustomerSavedEventArgs(Customer));
    }

}
