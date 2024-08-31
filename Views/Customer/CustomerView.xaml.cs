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

    public CustomerView(ObservableCollection<CustomerPackageViewModel> customers)
    {
        InitializeComponent();
        Customers = customers;
        CustomerTappedCommand = new Command<CustomerPackageViewModel>(OnCustomerTapped);
        BindingContext = this;
    }

    private void OnCustomerTapped(CustomerPackageViewModel selectedCustomer)
    {
        CustomerSelected?.Invoke(this, selectedCustomer);
    }

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
