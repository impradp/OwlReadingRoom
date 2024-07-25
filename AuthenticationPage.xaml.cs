using Auth0.OidcClient;
using Microsoft.Extensions.Configuration;
using OwlReadingRoom.Utils;

namespace OwlReadingRoom;

public partial class AuthenticationPage : ContentPage
{
    private readonly Auth0Client _client;
    private readonly IConfiguration _configuration;
    public AuthenticationPage(IConfiguration configuration)
	{
        InitializeComponent();
        _configuration = configuration;
        _client = Auth0Handler.GetAuth0Client(configuration);
    }

    private async void OnLoginClicked(object sender, EventArgs e)
    {

        var extraParameters = new Dictionary<string, string>();
        var audience = ""; // FILL WITH AUDIENCE AS NEEDED

        if (!string.IsNullOrEmpty(audience))
            extraParameters.Add("audience", audience);

        var result = await _client.LoginAsync(extraParameters);
        var resultView = new MainView(result,_client, _configuration);

        // Navigate to the main view
        await Navigation.PushAsync(resultView);

    }
}
