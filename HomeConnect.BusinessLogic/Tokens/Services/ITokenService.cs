using BusinessLogic.Tokens.Models;
using BusinessLogic.Users.Entities;

namespace BusinessLogic.Tokens.Services;

public interface ITokenService
{
    string GetToken(GetTokenArgs args);
    User GetUserFromToken(string token);
    bool IsTokenExpired(string token);
}
