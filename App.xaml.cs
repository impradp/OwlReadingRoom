using OwlReadingRoom.Services.Startup;

namespace OwlReadingRoom
{
    public partial class App : Application
    {
        private readonly StartupCoordinator _startupCoordinator;

        public App(StartupCoordinator startupCoordinator)
        {
            InitializeComponent();
            _startupCoordinator = startupCoordinator;
            Application.Current.UserAppTheme = AppTheme.Light;
            MainPage = new AppShell();
        }

        protected override async void OnStart()
        {
            base.OnStart();
            await _startupCoordinator.ExecuteStartupTasksAsync();
        }
    }
}
