using Auth0.OidcClient;
using IdentityModel.OidcClient;
using IdentityModel.OidcClient.Browser;
using Microsoft.Extensions.Configuration;
using OwlReadingRoom.Models;
using OwlReadingRoom.Views;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace OwlReadingRoom
{
    public partial class MainView : ContentPage, INotifyPropertyChanged
    {
        private string _selectedMenu="";

        private readonly LoginResult _loginResult;

        private readonly Auth0Client _auth0Client;

        private readonly IConfiguration _configuration;

        public MainView(LoginResult loginResult, Auth0Client auth0Client, IConfiguration configuration)
        {
            InitializeComponent();
            _loginResult = loginResult;
            BindingContext = this;
            _auth0Client = auth0Client;
            _configuration = configuration;
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
            DynamicContentArea.Content = new NewEntryView();
        }

        private void OnCustomerMenuClicked(object sender, EventArgs e)
        {
            SelectedMenu = "Customer";
            DynamicContentArea.Content = new CustomerView();
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

            // TODO: Popup error on logout issue
            if (!browserResult.Equals(BrowserResultType.Success))
            {
                // throw new Exception();
            }
            var resultPage = new AuthenticationPage(_configuration);

            // Navigate to the result page
            await Navigation.PushAsync(resultPage);
            // Clear the navigation stack
            Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 2]);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
