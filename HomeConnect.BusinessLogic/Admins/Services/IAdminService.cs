using BusinessLogic.BusinessOwners.Entities;
using BusinessLogic.Users.Entities;
using BusinessLogic.Users.Models;

namespace BusinessLogic.Admins.Services;

public interface IAdminService
{
    public Guid Create(CreateUserArgs createUserArgs);
    void Delete(string id);
    public Guid CreateBusinessOwner(CreateUserArgs createUserArgs);
    PagedData<User> GetUsers(int? currentPage, int? pageSize, string? fullNameFilter, string? roleFilter);
    PagedData<Business> GetBusinesses(int? currentPage, int? pageSize, string? nameFilter, string? ownerFilter);
}
