namespace BusinessLogic;

public interface IBusinessRepository
{
    PagedData<Business> GetBusinesses(int currentPage, int pageSize, string? fullNameFilter = null, string? nameFilter = null);
}
