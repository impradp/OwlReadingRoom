using Microsoft.Extensions.Configuration;
using OwlReadingRoom.Configurations;
using OwlReadingRoom.Services;
using OwlReadingRoom.ViewModels;

namespace OwlReadingRoom.Views.Customer
{
    public partial class ReceiptView : ContentView
    {
        private CustomerDetailViewModel _customer;

        private readonly IServiceProvider _serviceProvider;

        public CompanyDetails Company { get; set; }

        public ReceiptView(IServiceProvider serviceProvider, CustomerDetailViewModel customer)
        {
            InitializeComponent();
            Customer = customer;
            _serviceProvider = serviceProvider;
            LoadCompanyInfo();
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
        /// Loads conmpany info for receipt generation.
        /// The company info will be extracted from the configuration manager.
        /// </summary>
        public void LoadCompanyInfo()
        {
            IConfiguration configuration = _serviceProvider.GetService<IConfiguration>();
            Company = ConfigurationHandler.GetCompanyInformation(configuration);
        }

        /// <summary>
        /// Downloads the payment receipt as a pdf.
        /// </summary>
        /// <returns>The downloaded pdf</returns>
        private async void OnDownloadReceiptClicked(object sender, EventArgs e)
        {
            IPdfService pdfService = _serviceProvider.GetService<IPdfService>();
            await pdfService.DownloadAsync(ReceiptContent);
        }
    }
}
