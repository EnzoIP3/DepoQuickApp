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
        var user = GetUser(email);
        EnsureUserIsNotNull(user);
        _context.Users.Remove(user!);
        _context.SaveChanges();
    }

    private User? GetUser(string email)
    {
        return _context.Users.FirstOrDefault(u => u.Email == email);
    }

    private static void EnsureUserIsNotNull(User? user)
    {
        if (user == null)
        {
            throw new ArgumentException("User does not exist");
        }
    }

    public List<User> GetUsers(int currentPage, int pageSize, string? fullNameFilter = null, string? roleFilter = null)
    {
        IQueryable<User> query = _context.Users;

        if (!string.IsNullOrEmpty(fullNameFilter))
        {
            query = query.Where(u => (u.Name + " " + u.Surname).Contains(fullNameFilter));
        }
        else if (!string.IsNullOrEmpty(roleFilter))
        {
            query = query.Where(u => u.Role.Name == roleFilter);
        }

        return query
            .Skip((currentPage - 1) * pageSize)
            .Take(pageSize)
            .ToList();
    }
}
