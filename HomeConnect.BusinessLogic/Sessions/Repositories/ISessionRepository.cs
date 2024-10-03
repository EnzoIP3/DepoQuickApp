using BusinessLogic.Sessions.Entities;

namespace BusinessLogic.Sessions.Repositories;

public interface ISessionRepository
{
    Session Get(Guid sessionId);
    void Add(Session session);
}
