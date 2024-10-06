namespace OwlReadingRoom
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Overrides the back button functionality at shell level.
        /// </summary>
        /// <returns>The boolean flag indicating whether to act when back button is pressed or not.</returns>
        protected override bool OnBackButtonPressed()
        {
            // true or false to disable or enable the action
            return false;
        }
    }
}
