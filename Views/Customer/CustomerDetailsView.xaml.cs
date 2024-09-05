using OwlReadingRoom.Events;
using OwlReadingRoom.ViewModels;
using System.ComponentModel;

namespace OwlReadingRoom.Views.Customer;

public partial class CustomerDetailsView : ContentView, INotifyPropertyChanged
{
    public event EventHandler<CustomerSavedEventArgs> CustomerUpdateSelected;
    public event EventHandler<CustomerSavedEventArgs> CustomerReceiptSelected;
    private CustomerPackageViewModel _customer;
    public CustomerDetailsView(CustomerPackageViewModel customer)
    {
        InitializeComponent();
        BindingContext = this;
        _customer = customer;
    }

    public CustomerPackageViewModel Customer
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
    public bool HasDocuments => Customer != null && Customer.Documents != null && Customer.Documents.Count > 0;
    public bool IsEmptyState => !HasDocuments;

    private void OnEditClicked(object sender, EventArgs e)
    {
        CustomerUpdateSelected?.Invoke(this, new CustomerSavedEventArgs(Customer));
    }

    private void OnReceiptClicked(object sender, EventArgs e)
    {
        CustomerReceiptSelected?.Invoke(this, new CustomerSavedEventArgs(Customer));
    }

}
