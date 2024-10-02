using BusinessLogic;
using BusinessLogic.Users.Entities;
using BusinessLogic.Users.Repositories;

namespace HomeConnect.DataAccess.Users;

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

    public User Get(Guid id)
    {
        throw new NotImplementedException();
    }

    public bool Exists(Guid id)
    {
        throw new NotImplementedException();
    }

    public void Delete(Guid id)
    {
        throw new NotImplementedException();
    }

    PagedData<User> IUserRepository.GetUsers(int currentPage, int pageSize, string? fullNameFilter, string? roleFilter)
    {
        throw new NotImplementedException();
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

    public User? GetUser(string email)
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
        query = FilterByFullName(fullNameFilter, query);
        query = FilterByRole(roleFilter, query);
        return query
            .Skip((currentPage - 1) * pageSize)
            .Take(pageSize)
            .ToList();
    }

    private static IQueryable<User> FilterByRole(string? roleFilter, IQueryable<User> query)
    {
        if (!string.IsNullOrEmpty(roleFilter))
        {
            query = query.Where(u => u.Role.Name == roleFilter);
        }

        return query;
    }

    private static IQueryable<User> FilterByFullName(string? fullNameFilter, IQueryable<User> query)
    {
        if (!string.IsNullOrEmpty(fullNameFilter))
        {
            query = query.Where(u => (u.Name + " " + u.Surname).Contains(fullNameFilter));
        }

        return query;
    }
}
