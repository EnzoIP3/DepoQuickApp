using BusinessLogic;
using BusinessLogic.Users.Entities;

namespace HomeConnect.WebApi.Filters;

public interface IAuthRepository
{
    bool IsAuthorizationExpired(string authorizationHeader);
    User? GetUserOfAuthorization(string authorizationHeader);
}
