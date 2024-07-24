using OwlReadingRoom.Views;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace OwlReadingRoom
{
    public partial class MainPage : ContentPage, INotifyPropertyChanged
    {
        private string _selectedMenu="";
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

        public MainPage()
        {
            InitializeComponent();
            BindingContext = this;
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

        private void OnLogoutMenuClicked(object sender, EventArgs e)
        {
            SelectedMenu = "Logout";
            DynamicContentArea.Content = new ProfileView();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
