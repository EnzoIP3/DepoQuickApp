using BusinessLogic.Sessions.Repositories;
using BusinessLogic.Users.Entities;

namespace BusinessLogic.Sessions.Services;

public class SessionService : ISessionService
{
    private readonly ISessionRepository _sessionRepository;

    public SessionService(ISessionRepository sessionRepository)
    {
        _sessionRepository = sessionRepository;
    }

    public User? GetUserFromSession(string sessionId)
    {
        throw new NotImplementedException();
    }

    public bool IsSessionExpired(string sessionId)
    {
        throw new NotImplementedException();
    }
}
