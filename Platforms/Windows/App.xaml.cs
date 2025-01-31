﻿using Microsoft.UI.Xaml;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace OwlReadingRoom.WinUI
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : MauiWinUIApplication
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            Boolean redirectionActivationEnabled = Auth0.OidcClient.Platforms.Windows.Activator.Default.CheckRedirectionActivation();
            if (redirectionActivationEnabled)
                return;

            this.InitializeComponent();

            Microsoft.Maui.Handlers.WindowHandler.Mapper.AppendToMapping(nameof(IWindow), (handler, view) =>
            {
                var nativeWindow = handler.PlatformView;
                nativeWindow.ExtendsContentIntoTitleBar = true;
                nativeWindow.SetTitleBar(null);
            });
        }

        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
    }

}
