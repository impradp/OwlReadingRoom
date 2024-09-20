using OwlReadingRoom.Services;
using OwlReadingRoom.Utils;
using OwlReadingRoom.ViewModels;
using System.Collections.ObjectModel;

namespace OwlReadingRoom.Views.Customer;

public partial class CustomerUpdateView : ContentView
{
    private PersonalDetailView _personalDetailView;
    private DocumentDetailView _documentDetailView;
    private PackagePaymentDetailView _packagePaymentDetailView;
    private readonly IServiceProvider _serviceProvider;

    public CustomerDetailViewModel Customer { get; set; }

    public CustomerUpdateView(CustomerDetailViewModel viewModel, IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        try
        {
            InitializeComponent();
            SetPersonalDetailContent(viewModel);
            SetDocumentDetailContent(viewModel);
            SetPackagePaymentDetailContent(viewModel);
            TabContent.Content = _personalDetailView;
        }
        catch (Exception ex)
        {
            ExceptionHandler.HandleException("Initializing CustomerListView", ex);
        }
    }

    private void SetPersonalDetailContent(CustomerDetailViewModel customerPackage)
    {
        PersonalDetailEditViewModel personalDetailEditViewModel = new PersonalDetailEditViewModel
        {
            CustomerId = customerPackage.CustomerId,
            Allergies = customerPackage.Allergies,
            CurrentAddress = customerPackage.CurrentAddress,
            DateOfBirth = customerPackage.DateOfBirth,
            Disease = customerPackage.Disease,
            Faculty = customerPackage.Faculty,
            FullName = customerPackage.FullName,
            Gender = customerPackage.Gender,
            MobileNumber = customerPackage.MobileNumber,
            PermanantAddress = customerPackage.PermanantAddress,
            Status = customerPackage.Status
        };

        _personalDetailView = new PersonalDetailView(personalDetailEditViewModel, _serviceProvider.GetService<ICustomerService>());
    }

    private void SetDocumentDetailContent(CustomerDetailViewModel customerPackage)
    {
        DocumentEditViewModel viewModel = new DocumentEditViewModel()
        {
            CustomerId = customerPackage.CustomerId,
            DocumentInformationId = customerPackage.Documents?.Id,
            DocumentNumber = customerPackage.Documents?.DocumentNumber,
            DocumentType = customerPackage.Documents?.DocumentType,
            IssueDate = customerPackage.Documents?.IssueDate,
            PlaceOfIssue = customerPackage.Documents?.PlaceOfIssue,
            SelectedFiles = new ObservableCollection<DocumentImageViewModel>(customerPackage.Documents?.Locations ?? Enumerable.Empty<DocumentImageViewModel>())
        };
        _documentDetailView = new DocumentDetailView(viewModel, _serviceProvider.GetService<ICustomerService>());
    }

    private void SetPackagePaymentDetailContent(CustomerDetailViewModel customerPackage)
    {
        //TODO: Implement service method usage for data extraction.
        _packagePaymentDetailView = new PackagePaymentDetailView(customerPackage);
    }

    private void OnPersonalDetailTabTapped(object sender, EventArgs e)
    {
        try
        {
            SetActiveTab(PersonalDetailTab);
            SetInactiveTabs(DocumentDetailTab, PackagePaymentTab);
            TabContent.Content = _personalDetailView;
        }
        catch (Exception ex)
        {
            ExceptionHandler.HandleException("Switching to personal detail tab", ex);
        }
    }

    private void OnDocumentDetailTabTapped(object sender, EventArgs e)
    {
        try
        {
            SetActiveTab(DocumentDetailTab);
            SetInactiveTabs(PersonalDetailTab, PackagePaymentTab);
            TabContent.Content = _documentDetailView;
        }
        catch (Exception ex)
        {
            ExceptionHandler.HandleException("Switching to document detail tab", ex);
        }
    }

    private void OnPackagePaymentTabTapped(object sender, EventArgs e)
    {
        try
        {
            SetActiveTab(PackagePaymentTab);
            SetInactiveTabs(PersonalDetailTab, DocumentDetailTab);
            TabContent.Content = _packagePaymentDetailView;
        }
        catch (Exception ex)
        {
            ExceptionHandler.HandleException("Switching to document detail tab", ex);
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

    private void SetInactiveTabs(StackLayout tab1,StackLayout tab2)
    {
        try
        {
            ((Label)tab1.Children[0]).TextColor = Color.FromHex("#64748B");
            ((BoxView)tab1.Children[1]).Color = Color.FromHex("#E2E8F0");
            ((Label)tab2.Children[0]).TextColor = Color.FromHex("#64748B");
            ((BoxView)tab2.Children[1]).Color = Color.FromHex("#E2E8F0");
        }
        catch (Exception ex)
        {
            ExceptionHandler.HandleException("Setting inactive tab", ex);
        }
    }


}
