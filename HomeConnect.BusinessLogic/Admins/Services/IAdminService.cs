using BusinessLogic.BusinessOwners.Entities;
using BusinessLogic.Users.Entities;

namespace BusinessLogic.Admins.Services;

public interface IAdminService
{
    void DeleteAdmin(string id);
    PagedData<User> GetUsers(int? currentPage, int? pageSize, string? fullNameFilter, string? roleFilter);
    PagedData<Business> GetBusinesses(int? currentPage, int? pageSize, string? nameFilter, string? ownerFilter);
}
