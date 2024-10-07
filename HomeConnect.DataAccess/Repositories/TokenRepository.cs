using BusinessLogic.Auth.Entities;
using BusinessLogic.Auth.Repositories;
using Microsoft.EntityFrameworkCore;

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
        return _context.Tokens.Include(t => t.User).ThenInclude(u => u.Role).ThenInclude(r => r.Permissions)
            .First(t => t.Id == token);
    }

    public void Add(Token token)
    {
        _context.Tokens.Add(token);
        _context.SaveChanges();
    }

    public bool Exists(Guid token)
    {
        throw new NotImplementedException();
    }

    private void EnsureSessionExists(Guid sessionId)
    {
        if (_context.Tokens.Find(sessionId) == null)
        {
            throw new ArgumentException("Session does not exist");
        }
    }
}
