using OwlReadingRoom.Events;
using OwlReadingRoom.Services;
using OwlReadingRoom.Services.Resources;
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

    public event EventHandler<CustomerSavedEventArgs> PackageCreatedForCustomer;

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

    /// <summary>
    /// Sets the document detail content for the document detail tab.
    /// </summary>
    /// <param name="customerPackage">The customer package information for the corresponding document owner.</param>
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

    /// <summary>
    /// Sets the package payment information for the package payment information.
    /// </summary>
    /// <param name="customerPackage">The customer detail information.</param>
    private void SetPackagePaymentDetailContent(CustomerDetailViewModel customerPackage)
    {
        PackageAndPaymentEditViewModel packageAndPaymentEditViewModel = new PackageAndPaymentEditViewModel
        {
            Id = customerPackage.BookingDetails?.Id

        };
        _packagePaymentDetailView = new PackagePaymentDetailView(customerPackage, packageAndPaymentEditViewModel, _serviceProvider.GetService<IPhysicalResourceService>(), _serviceProvider);
        _packagePaymentDetailView.PackageCreated += OnPackageCreated;
    }

    /// <summary>
    /// Invokes the event once the package is created.
    /// </summary>
    /// <param name="sender">The object which initiates the on packagecreated event.</param>
    /// <param name="updatedCustomer">The event argument containing customer detail view.</param>
    private void OnPackageCreated(object sender, CustomerDetailViewModel updatedCustomer)
    {
        // Notify the MainView that we need to switch back to CustomerDetailsView
        PackageCreatedForCustomer?.Invoke(this, new CustomerSavedEventArgs(updatedCustomer));
    }

    /// <summary>
    /// Handles the personal detail tab tap event.
    /// </summary>
    /// <param name="sender">The tab that initiates this event.</param>
    /// <param name="e">The event argument containing the personal detail.</param>
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

    /// <summary>
    /// Handles the document detail tab tap event.
    /// </summary>
    /// <param name="sender">The button that trigges the tap event.</param>
    /// <param name="e">The event argument containing the customer info.</param>
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

    /// <summary>
    /// Handles the package payment tab tap event.
    /// </summary>
    /// <param name="sender">The button that handles the tap event.</param>
    /// <param name="e">The event argument that contains the customer detail argument.</param>
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

    /// <summary>
    /// Sets the active tab to display the active styles.
    /// </summary>
    /// <param name="tab">The tab containing the styles information.</param>
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

    /// <summary>
    /// Sets the inactive tab info and styles info.
    /// </summary>
    /// <param name="tab1">The tab containing the styles of its own for active or inactive.</param>
    /// <param name="tab2">The tab containing the styles of its own for active or inactive.</param>
    private void SetInactiveTabs(StackLayout tab1, StackLayout tab2)
    {
        try
        {
            ((Label)tab1.Children[0]).TextColor = Color.FromArgb("#64748B");
            ((BoxView)tab1.Children[1]).Color = Color.FromArgb("#E2E8F0");
            ((Label)tab2.Children[0]).TextColor = Color.FromArgb("#64748B");
            ((BoxView)tab2.Children[1]).Color = Color.FromArgb("#E2E8F0");
        }
        catch (Exception ex)
        {
            ExceptionHandler.HandleException("Setting inactive tab", ex);
        }
    }

}
