using BusinessLogic;
using Microsoft.EntityFrameworkCore;

namespace HomeConnect.DataAccess;

public class UserRepository
{
    private readonly DbContext _context;

    public UserRepository(DbContext context)
    {
        _context = context;
    }

    public bool Exists(string email)
    {
        return _context.Set<User>().Any(u => u.Email == email);
    }
}
