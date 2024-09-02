using OwlReadingRoom.Services;
using OwlReadingRoom.ViewModels;
using System.Windows.Input;

namespace OwlReadingRoom.Views.Customer;

public partial class ReceiptView : ContentView
{
    private CustomerPackageViewModel _customer;
    private readonly IPrintService _printService;
    public ICommand PrintCommand { get; private set; }
    public ReceiptView(CustomerPackageViewModel customer, IPrintService printService)
    {
        InitializeComponent();
        _customer = customer;
        _printService = printService;
        PrintCommand = new Command(async () => PrintReceipt());
        BindingContext = this;
    }

    public CustomerPackageViewModel Customer
    {
        get => _customer;
        set
        {
            if (_customer != value)
            {
                _customer = value;
            }
        }
    }

    private async Task PrintReceipt()
    {
        if (DeviceInfo.Platform == DevicePlatform.WinUI)
        {
            await _printService.PrintAsync(this);
        }
        else
        {
            await Application.Current.MainPage.DisplayAlert("Print Error", "Printing is only supported on Windows.", "OK");
        }
    }

}