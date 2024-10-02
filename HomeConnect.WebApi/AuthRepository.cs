using BusinessLogic.Users.Entities;
using HomeConnect.WebApi.Filters;
namespace HomeConnect.WebApi;

public class AuthRepository : IAuthRepository
{
    public bool IsAuthorizationExpired(string authorizationHeader)
    {
        throw new NotImplementedException();
    }

    public User? GetUserOfAuthorization(string authorizationHeader)
    {
        throw new NotImplementedException();
    }
}
