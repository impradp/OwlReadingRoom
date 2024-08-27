using Auth0.OidcClient;
using OwlReadingRoom.Services;
using OwlReadingRoom.Utils;

namespace OwlReadingRoom.Views;

public partial class AuthenticationPage : ContentPage
{
    private readonly Auth0Client _auth0Client;
    private readonly IServiceProvider _serviceProvider;
    private readonly IUserService _userService;
    public AuthenticationPage(Auth0Client auth0Client, IServiceProvider serviceProvider, IUserService userService)
    {
        InitializeComponent();
        _auth0Client = auth0Client;
        _serviceProvider = serviceProvider;
        _userService = userService;
    }

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        // Show the activity indicator
        Loader.IsLoading = true;

        try
        {

            var extraParameters = new Dictionary<string, string>();
            var audience = ""; // FILL WITH AUDIENCE AS NEEDED

            if (!string.IsNullOrEmpty(audience))
                extraParameters.Add("audience", audience);

            var result = await _auth0Client.LoginAsync(extraParameters);
            _userService.SetUserInfo(result);
            var resultView = new MainView(result, _auth0Client, _serviceProvider);

            // Navigate to the main view
            await Navigation.PushAsync(resultView);
        }
        catch (Exception ex)
        {
            // Handle any exceptions
            ExceptionHandler.HandleException("Logging into system", ex);
        }
        finally
        {
            // Hide the activity indicator
            Loader.IsLoading = false;
        }

    }
}
