using Auth0.OidcClient;
using Microsoft.Extensions.Configuration;

namespace OwlReadingRoom.Utils
{
    public static class Auth0Handler
    {
        public static Auth0Client GetAuth0Client(IConfiguration configuration)
        {
            var settings = configuration.GetRequiredSection("Auth0");

            return new Auth0Client(new Auth0ClientOptions
            {
                Domain = settings.GetSection("Domain").Value,
                ClientId = settings.GetSection("ClientId").Value,
                Scope = settings.GetSection("Scope").Value,
                RedirectUri = settings.GetSection("RedirectUri").Value,
                PostLogoutRedirectUri = settings.GetSection("PostLogoutRedirectUri").Value
            });
        }
    }
}
