using OwlReadingRoom.Utils;
using OwlReadingRoom.ViewModels;

namespace OwlReadingRoom.Views.Customer;

public partial class CustomerUpdateView : ContentView
{
    private PersonalDetailView _personalDetailView;
    private DocumentDetailView _documentDetailView;
    private PackagePaymentDetailView _packagePaymentDetailView;

    public CustomerPackageViewModel Customer { get; set; }

    public CustomerUpdateView(CustomerPackageViewModel viewModel)
    {
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

    private void SetPersonalDetailContent(CustomerPackageViewModel customerPackage)
    {
        //TODO: Implement service method usage for data extraction.
        _personalDetailView = new PersonalDetailView(customerPackage);
    }

    private void SetDocumentDetailContent(CustomerPackageViewModel customerPackage)
    {
        //TODO: Implement service method usage for data extraction.
        _documentDetailView = new DocumentDetailView(customerPackage);
    }

    private void SetPackagePaymentDetailContent(CustomerPackageViewModel customerPackage)
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
