using BusinessLogic;

namespace HomeConnect.WebApi.Session;

public interface ISessionService
{
    User? GetUserByToken(string token);
}
