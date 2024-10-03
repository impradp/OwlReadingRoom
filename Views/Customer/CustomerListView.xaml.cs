using OwlReadingRoom.Services;
using OwlReadingRoom.Utils;
using OwlReadingRoom.ViewModels;
using System.Collections.ObjectModel;

namespace OwlReadingRoom.Views.Customer;

public partial class CustomerListView : ContentView
{
    private readonly IBookingService _bookingService;
    private CustomerView _activeView;
    private CustomerView _inactiveView;

    public event EventHandler<CustomerPackageViewModel> CustomerSelected;

    public ObservableCollection<CustomerPackageViewModel> ActiveCustomers { get; set; }
    public ObservableCollection<CustomerPackageViewModel> InactiveCustomers { get; set; }

    public CustomerListView(IBookingService bookingService)
    {
        _bookingService = bookingService;
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
        List<CustomerPackageViewModel> customerPackageViewModels = _bookingService.GetInactiveCustomerList();
        InactiveCustomers = new ObservableCollection<CustomerPackageViewModel>(customerPackageViewModels);

        _inactiveView = new CustomerView(InactiveCustomers);
        _inactiveView.CustomerSelected += OnCustomerSelected;
    }

    private void SetActiveCustomers()
    {
        //TODO: Implement service method usage for data extraction.
        List<CustomerPackageViewModel> customerPackageViewModels = _bookingService.GetActiveCustomerList();
        ActiveCustomers = new ObservableCollection<CustomerPackageViewModel>(customerPackageViewModels);

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
            ((Label)tab.Children[0]).TextColor = Color.FromArgb("#3B82F6");
            ((BoxView)tab.Children[1]).Color = Color.FromArgb("#3B82F6");
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
            ((Label)tab.Children[0]).TextColor = Color.FromArgb("#64748B");
            ((BoxView)tab.Children[1]).Color = Color.FromArgb("#E2E8F0");
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
