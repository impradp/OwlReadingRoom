namespace OwlReadingRoom
{
    public partial class App : Application
    {
        public App(AuthenticationPage authenticationPage)
        {
            InitializeComponent();

            MainPage = new NavigationPage(authenticationPage);
        }
    }
}
