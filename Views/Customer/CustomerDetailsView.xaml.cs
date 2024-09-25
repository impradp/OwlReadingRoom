using OwlReadingRoom.Components.AlertDialog;
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
    public CustomerDetailViewModel CustomerDetail { get; private set; }
    private readonly ICustomerDetailsService _customerDetailService;

    private bool _isEditVisible;
    private bool _isRenewVisible;
    private bool _isReceiptVisible;

    public bool IsEditVisible
    {
        get => _isEditVisible;
        set
        {
            if (_isEditVisible != value)
            {
                _isEditVisible = value;
                OnPropertyChanged();
            }
        }
    }

    public bool IsRenewVisible
    {
        get => _isRenewVisible;
        set
        {
            if (_isRenewVisible != value)
            {
                _isRenewVisible = value;
                OnPropertyChanged();
            }
        }
    }

    public bool IsReceiptVisible
    {
        get => _isReceiptVisible;
        set
        {
            if (_isReceiptVisible != value)
            {
                _isReceiptVisible = value;
                OnPropertyChanged();
            }
        }
    }

    public CustomerDetailsView(CustomerPackageViewModel customer, ICustomerDetailsService customerDetailService)
    {
        InitializeComponent();
        _customerMinimalDetail = customer;
        _customerDetailService = customerDetailService;
        LoadCustomerDetail();
        BindingContext = this;
    }

    public void LoadCustomerDetail()
    {
        CustomerDetail = _customerDetailService.fetchCustomerDetails(_customerMinimalDetail);
        IsRenewVisible = CustomerDetail.Status.ToLower().Equals("inactive") && !String.IsNullOrEmpty(CustomerDetail.BookingDetails?.PackageName);
        IsReceiptVisible = !IsRenewVisible && !String.IsNullOrEmpty(CustomerDetail.BookingDetails?.PackageName);
        IsEditVisible = !IsRenewVisible;
        OnPropertyChanged(nameof(CustomerDetail));
    }

    private void OnEditClicked(object sender, EventArgs e)
    {
        CustomerUpdateSelected?.Invoke(this, new CustomerSavedEventArgs(CustomerDetail));
    }

    private void OnRenewCustomer(object sender, EventArgs e)
    {
        _customerDetailService.RenewCustomerBooking(CustomerDetail);
        CustomerUpdateSelected?.Invoke(this, new CustomerSavedEventArgs(CustomerDetail));
        AlertService.Instance.ShowAlert("Success", "Customer booking renewed successfully.", AlertType.Success);
    }

    private void OnReceiptClicked(object sender, EventArgs e)
    {
        CustomerReceiptSelected?.Invoke(this, new CustomerSavedEventArgs(CustomerDetail));
    }
}