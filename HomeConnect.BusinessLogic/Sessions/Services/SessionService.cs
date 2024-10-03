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
        EnsureSessionIdIsValidGuid(sessionId);
        var session = _sessionRepository.Get(Guid.Parse(sessionId));
        return session.User;
    }

    private static void EnsureSessionIdIsValidGuid(string sessionId)
    {
        if (!Guid.TryParse(sessionId, out _))
        {
            throw new ArgumentException("Invalid session id");
        }
    }

    public bool IsSessionExpired(string sessionId)
    {
        throw new NotImplementedException();
    }
}
