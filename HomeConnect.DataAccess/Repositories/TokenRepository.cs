using BusinessLogic.Tokens.Entities;
using BusinessLogic.Tokens.Repositories;

namespace HomeConnect.DataAccess.Repositories;

public class TokenRepository : ITokenRepository
{
    private readonly Context _context;

    public TokenRepository(Context context)
    {
        _context = context;
    }

    public Token Get(Guid token)
    {
        EnsureSessionExists(token);
        return _context.Tokens.Find(token)!;
    }

    public void Add(Token token)
    {
        _context.Tokens.Add(token);
        _context.SaveChanges();
    }

    private void EnsureSessionExists(Guid sessionId)
    {
        if (_context.Tokens.Find(sessionId) == null)
        {
            throw new ArgumentException("Session does not exist");
        }
    }
}
