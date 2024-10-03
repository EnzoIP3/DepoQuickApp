using BusinessLogic.Users.Entities;

namespace BusinessLogic.Sessions.Services;

public interface ISessionService
{
    User? GetUserFromSession(string sessionId);
    bool IsSessionExpired(string sessionId);
}
