using BusinessLogic.Admins.Models;
using BusinessLogic.BusinessOwners.Entities;
using BusinessLogic.BusinessOwners.Repositories;
using BusinessLogic.Roles.Entities;
using BusinessLogic.Users.Entities;
using BusinessLogic.Users.Repositories;

namespace BusinessLogic.Admins.Services;

public class AdminService : IAdminService
{
    private readonly IBusinessRepository _businessRepository;

    private readonly IUserRepository _userRepository;

    public AdminService(IUserRepository userRepository, IBusinessRepository businessRepository)
    {
        _userRepository = userRepository;
        _businessRepository = businessRepository;
    }

    public void DeleteAdmin(string adminIdStr)
    {
        EnsureValidGuid(adminIdStr, out Guid adminId);
        EnsureEntityExists(adminId);
        EnsureOtherAdminExists();
        _userRepository.Delete(adminId);
    }

    private void EnsureOtherAdminExists()
    {
        var filterArgs = new FilterArgs { PageSize = 1, CurrentPage = 1, RoleFilter = Role.Admin };
        if (_userRepository.GetPaged(filterArgs).TotalPages == 1)
        {
            throw new InvalidOperationException("The last admin cannot be deleted");
        }
    }

    public PagedData<User> GetUsers(GetUsersArgs args)
    {
        var filterArgs = new FilterArgs { FullNameFilter = args.FullNameFilter, RoleFilter = args.RoleFilter, };
        filterArgs.CurrentPage = args.CurrentPage ?? filterArgs.CurrentPage;
        filterArgs.PageSize = args.PageSize ?? filterArgs.PageSize;
        PagedData<User> users =
            _userRepository.GetPaged(filterArgs);
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
            _businessRepository.GetPaged(filterArgs);
        return businesses;
    }

    private void EnsureEntityExists(Guid id)
    {
        if (!_userRepository.Exists(id))
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
