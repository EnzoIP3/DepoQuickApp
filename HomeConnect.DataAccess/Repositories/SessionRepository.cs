using BusinessLogic.Sessions.Entities;
using BusinessLogic.Sessions.Repositories;

namespace HomeConnect.DataAccess.Repositories;

public class SessionRepository : ISessionRepository
{
    private readonly Context _context;

    public SessionRepository(Context context)
    {
        _context = context;
    }

    public Session Get(Guid sessionId)
    {
        return _context.Sessions.Find(sessionId);
    }

    public void Add(Session session)
    {
        _context.Sessions.Add(session);
        _context.SaveChanges();
    }
}
