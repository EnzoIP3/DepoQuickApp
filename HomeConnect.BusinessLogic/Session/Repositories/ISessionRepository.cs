namespace BusinessLogic.Session.Repositories;

public interface ISessionRepository
{
    bool IsAuthorizationExpired(string authorizationHeader);
}
