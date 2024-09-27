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
        query = FilterByOwnerFullName(fullNameFilter, query);
        query = FilterByBusinessName(nameFilter, query);
        return query.Skip((page - 1) * pageSize).Take(pageSize).ToList();
    }

    public Business? GetBusinessByOwner(string ownerEmail)
    {
        throw new NotImplementedException();
    }

    public Business? GetBusinessByRut(string rut)
    {
        throw new NotImplementedException();
    }

    private static IQueryable<Business> FilterByBusinessName(string? nameFilter, IQueryable<Business> query)
    {
        if (!string.IsNullOrWhiteSpace(nameFilter))
        {
            query = query.Where(b => b.Name.Contains(nameFilter));
        }

        return query;
    }

    private static IQueryable<Business> FilterByOwnerFullName(string? fullNameFilter, IQueryable<Business> query)
    {
        if (!string.IsNullOrWhiteSpace(fullNameFilter))
        {
            query = query.Where(b => (b.Owner.Name + " " + b.Owner.Surname).Contains(fullNameFilter));
        }

        return query;
    }

    public void Add(Business business)
    {
        if (_context.Businesses.Any(b => b.Rut == business.Rut))
        {
            throw new ArgumentException("Business with this RUT already exists.");
        }

        _context.Businesses.Add(business);
        _context.SaveChanges();
    }
}
