using BusinessLogic;

namespace HomeConnect.DataAccess.Repositories;

public abstract class PaginatedRepositoryBase<T>
    where T : class
{
    protected readonly Context Context;

    protected PaginatedRepositoryBase(Context context)
    {
        Context = context;
    }

    public PagedData<T> GetAllPaged(int currentPage, int pageSize, params object[] filters)
    {
        IQueryable<T> query = GetQueryable();
        query = ApplyFilters(query, filters);
        return Paginate(query, currentPage, pageSize);
    }

    protected abstract IQueryable<T> GetQueryable();
    protected abstract IQueryable<T> ApplyFilters(IQueryable<T> query, params object[] filters);

    private static PagedData<T> Paginate(IQueryable<T> query, int currentPage, int pageSize)
    {
        var totalItems = query.Count();
        var items = query.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();

        return new PagedData<T>
        {
            Data = items,
            Page = currentPage,
            PageSize = pageSize,
            TotalPages = CalculateTotalPages(pageSize, totalItems)
        };
    }

    private static int CalculateTotalPages(int pageSize, int totalItems)
    {
        return (int)Math.Ceiling((double)totalItems / pageSize);
    }
}
