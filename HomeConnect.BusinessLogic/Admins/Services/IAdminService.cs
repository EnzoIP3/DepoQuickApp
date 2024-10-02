using BusinessLogic.Admins.Models;
using BusinessLogic.Users.Models;

namespace BusinessLogic.Admins.Services;

public interface IAdminService
{
    public Guid Create(CreateUserArgs createUserArgs);
    void Delete(Guid id);
    public Guid CreateBusinessOwner(CreateUserArgs createUserArgs);
    PagedData<GetUsersArgs> GetUsers(int? currentPage, int? pageSize, string? fullNameFilter, string? roleFilter);
    PagedData<GetBusinessesArgs> GetBusinesses(int? currentPage, int? pageSize, string? nameFilter, string? ownerFilter);
}
