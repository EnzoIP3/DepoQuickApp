using BusinessLogic;
using BusinessLogic.Users.Entities;
using BusinessLogic.Users.Repositories;

namespace HomeConnect.DataAccess.Repositories;

public class UserRepository : IUserRepository
{
    private readonly Context _context;

    public UserRepository(Context context)
    {
        _context = context;
    }

    public User? GetByEmail(string email)
    {
        throw new NotImplementedException();
    }

    public void Add(User user)
    {
        _context.Users.Add(user);
        _context.SaveChanges();
    }

    public bool Exists(Guid id)
    {
        return _context.Users.Any(u => u.Id == id);
    }

    public void Delete(Guid id)
    {
        var user = Get(id);
        _context.Users.Remove(user);
        _context.SaveChanges();
    }

    public User Get(Guid id)
    {
        return _context.Users.First(u => u.Id == id);
    }

    public PagedData<User> GetUsers(int currentPage, int pageSize, string? fullNameFilter = null,
        string? roleFilter = null)
    {
        IQueryable<User> query = _context.Users;
        query = FilterByFullName(fullNameFilter, query);
        query = FilterByRole(roleFilter, query);
        return new PagedData<User>()
        {
            Data = query
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize)
                .ToList(),
            Page = currentPage,
            PageSize = pageSize,
            TotalPages = (int)Math.Ceiling(query.Count() / (double)pageSize)
        };
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
