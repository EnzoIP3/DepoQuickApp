using BusinessLogic;

namespace HomeConnect.DataAccess;

public class BusinessRepository : IBusinessRepository
{
    private readonly Context _context;

    public BusinessRepository(Context context)
    {
        _context = context;
    }

    public List<Business> GetBusinesses(int page, int pageSize, string? fullNameFilter = null,
        string? nameFilter = null)
    {
        IQueryable<Business> query = _context.Businesses;

        if (!string.IsNullOrWhiteSpace(fullNameFilter))
        {
            query = query.Where(b => (b.Owner.Name + " " + b.Owner.Surname).Contains(fullNameFilter));
        }

        return query.Skip((page - 1) * pageSize).Take(pageSize).ToList();
    }
}
