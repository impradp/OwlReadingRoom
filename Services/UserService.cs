using IdentityModel.OidcClient;
using OwlReadingRoom.Models;

namespace OwlReadingRoom.Services;

public class UserService : IUserService
{
    public User CurrentUser { get; set; }
    public void ClearUserInfo()
    {
        CurrentUser = null;
    }

    public void SetUserInfo(LoginResult loginResult)
    {
        if (!loginResult.IsError)
        {
            CurrentUser = new User();
            var authUser = loginResult.User;
            CurrentUser.Name = authUser.FindFirst(c => c.Type == "name")?.Value;
            CurrentUser.Email = authUser.FindFirst(c => c.Type == "email")?.Value;
            CurrentUser.Role = authUser.FindFirst(c => c.Type == "roles")?.Value
                ?? authUser.FindFirst(c => c.Type == "role")?.Value
                ?? "Guest";
            string GivenName = authUser.FindFirst(c => c.Type == "given_name")?.Value;
        }
        
    }
}
