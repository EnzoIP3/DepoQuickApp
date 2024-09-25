using BusinessLogic;
using Microsoft.EntityFrameworkCore;

namespace HomeConnect.DataAccess;

public class UserRepository : IUserRepository
{
    private readonly Context _context;

    public UserRepository(Context context)
    {
        _context = context;
    }

    public void Add(User user)
    {
        if (Exists(user.Email))
        {
            throw new ArgumentException("User with this email already exists.");
        }

        _context.Users.Add(user);
        _context.SaveChanges();
    }

    public bool Exists(string email)
    {
        return _context.Users.Any(u => u.Email == email);
    }

    public void Delete(string email)
    {
        throw new NotImplementedException();
    }

    public List<User> GetUsers(int currentPage, int pageSize, string? fullNameFilter = null, string? roleFilter = null)
    {
        throw new NotImplementedException();
    }
}
