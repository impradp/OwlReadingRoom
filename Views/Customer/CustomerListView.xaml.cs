using OwlReadingRoom.Utils;
using OwlReadingRoom.ViewModels;
using System.Collections.ObjectModel;

namespace OwlReadingRoom.Views.Customer;

public partial class CustomerListView : ContentView
{
    private CustomerView _activeView;
    private CustomerView _inactiveView;

    public event EventHandler<CustomerPackageViewModel> CustomerSelected;

    public ObservableCollection<CustomerPackageViewModel> ActiveCustomers { get; set; }
    public ObservableCollection<CustomerPackageViewModel> InactiveCustomers { get; set; }

    public CustomerListView()
    {
        try
        {
            InitializeComponent();
            SetActiveCustomers();
            SetInactiveCustomers();
            TabContent.Content = _activeView;
        }
        catch (Exception ex)
        {
            ExceptionHandler.HandleException("Initializing CustomerListView", ex);
        }
    }

    private void SetInactiveCustomers()
    {
        //TODO: Implement service method usage for data extraction.
        InactiveCustomers = new ObservableCollection<CustomerPackageViewModel>
        {
            new CustomerPackageViewModel
            {
                FullName = "Jonathan Radford",
                ContactNumber = "+977-9801010101",
                Faculty = "CA",
                StartDate = "06/12/2024",
                EndDate = "06/12/2024",
                Package = "3 months",
                AllocatedSpace = "First Floor (AC)",
                PaymentStatus = "Paid",
                Dues = "00.00"
            }
        };

        _inactiveView = new CustomerView(InactiveCustomers);
        _inactiveView.CustomerSelected += OnCustomerSelected;
    }

    private void SetActiveCustomers()
    {
        //TODO: Implement service method usage for data extraction.
        ActiveCustomers = new ObservableCollection<CustomerPackageViewModel>
        {
            new CustomerPackageViewModel
            {
                FullName = "Samantha Radford",
                ContactNumber = "+977-9801010101",
                Faculty = "Doctor",
                StartDate = "06/12/2024",
                EndDate = "06/12/2024",
                Package = "1 month",
                AllocatedSpace = "First Floor (AC)",
                PaymentStatus = "Paid",
                Dues = "00.00"
            }
        };

        _activeView = new CustomerView(ActiveCustomers);
        _activeView.CustomerSelected += OnCustomerSelected;
    }

    private void OnCustomerSelected(object sender, CustomerPackageViewModel selectedCustomer)
    {
        CustomerSelected?.Invoke(this, selectedCustomer);
    }

    private void OnActiveTabTapped(object sender, EventArgs e)
    {
        try
        {
            SetActiveTab(ActiveTab);
            SetInactiveTab(InactiveTab);
            TabContent.Content = _activeView;
        }
        catch (Exception ex)
        {
            ExceptionHandler.HandleException("Switching to active tab", ex);
        }
    }

    private void OnInactiveTabTapped(object sender, EventArgs e)
    {
        try
        {
            SetActiveTab(InactiveTab);
            SetInactiveTab(ActiveTab);
            TabContent.Content = _inactiveView;
        }
        catch (Exception ex)
        {
            ExceptionHandler.HandleException("Switching to inactive tab", ex);
        }
    }

    private void SetActiveTab(StackLayout tab)
    {
        try
        {
            ((Label)tab.Children[0]).TextColor = Color.FromHex("#3B82F6");
            ((BoxView)tab.Children[1]).Color = Color.FromHex("#3B82F6");
        }
        catch (Exception ex)
        {
            ExceptionHandler.HandleException("Setting active tab", ex);
        }
    }

    private void SetInactiveTab(StackLayout tab)
    {
        try
        {
            ((Label)tab.Children[0]).TextColor = Color.FromHex("#64748B");
            ((BoxView)tab.Children[1]).Color = Color.FromHex("#E2E8F0");
        }
        catch (Exception ex)
        {
            ExceptionHandler.HandleException("Setting inactive tab", ex);
        }
    }

    // New method to update customers
    public void UpdateCustomers(ObservableCollection<CustomerPackageViewModel> activeCustomers, ObservableCollection<CustomerPackageViewModel> inactiveCustomers)
    {
        try
        {
            ActiveCustomers = activeCustomers;
            InactiveCustomers = inactiveCustomers;

            _activeView.Customers = ActiveCustomers;
            _inactiveView.Customers = InactiveCustomers;
        }
        catch (Exception ex)
        {
            ExceptionHandler.HandleException("Updating customers", ex);
        }
    }
}
