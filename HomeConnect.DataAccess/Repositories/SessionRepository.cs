using BusinessLogic.Session.Repositories;
using BusinessLogic.Users.Entities;

namespace HomeConnect.DataAccess.Repositories;

public class SessionRepository : ISessionRepository
{
    public bool IsAuthorizationExpired(string authorizationHeader)
    {
        throw new NotImplementedException();
    }
}
