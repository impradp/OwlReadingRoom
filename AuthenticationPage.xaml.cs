using Auth0.OidcClient;

namespace OwlReadingRoom;

public partial class AuthenticationPage : ContentPage
{
    private readonly Auth0Client _auth0Client;
    private readonly IServiceProvider _serviceProvider;
    public AuthenticationPage(Auth0Client auth0Client, IServiceProvider serviceProvider)
    {
        InitializeComponent();
        _auth0Client = auth0Client;
        _serviceProvider = serviceProvider;
    }

    private async void OnLoginClicked(object sender, EventArgs e)
    {

        var extraParameters = new Dictionary<string, string>();
        var audience = ""; // FILL WITH AUDIENCE AS NEEDED

        if (!string.IsNullOrEmpty(audience))
            extraParameters.Add("audience", audience);

        var result = await _auth0Client.LoginAsync(extraParameters);
        var resultView = new MainView(result, _auth0Client, _serviceProvider);

        // Navigate to the main view
        await Navigation.PushAsync(resultView);

    }
}
