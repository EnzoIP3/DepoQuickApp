using BusinessLogic;
using BusinessLogic.Users.Entities;
using BusinessLogic.Users.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HomeConnect.DataAccess.Repositories;

public class UserRepository : PaginatedRepositoryBase<User>, IUserRepository
{
    private readonly Context _context;

    public UserRepository(Context context)
        : base(context)
    {
        _context = context;
    }

    public PagedData<User> GetPaged(int currentPage, int pageSize, string? fullNameFilter = null,
        string? roleFilter = null)
    {
        var filters = new object[2];
        filters[0] = fullNameFilter ?? string.Empty;
        filters[1] = roleFilter ?? string.Empty;
        return GetAllPaged(currentPage, pageSize, filters);
    }

    public User GetByEmail(string email)
    {
        return _context.Users.First(u => u.Email == email);
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

    public bool ExistsByEmail(string email)
    {
        return _context.Users.Any(u => u.Email == email);
    }

    public void Delete(Guid id)
    {
        User user = Get(id);
        _context.Users.Remove(user);
        _context.SaveChanges();
    }

    public User Get(Guid id)
    {
        return _context.Users.First(u => u.Id == id);
    }

    protected override IQueryable<User> GetQueryable()
    {
        return _context.Users.Include(u => u.Roles);
    }

    protected override IQueryable<User> ApplyFilters(IQueryable<User> query, params object[] filters)
    {
        var fullNameFilter = filters.Length > 0 ? filters[0] as string : null;
        var roleFilter = filters.Length > 1 ? filters[1] as string : null;

        query = FilterByFullName(fullNameFilter, query);
        query = FilterByRole(roleFilter, query);

        return query;
    }

    private static IQueryable<User> FilterByRole(string? roleFilter, IQueryable<User> query)
    {
        if (!string.IsNullOrEmpty(roleFilter))
        {
            query = query.Where(u => u.Roles.Any(r => r.Name.Contains(roleFilter)));
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

    public void Update(User user)
    {
        _context.Users.Update(user);
        _context.SaveChanges();
    }
}
