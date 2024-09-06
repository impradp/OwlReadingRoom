using OwlReadingRoom.Services;
using OwlReadingRoom.ViewModels;

namespace OwlReadingRoom.Views.Customer
{
    public partial class ReceiptView : ContentView
    {
        private CustomerDetailViewModel _customer;

        private IPdfService _pdfService;

        public ReceiptView(CustomerDetailViewModel customer, IPdfService pdfService)
        {
            InitializeComponent();
            _customer = customer;
            _pdfService = pdfService;
            BindingContext = this;
        }

        public CustomerDetailViewModel Customer
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
        private async void OnDownloadReceiptClicked(object sender, EventArgs e)
        {
            await _pdfService.DownloadAsync(ReceiptContent);
        }
    }
}
