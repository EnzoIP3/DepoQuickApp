using BusinessLogic.Auth.Models;
using BusinessLogic.Users.Entities;

namespace BusinessLogic.Auth.Services;

public interface IAuthService
{
    string CreateToken(CreateTokenArgs args);
    User GetUserFromToken(string token);
    bool IsTokenExpired(string token);
    bool Exists(string token);
}
