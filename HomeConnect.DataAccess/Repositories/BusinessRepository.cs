using BusinessLogic;
using BusinessLogic.BusinessOwners.Entities;
using BusinessLogic.BusinessOwners.Repositories;

namespace HomeConnect.DataAccess.Repositories;

public class BusinessRepository : IBusinessRepository
{
    private readonly Context _context;

    public BusinessRepository(Context context)
    {
        _context = context;
    }

    public PagedData<Business> GetBusinesses(int page, int pageSize, string? fullNameFilter = null,
        string? nameFilter = null)
    {
        IQueryable<Business> query = _context.Businesses;
        query = FilterByOwnerFullName(fullNameFilter, query);
        query = FilterByBusinessName(nameFilter, query);
        List<Business> businesses = PaginateBusinesses(page, pageSize, query);
        return new PagedData<Business>
        {
            Data = businesses, Page = page, PageSize = pageSize, TotalPages = CalculateTotalPages(pageSize, query)
        };
    }

    public Business? GetBusinessByOwner(string ownerEmail)
    {
        return _context.Businesses.FirstOrDefault(b => b.Owner.Email == ownerEmail);
    }

    public Business? GetBusinessByRut(string rut)
    {
        return _context.Businesses.FirstOrDefault(b => b.Rut == rut);
    }

    public void Add(Business business)
    {
        EnsureBusinessDoesNotExist(business);
        _context.Businesses.Add(business);
        _context.SaveChanges();
    }

    private static List<Business> PaginateBusinesses(int page, int pageSize, IQueryable<Business> query)
    {
        return query.Skip((page - 1) * pageSize).Take(pageSize).ToList();
    }

    private static int CalculateTotalPages(int pageSize, IQueryable<Business> query)
    {
        return (int)Math.Ceiling(query.Count() / (double)pageSize);
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

    private void EnsureBusinessDoesNotExist(Business business)
    {
        if (_context.Businesses.Any(b => b.Rut == business.Rut))
        {
            throw new ArgumentException("Business with this RUT already exists.");
        }
    }
}
