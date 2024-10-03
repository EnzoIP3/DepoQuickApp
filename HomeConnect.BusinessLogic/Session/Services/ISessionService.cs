using BusinessLogic.Users.Entities;

namespace BusinessLogic.Session.Services;

public interface ISessionService
{
    User? GetUserByToken(string token);
}
