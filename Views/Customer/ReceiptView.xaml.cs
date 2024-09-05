using OwlReadingRoom.Services;
using OwlReadingRoom.ViewModels;
using System.Windows.Input;

namespace OwlReadingRoom.Views.Customer
{
    public partial class ReceiptView : ContentView
    {
        private CustomerPackageViewModel _customer;

        private IPdfService _pdfService;
        public ICommand DownloadCommand { get; private set; }

        public ReceiptView(CustomerPackageViewModel customer, IPdfService pdfService)
        {
            InitializeComponent();
            _customer = customer;
            _pdfService = pdfService;
            DownloadCommand = new Command(async () => await DownloadReceiptPdfAsync());
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
                    OnPropertyChanged(nameof(Customer));
                }
            }
        }

        /// <summary>
        /// Downloads the payment receipt as a pdf.
        /// </summary>
        /// <returns>The downloaded pdf</returns>
        private async Task DownloadReceiptPdfAsync()
        {
            await _pdfService.DownloadAsync(ReceiptContent);
        }
    }
}
