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
        EnsureSessionExists(sessionId);
        return _context.Sessions.Find(sessionId)!;
    }

    private void EnsureSessionExists(Guid sessionId)
    {
        if (_context.Sessions.Find(sessionId) == null)
        {
            throw new ArgumentException("Session does not exist");
        }
    }

    public void Add(Session session)
    {
        _context.Sessions.Add(session);
        _context.SaveChanges();
    }
}
