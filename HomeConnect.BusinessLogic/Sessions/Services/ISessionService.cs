using BusinessLogic.Users.Entities;

namespace BusinessLogic.Sessions.Services;

public interface ISessionService
{
    User? GetUserByToken(string token);
}
