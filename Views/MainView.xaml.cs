﻿using Auth0.OidcClient;
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
using OwlReadingRoom.Views.Resources.Package;
using OwlReadingRoom.Views.Resources.Rooms;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace OwlReadingRoom
{
    public partial class MainView : ContentPage, INotifyPropertyChanged
    {
        private string _selectedMenu = "";
        private readonly LoginResult _loginResult;
        private bool _isResourceMenuExpanded;
        private readonly Auth0Client _auth0Client;
        private readonly IServiceProvider _serviceProvider;
        private SelectedSubMenu _currentSubmenu = SelectedSubMenu.None;

        private CustomerListView _customerListView;
        private CustomerDetailsView _currentCustomerDetailsView;

        public event PropertyChangedEventHandler PropertyChanged;

        public MainView(LoginResult loginResult, Auth0Client auth0Client, IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _loginResult = loginResult;
            _auth0Client = auth0Client;
            _serviceProvider = serviceProvider;

            // Set initial content
            SetCustomerListView();

            BindingContext = this;
        }

        /// <summary>
        /// Sets the list of customers to be displayed on landing page.
        /// </summary>
        private void SetCustomerListView()
        {
            // Create a single instance of CustomerListView
            _customerListView = _serviceProvider.GetService<CustomerListView>();
            _customerListView.CustomerSelected += OnCustomerSelected;

            UnsubscribeFromCurrentDetailView();
            DynamicContentArea.Content = _customerListView;
        }

        /// <summary>
        /// Sets the customer detail view for the selected customer.
        /// </summary>
        /// <param name="sender">The object that triggers the customer selection event.</param>
        /// <param name="selectedCustomer">The selected customer object passed on to this function for detailed display.</param>
        private void OnCustomerSelected(object sender, CustomerPackageViewModel selectedCustomer)
        {
            try
            {
                UnsubscribeFromCurrentDetailView();
                Func<CustomerPackageViewModel, CustomerDetailsView>? customerDetailView = _serviceProvider.GetService<Func<CustomerPackageViewModel, CustomerDetailsView>>();
                _currentCustomerDetailsView = customerDetailView(selectedCustomer);
                _currentCustomerDetailsView.CustomerUpdateSelected += OnCustomerUpdateSelected;
                _currentCustomerDetailsView.CustomerReceiptSelected += OnCustomerReceiptSelected;
                DynamicContentArea.Content = _currentCustomerDetailsView;
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException("displaying customer details", ex);
            }
        }

        /// <summary>
        /// Unsubscribes the event triggers for the current view.
        /// </summary>
        private void UnsubscribeFromCurrentDetailView()
        {
            if (_currentCustomerDetailsView != null)
            {
                _currentCustomerDetailsView.CustomerUpdateSelected -= OnCustomerUpdateSelected;
                _currentCustomerDetailsView.CustomerReceiptSelected -= OnCustomerReceiptSelected;
                _currentCustomerDetailsView = null;
            }
        }

        /// <summary>
        /// Handles the display of the receipt view for selected customer.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCustomerReceiptSelected(object sender, CustomerSavedEventArgs e)
        {
            var customerReceiptView = new ReceiptView(_serviceProvider, e.SavedCustomerDetail);
            DynamicContentArea.Content = customerReceiptView;
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
                User user = new();
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

        /// <summary>
        /// Handles the display of room list upon room sub menu click.
        /// </summary>
        /// <param name="sender">The sub menu that triggers this event.</param>
        /// <param name="e">The event argument passed on to this function for room list display.</param>
        private void OnRoomSubmenuClicked(object sender, EventArgs e)
        {
            CurrentSubmenu = SelectedSubMenu.Room;
            DynamicContentArea.Content = _serviceProvider.GetService<RoomListView>();
        }

        /// <summary>
        /// Handles the display of the package list upon package sub menu click.
        /// </summary>
        /// <param name="sender">The package menu that triggers this event.</param>
        /// <param name="e">The event argument passed on to this function for package list display.</param>
        private void OnPackageSubmenuClicked(object sender, EventArgs e)
        {
            CurrentSubmenu = SelectedSubMenu.Package;
            DynamicContentArea.Content = _serviceProvider.GetService<PackageListView>();
        }

        /// <summary>
        /// Handles the display of customer creation form as a modal popup.
        /// </summary>
        /// <param name="sender">The new entry button that triggers this event.</param>
        /// <param name="e">The event argument passed on to this function for customer registration.</param>
        private void OnNewEntryButtonClicked(object sender, EventArgs e)
        {
            var newCustomerPopup = _serviceProvider.GetService<NewCustomer>();
            newCustomerPopup.CustomerPackageSaved += OnCustomerMenuClicked;
            this.ShowPopup(newCustomerPopup);
        }

        /// <summary>
        /// Handles the display of customer update view.
        /// </summary>
        /// <param name="sender">The edit button that triggered the customer update function.</param>
        /// <param name="e">The argument passed down to update the selected customer.</param>
        private void OnCustomerUpdateSelected(object sender, CustomerSavedEventArgs e)
        {
            var customerUpdateView = new CustomerUpdateView(e.SavedCustomerDetail, _serviceProvider);
            customerUpdateView.PackageCreatedForCustomer += OnPackageCreatedForCustomer;
            DynamicContentArea.Content = customerUpdateView;
        }

        /// <summary>
        /// Handles the package creation event for designated customer.
        /// Redirects to the detail page once the package is saved.
        /// </summary>
        /// <param name="sender">The button that triggers this event.</param>
        /// <param name="e">The event argument containing the customer info.</param>
        private void OnPackageCreatedForCustomer(object sender, CustomerSavedEventArgs e)
        {
            CustomerPackageViewModel customerPackage = new CustomerPackageViewModel()
            {
                CustomerId = e.SavedCustomerDetail.CustomerId,
                BookingId = e.SavedCustomerDetail.BookingDetails.Id,
                Status = (Status)Enum.Parse(typeof(Status), e.SavedCustomerDetail.Status, true),
                ContactNumber = e.SavedCustomerDetail.MobileNumber
            };
            // Switch back to CustomerDetailsView
            OnCustomerSelected(this, customerPackage);
        }

        /// <summary>
        /// Handles the display of customer list through customer menu click event.
        /// </summary>
        /// <param name="sender">The customer menu that triggered this action event.</param>
        /// <param name="e">The argument passed down to display the list of customers.</param>
        private void OnCustomerMenuClicked(object sender, EventArgs e)
        {
            SelectedMenu = "Customer";
            SetCustomerListView();
        }
        /// <summary>
        /// Handles the display of resources list through resource menu click event.
        /// </summary>
        /// <param name="sender">The resource menu that triggered this action event.</param>
        /// <param name="e">The argument passed down to display the list of resources.</param>
        private void OnResourceMenuClicked(object sender, EventArgs e)
        {
            IsResourceMenuExpanded = !IsResourceMenuExpanded;
            SelectedMenu = "Resources";
        }

        /// Handles the logout function through logout menu click event.
        /// </summary>
        /// <param name="sender">The logout menu that triggered this action event.</param>
        /// <param name="e">The argument passed down to logout the current user.</param>
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

        /// <summary>
        /// Handles the home selection event that will display the landing page.
        /// </summary>
        /// <param name="sender">The company image that triggers this event.</param>
        /// <param name="e">The argument passed on to display landing page upon company image click.</param>
        private void OnHomeSelected(object sender, EventArgs e)
        {
            SetCustomerListView();
        }

        #endregion

        /// <summary>
        /// Handles the property change event.
        /// </summary>
        /// <param name="propertyName">The name of the property that is responsible to change the state of the corresponding fields.</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
