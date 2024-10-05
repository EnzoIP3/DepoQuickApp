using BusinessLogic;
using BusinessLogic.BusinessOwners.Entities;
using BusinessLogic.BusinessOwners.Repositories;

namespace HomeConnect.DataAccess.Repositories;

public class BusinessRepository : PaginatedRepositoryBase<Business>, IBusinessRepository
{
    public BusinessRepository(Context context)
        : base(context)
    {
    }

    public PagedData<Business> GetPagedData(int currentPage, int pageSize, string? fullNameFilter = null,
        string? nameFilter = null)
    {
        var filters = new object[2];
        filters[0] = fullNameFilter ?? string.Empty;
        filters[1] = nameFilter ?? string.Empty;
        return GetAllPaged(currentPage, pageSize, filters);
    }

    public Business? GetBusinessByOwnerId(string ownerEmail)
    {
        return Context.Businesses.FirstOrDefault(b => b.Owner.Email == ownerEmail);
    }

    public Business Get(string rut)
    {
        return Context.Businesses.First(b => b.Rut == rut);
    }

    public bool Exists(string rut)
    {
        return Context.Businesses.Any(b => b.Rut == rut);
    }

    public Business GetByOwnerId(Guid ownerId)
    {
        return Context.Businesses.First(b => b.Owner.Id == ownerId);
    }

    public bool ExistsByOwnerId(Guid ownerId)
    {
        throw new NotImplementedException();
    }

    public void Add(Business business)
    {
        EnsureBusinessDoesNotExist(business);
        Context.Businesses.Add(business);
        Context.SaveChanges();
    }

    protected override IQueryable<Business> GetQueryable()
    {
        return Context.Businesses;
    }

    protected override IQueryable<Business> ApplyFilters(IQueryable<Business> query, params object[] filters)
    {
        var fullNameFilter = filters.Length > 0 ? filters[0] as string : null;
        var nameFilter = filters.Length > 1 ? filters[1] as string : null;

        query = FilterByOwnerFullName(fullNameFilter, query);
        query = FilterByBusinessName(nameFilter, query);

        return query;
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
        if (Context.Businesses.Any(b => b.Rut == business.Rut))
        {
            throw new ArgumentException("Business with this RUT already exists.");
        }
    }
}
