using Auth0.OidcClient;
using CommunityToolkit.Maui.Views;
using IdentityModel.OidcClient;
using IdentityModel.OidcClient.Browser;
using OwlReadingRoom.Events;
using OwlReadingRoom.Models;
using OwlReadingRoom.Services;
using OwlReadingRoom.Utils;
using OwlReadingRoom.ViewModels;
using OwlReadingRoom.Views;
using OwlReadingRoom.Views.Customer;
using OwlReadingRoom.Views.Profile;
using OwlReadingRoom.Views.Resources;
using OwlReadingRoom.Views.Resources.Package;
using OwlReadingRoom.Views.Resources.Rooms;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace OwlReadingRoom
{
    public partial class MainView : ContentPage, INotifyPropertyChanged
    {
        private string _selectedMenu = "";
        private LoginResult _loginResult;
        private bool _isResourceMenuExpanded;
        private readonly Auth0Client _auth0Client;
        private readonly IServiceProvider _serviceProvider;
        private SelectedSubMenu _currentSubmenu = SelectedSubMenu.None;

        private readonly CustomerListView _customerListView;
        private CustomerDetailsView _currentCustomerDetailsView;

        public event PropertyChangedEventHandler PropertyChanged;

        public MainView(LoginResult loginResult, Auth0Client auth0Client, IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _loginResult = loginResult;
            _auth0Client = auth0Client;
            _serviceProvider = serviceProvider;

            // Create a single instance of CustomerListView
            _customerListView = new CustomerListView();
            _customerListView.CustomerSelected += OnCustomerSelected;

            // Set initial content
            SetCustomerListView();

            BindingContext = this;
        }

        private void SetCustomerListView()
        {
            UnsubscribeFromCurrentDetailView();
            DynamicContentArea.Content = _customerListView;
        }

        private void OnCustomerSelected(object sender, CustomerPackageViewModel selectedCustomer)
        {
            UnsubscribeFromCurrentDetailView();

            _currentCustomerDetailsView = new CustomerDetailsView(selectedCustomer);
            _currentCustomerDetailsView.CustomerUpdateSelected += OnCustomerUpdateSelected;
            _currentCustomerDetailsView.CustomerReceiptSelected += OnCustomerReceiptSelected;
            DynamicContentArea.Content = _currentCustomerDetailsView;
        }

        private void UnsubscribeFromCurrentDetailView()
        {
            if (_currentCustomerDetailsView != null)
            {
                _currentCustomerDetailsView.CustomerUpdateSelected -= OnCustomerUpdateSelected;
                _currentCustomerDetailsView.CustomerReceiptSelected -= OnCustomerReceiptSelected;
                _currentCustomerDetailsView = null;
            }
        }

        #region Properties

        public SelectedSubMenu CurrentSubmenu
        {
            get => _currentSubmenu;
            set
            {
                if (_currentSubmenu != value)
                {
                    _currentSubmenu = value;
                    OnPropertyChanged();
                }
            }
        }

        public User GetLoggedInUser
        {
            get
            {
                User user = new User();
                string timeOfDay = Utility.GetTimeOfDay();
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

        public bool IsResourceMenuExpanded
        {
            get => _isResourceMenuExpanded;
            set
            {
                if (_isResourceMenuExpanded != value)
                {
                    _isResourceMenuExpanded = value;
                    OnPropertyChanged();
                }
            }
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

        #endregion

        #region Event Handlers

        private void OnRoomSubmenuClicked(object sender, EventArgs e)
        {
            CurrentSubmenu = SelectedSubMenu.Room;
            DynamicContentArea.Content = _serviceProvider.GetService<RoomListView>();
        }

        private void OnPackageSubmenuClicked(object sender, EventArgs e)
        {
            CurrentSubmenu = SelectedSubMenu.Package;
            DynamicContentArea.Content = new PackageListView();
        }

        private void OnNewEntryButtonClicked(object sender, EventArgs e)
        {
            var newCustomerPopup = _serviceProvider.GetService<NewCustomer>();
            this.ShowPopup(newCustomerPopup);
        }

        private void OnCustomerUpdateSelected(object sender, CustomerSavedEventArgs e)
        {
            var customerUpdateView = new CustomerUpdateView(e.SavedCustomerPackage);
            DynamicContentArea.Content = customerUpdateView;
        }

        private void OnCustomerReceiptSelected(object sender, CustomerSavedEventArgs e)
        {
            var printService = _serviceProvider.GetService<IPrintService>();
            var customerReceiptView = new ReceiptView(e.SavedCustomerPackage, printService);
            DynamicContentArea.Content = customerReceiptView;
        }

        private void OnCustomerMenuClicked(object sender, EventArgs e)
        {
            SelectedMenu = "Customer";
            SetCustomerListView();
        }

        private void OnResourceMenuClicked(object sender, EventArgs e)
        {
            IsResourceMenuExpanded = !IsResourceMenuExpanded;
            SelectedMenu = "Resources";
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

            if (!browserResult.Equals(BrowserResultType.Success))
            {
                // TODO: Handle logout failure
            }
            var resultPage = _serviceProvider.GetService<AuthenticationPage>();

            await Navigation.PushAsync(resultPage);
            Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 2]);
        }

        private void OnHomeSelected(object sender, EventArgs e)
        {
            SetCustomerListView();
        }

        #endregion

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
