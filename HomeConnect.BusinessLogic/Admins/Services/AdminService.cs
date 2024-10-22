using BusinessLogic.BusinessOwners.Entities;
using BusinessLogic.BusinessOwners.Repositories;
using BusinessLogic.Roles.Entities;
using BusinessLogic.Roles.Repositories;
using BusinessLogic.Users.Entities;
using BusinessLogic.Users.Repositories;

namespace BusinessLogic.Admins.Services;

public class AdminService : IAdminService
{
    public AdminService(IUserRepository userRepository, IBusinessRepository businessRepository,
        IRoleRepository roleRepository)
    {
        UserRepository = userRepository;
        BusinessRepository = businessRepository;
        RoleRepository = roleRepository;
    }

    private IUserRepository UserRepository { get; }
    private IBusinessRepository BusinessRepository { get; }
    private IRoleRepository RoleRepository { get; }

    public void DeleteAdmin(string adminIdStr)
    {
        EnsureValidGuid(adminIdStr, out Guid adminId);
        EnsureEntityExists(adminId);
        EnsureOtherAdminExists();
        UserRepository.Delete(adminId);
    }

    private void EnsureOtherAdminExists()
    {
        if (UserRepository.GetPaged(1, 1, null, Role.Admin).TotalPages == 1)
        {
            throw new InvalidOperationException("The last admin cannot be deleted");
        }
    }

    public PagedData<User> GetUsers(int? currentPage = null, int? pageSize = null, string? fullNameFilter = null,
        string? roleFilter = null)
    {
        PagedData<User> users =
            UserRepository.GetPaged(currentPage ?? 1, pageSize ?? 10, fullNameFilter, roleFilter);
        return users;
    }

    public PagedData<Business> GetBusinesses(int? currentPage = null, int? pageSize = null, string? nameFilter = null,
        string? fullNameFilter = null)
    {
        PagedData<Business> businesses =
            BusinessRepository.GetPaged(currentPage ?? 1, pageSize ?? 10, fullNameFilter, nameFilter);
        return businesses;
    }

    private void EnsureEntityExists(Guid id)
    {
        if (!UserRepository.Exists(id))
        {
            throw new KeyNotFoundException("Admin does not exist.");
        }
    }

    private static void EnsureValidGuid(string idStr, out Guid id)
    {
        if (!Guid.TryParse(idStr, out id))
        {
            throw new ArgumentException("The id is not a valid GUID.");
        }
    }
}
