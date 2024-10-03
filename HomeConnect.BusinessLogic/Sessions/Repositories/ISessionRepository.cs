namespace BusinessLogic.Sessions.Repositories;

public interface ISessionRepository
{
    bool IsAuthorizationExpired(string authorizationHeader);
}
