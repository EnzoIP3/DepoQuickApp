using BusinessLogic.Sessions.Models;
using BusinessLogic.Users.Entities;

namespace BusinessLogic.Sessions.Services;

public interface ISessionService
{
    string CreateSession(CreateSessionArgs args);
    User GetUserFromSession(string sessionId);
    bool IsSessionExpired(string sessionId);
}
