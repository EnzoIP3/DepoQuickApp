using BusinessLogic.Sessions.Entities;
using BusinessLogic.Sessions.Models;
using BusinessLogic.Sessions.Repositories;
using BusinessLogic.Users.Entities;
using BusinessLogic.Users.Repositories;

namespace BusinessLogic.Sessions.Services;

public class SessionService : ISessionService
{
    private readonly ISessionRepository _sessionRepository;
    private readonly IUserRepository _userRepository;

    public SessionService(ISessionRepository sessionRepository, IUserRepository userRepository)
    {
        _sessionRepository = sessionRepository;
        _userRepository = userRepository;
    }

    public string CreateSession(CreateSessionArgs args)
    {
        var user = _userRepository.GetUser(args.Email);
        if (user == null || !user.Password.Equals(args.Password))
        {
            throw new ArgumentException("Invalid email or password");
        }

        var session = new Session(user);
        _sessionRepository.Add(session);
        return session.Id.ToString();
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
