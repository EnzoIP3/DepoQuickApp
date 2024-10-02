using BusinessLogic;
using BusinessLogic.Users.Entities;

namespace HomeConnect.WebApi.Session;

public interface ISessionService
{
    User? GetUserByToken(string token);
}
