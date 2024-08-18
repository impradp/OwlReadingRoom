using IdentityModel.OidcClient;
using OwlReadingRoom.Models;

namespace OwlReadingRoom.Services;

public interface IUserService
{
    User CurrentUser { get; set; }
    void SetUserInfo(LoginResult loginResult);
    void ClearUserInfo();
}
