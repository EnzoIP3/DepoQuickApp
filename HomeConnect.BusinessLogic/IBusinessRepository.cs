namespace BusinessLogic;

public interface IBusinessRepository
{
    List<Business> GetBusinesses(int currentPage, int pageSize, string? fullNameFilter = null, string? nameFilter = null);
}
