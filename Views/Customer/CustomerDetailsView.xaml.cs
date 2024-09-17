using OwlReadingRoom.Events;
using OwlReadingRoom.Services;
using OwlReadingRoom.ViewModels;
using System.Diagnostics;

namespace OwlReadingRoom.Views.Customer;

public partial class CustomerDetailsView : ContentView
{
    public event EventHandler<CustomerSavedEventArgs> CustomerUpdateSelected;
    public event EventHandler<CustomerSavedEventArgs> CustomerReceiptSelected;
    private CustomerPackageViewModel _customerMinimalDetail;
    public CustomerDetailViewModel CustomerDetail;
    private readonly ICustomerDetailsService _customerDetailService;
    public CustomerDetailsView(CustomerPackageViewModel customer, ICustomerDetailsService customerDetailService)
    {
        InitializeComponent();
        _customerMinimalDetail = customer;
        _customerDetailService = customerDetailService;
        LoadCustomerDetail();
        BindingContext = CustomerDetail;
    }

    public void LoadCustomerDetail()
    {
        CustomerDetail = _customerDetailService.fetchCustomerDetails(_customerMinimalDetail);
        Debug.WriteLine("");
    }

    private void OnEditClicked(object sender, EventArgs e)
    {
        CustomerUpdateSelected?.Invoke(this, new CustomerSavedEventArgs(CustomerDetail));
    }

    private void OnReceiptClicked(object sender, EventArgs e)
    {
        CustomerReceiptSelected?.Invoke(this, new CustomerSavedEventArgs(CustomerDetail));
    }

}
