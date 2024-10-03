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

    public User GetUserFromSession(string sessionId)
    {
        var session = _sessionRepository.Get(Guid.Parse(sessionId));
        return session.User;
    }

    public bool IsSessionExpired(string sessionId)
    {
        throw new NotImplementedException();
    }
}
