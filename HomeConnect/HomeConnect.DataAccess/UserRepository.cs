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
        EnsureUserDoesNotExist(user);
        _context.Users.Add(user);
        _context.SaveChanges();
    }

    private void EnsureUserDoesNotExist(User user)
    {
        if (Exists(user.Email))
        {
            throw new ArgumentException("User with this email already exists.");
        }
    }

    public bool Exists(string email)
    {
        return _context.Users.Any(u => u.Email == email);
    }

    public void Delete(string email)
    {
        var user = _context.Users.FirstOrDefault(u => u.Email == email);
        if (user != null)
        {
            _context.Users.Remove(user);
            _context.SaveChanges();
        }
    }

    public List<User> GetUsers(int currentPage, int pageSize, string? fullNameFilter = null, string? roleFilter = null)
    {
        throw new NotImplementedException();
    }
}
