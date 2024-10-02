using BusinessLogic.BusinessOwners.Entities;

namespace BusinessLogic.BusinessOwners.Repositories;

public interface IBusinessRepository
{
    void Add(Business business);
    PagedData<Business> GetBusinesses(int currentPage, int pageSize, string? fullNameFilter = null, string? nameFilter = null);
    Business? GetBusinessByOwner(string ownerEmail);
    Business? GetBusinessByRut(string rut);
}
