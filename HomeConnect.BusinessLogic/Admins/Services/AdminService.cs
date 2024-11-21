using BusinessLogic.Admins.Models;
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
        if (UserRepository.GetPaged(1, 1, roleFilter: Role.Admin).TotalPages == 1)
        {
            throw new InvalidOperationException("The last admin cannot be deleted");
        }
    }

    public PagedData<User> GetUsers(GetUsersArgs args)
    {
        PagedData<User> users =
            UserRepository.GetPaged(args.CurrentPage ?? 1, args.PageSize ?? 10, args.FullNameFilter, args.RoleFilter);
        return users;
    }

    public PagedData<Business> GetBusinesses(GetBusinessesArgs args)
    {
        var filterArgs = new FilterArgs
        {
            CurrentPage = args.CurrentPage ?? 1,
            PageSize = args.PageSize ?? 10,
            NameFilter = args.NameFilter,
            FullNameFilter = args.FullNameFilter
        };
        PagedData<Business> businesses =
            BusinessRepository.GetPaged(filterArgs);
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
