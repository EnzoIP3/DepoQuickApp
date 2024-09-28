using BusinessLogic;
using Microsoft.Extensions.Primitives;

namespace HomeConnect.WebApi.Filters;

public interface IAuthRepository
{
    bool IsAuthorizationExpired(string authorizationHeader);
    User? GetUserOfAuthorization(string authorizationHeader);
}
