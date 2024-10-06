using OwlReadingRoom.ViewModels;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace OwlReadingRoom.Views.Customer;

public partial class CustomerView : ContentView, INotifyPropertyChanged
{
    private ObservableCollection<CustomerPackageViewModel> _customers;
    public ObservableCollection<CustomerPackageViewModel> Customers
    {
        get => _customers;
        set
        {
            if (_customers != value)
            {
                _customers = value;
                OnPropertyChanged();
            }
        }
    }

    public event EventHandler<CustomerPackageViewModel> CustomerSelected;
    public event PropertyChangedEventHandler PropertyChanged;

    public ICommand CustomerTappedCommand { get; private set; }

    public bool IsEmptyState => Customers.Count==0;

    public CustomerView(ObservableCollection<CustomerPackageViewModel> customers)
    {
        InitializeComponent();
        Customers = customers;
        CustomerTappedCommand = new Command<CustomerPackageViewModel>(OnCustomerTapped);
        BindingContext = this;
    }

    /// <summary>
    /// Handles the customer row tap event to view the details of the selecte customer.
    /// </summary>
    /// <param name="selectedCustomer">The customer selected for detail view.</param>
    private void OnCustomerTapped(CustomerPackageViewModel selectedCustomer)
    {
        CustomerSelected?.Invoke(this, selectedCustomer);
    }

    /// <summary>
    /// Handles the property change event to perform event driven actions.
    /// </summary>
    /// <param name="propertyName">The property which identifies the action to be driven in the scope.</param>
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
