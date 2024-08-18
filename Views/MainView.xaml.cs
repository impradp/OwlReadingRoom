using Auth0.OidcClient;
using CommunityToolkit.Maui.Views;
using IdentityModel.OidcClient;
using IdentityModel.OidcClient.Browser;
using OwlReadingRoom.Events;
using OwlReadingRoom.Models;
using OwlReadingRoom.Services;
using OwlReadingRoom.Views;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace OwlReadingRoom
{
    public partial class MainView : ContentPage, INotifyPropertyChanged
    {
        private string _selectedMenu = "";

        private LoginResult _loginResult;

        private readonly Auth0Client _auth0Client;

        private readonly IServiceProvider _serviceProvider;

        public event PropertyChangedEventHandler PropertyChanged;

        public MainView(LoginResult loginResult, Auth0Client auth0Client, IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _loginResult = loginResult;
            _auth0Client = auth0Client;
            DynamicContentArea.Content = new CustomerListView();
            _serviceProvider = serviceProvider;
            BindingContext = this;
        }

        public User GetLoggedInUser
        {
            get
            {
                User user = new User();
                string timeOfDay = GetTimeOfDay();
                var authUser = _loginResult.User;
                user.Name = authUser.FindFirst(c => c.Type == "name")?.Value;
                user.Email = authUser.FindFirst(c => c.Type == "email")?.Value;
                user.Picture = authUser.FindFirst(c => c.Type == "picture")?.Value;
                user.Role = authUser.FindFirst(c => c.Type == "roles")?.Value
                    ?? authUser.FindFirst(c => c.Type == "role")?.Value
                    ?? "Guest";
                string GivenName = authUser.FindFirst(c => c.Type == "given_name")?.Value;
                user.GreetingInfo = $"Good {timeOfDay}, {GivenName}!";
                return user;
            }
        }

        private string GetTimeOfDay()
        {
            var hour = DateTime.Now.Hour;
            if (hour < 12) return "Morning";
            if (hour < 18) return "Afternoon";
            return "Evening";
        }

        public string SelectedMenu
        {
            get => _selectedMenu;
            set
            {
                if (_selectedMenu != value)
                {
                    _selectedMenu = value;
                    OnPropertyChanged();
                }
            }
        }

        private void OnNewEntryButtonClicked(object sender, EventArgs e)
        {
            var newCustomerPopup = _serviceProvider.GetService<NewCustomer>();
            newCustomerPopup.CustomerPackageSaved += OnCustomerSaved;
            this.ShowPopup(newCustomerPopup);
        }

        private void OnCustomerSaved(object sender, CustomerSavedEventArgs e)
        {
            // Map the saved value received to the binding values needed for New Entry.
            var savedPersonalDetails = new PersonalDetailView();
            savedPersonalDetails.PersonalDetailSaved += OnPersonalDetailSaved;
            DynamicContentArea.Content = savedPersonalDetails;
        }

        private void OnPersonalDetailSaved(object sender, PersonalDetailSavedEventArgs e)
        {
            // Map the saved value received to the binding values needed for New Entry.
            var savedDocumentDetails = new DocumentDetailView();
            savedDocumentDetails.DocumentDetailSaved += OnDocumentDetailSaved;
            DynamicContentArea.Content = savedDocumentDetails;
        }

        private void OnDocumentDetailSaved(object sender, DocumentDetailSavedEventArgs e)
        {
            // Map the saved value received to the binding values needed for New Entry.
            var savedPackagePaymentDetails = new PackagePaymentDetailView();
            savedPackagePaymentDetails.PackagePaymentDetailSaved += OnPackagePaymentDetailSaved;
            DynamicContentArea.Content = savedPackagePaymentDetails;
        }

        private void OnPackagePaymentDetailSaved(object sender, PackagePaymentSavedEventArgs e)
        {
            // Map the saved value received to the binding values needed for New Entry.
            DynamicContentArea.Content = new CustomerListView();
        }

        private void OnCustomerMenuClicked(object sender, EventArgs e)
        {
            SelectedMenu = "Customer";
            DynamicContentArea.Content = new CustomerListView();
        }

        private void OnResourceMenuClicked(object sender, EventArgs e)
        {
            SelectedMenu = "Resources";
            DynamicContentArea.Content = new ResourcesView();
        }

        private void OnProfileMenuClicked(object sender, EventArgs e)
        {
            SelectedMenu = "Settings";
            DynamicContentArea.Content = new ProfileView();
        }

        private async void OnLogoutMenuClicked(object sender, EventArgs e)
        {
            SelectedMenu = "Logout";
            BrowserResultType browserResult = await _auth0Client.LogoutAsync();
            _serviceProvider.GetService<IUserService>()?.ClearUserInfo();

            // TODO: Popup error on logout issue
            if (!browserResult.Equals(BrowserResultType.Success))
            {
                // throw new Exception();
            }
            var resultPage = _serviceProvider.GetService<AuthenticationPage>();

            // Navigate to the result page
            await Navigation.PushAsync(resultPage);
            // Clear the navigation stack
            Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 2]);
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void OnHomeSelected(object sender, EventArgs e)
        {
            // Navigate to the desired page
            DynamicContentArea.Content = new CustomerListView();
        }
    }

}
