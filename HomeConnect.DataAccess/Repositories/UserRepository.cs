using BusinessLogic;
using BusinessLogic.Users.Entities;
using BusinessLogic.Users.Repositories;

namespace HomeConnect.DataAccess.Repositories;

public class UserRepository : PaginatedRepositoryBase<User>, IUserRepository
{
    private readonly Context _context;

    public UserRepository(Context context)
        : base(context)
    {
        _context = context;
    }

    public PagedData<User> GetAllPaged(int currentPage, int pageSize, string? fullNameFilter = null,
        string? roleFilter = null)
    {
        var filters = new object[2];
        filters[0] = fullNameFilter ?? string.Empty;
        filters[1] = roleFilter ?? string.Empty;
        return GetAllPaged(currentPage, pageSize, filters);
    }

    public void Add(User user)
    {
        EnsureUserDoesNotExist(user);
        _context.Users.Add(user);
        _context.SaveChanges();
    }

    public User Get(Guid id)
    {
        User? user = _context.Users.FirstOrDefault(u => u.Id == id);
        if (user == null)
        {
            throw new ArgumentException("User does not exist");
        }

        return user;
    }

    public bool Exists(Guid id)
    {
        return _context.Users.Any(u => u.Id == id);
    }

    public void Delete(Guid id)
    {
        User user = Get(id);
        _context.Users.Remove(user);
        _context.SaveChanges();
    }

    public User? GetUser(string email)
    {
        return _context.Users.FirstOrDefault(u => u.Email == email);
    }

    public bool Exists(string email)
    {
        return _context.Users.Any(u => u.Email == email);
    }

    public void Delete(string email)
    {
        User? user = GetUser(email);
        EnsureUserIsNotNull(user);
        _context.Users.Remove(user!);
        _context.SaveChanges();
    }

    protected override IQueryable<User> GetQueryable()
    {
        return _context.Users;
    }

    protected override IQueryable<User> ApplyFilters(IQueryable<User> query, params object[] filters)
    {
        var fullNameFilter = filters.Length > 0 ? filters[0] as string : null;
        var roleFilter = filters.Length > 1 ? filters[1] as string : null;

        query = FilterByFullName(fullNameFilter, query);
        query = FilterByRole(roleFilter, query);

        return query;
    }

    private void EnsureUserDoesNotExist(User user)
    {
        if (Exists(user.Email))
        {
            throw new ArgumentException("User with this email already exists.");
        }
    }

    private static void EnsureUserIsNotNull(User? user)
    {
        if (user == null)
        {
            throw new ArgumentException("User does not exist");
        }
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
