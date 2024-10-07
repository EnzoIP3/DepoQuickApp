using BusinessLogic.BusinessOwners.Entities;
using BusinessLogic.BusinessOwners.Repositories;
using BusinessLogic.Roles.Entities;
using BusinessLogic.Roles.Repositories;
using BusinessLogic.Users.Entities;
using BusinessLogic.Users.Models;
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

    public Guid Create(CreateUserArgs args)
    {
        ValidateUserArgs(args);
        EnsureUserEmailIsUnique(args.Email);

        Role role = GetRoleByName(args.Role);
        var admin = CreateNewUser(args, role);

        UserRepository.Add(admin);
        return admin.Id;
    }

    public Guid CreateBusinessOwner(CreateUserArgs args)
    {
        ValidateUserArgs(args);
        EnsureUserEmailIsUnique(args.Email);

        Role role = GetRoleByName(args.Role);
        var businessOwner = CreateNewUser(args, role);

        UserRepository.Add(businessOwner);
        return businessOwner.Id;
    }

    public void Delete(string adminIdStr)
    {
        EnsureValidGuid(adminIdStr, out Guid adminId);
        EnsureEntityExists(adminId);

        UserRepository.Delete(adminId);
    }

    public PagedData<User> GetUsers(int? currentPage = 1, int? pageSize = 10, string? fullNameFilter = null,
        string? roleFilter = null)
    {
        PagedData<User> users =
            UserRepository.GetAllPaged(currentPage!.Value, pageSize!.Value, fullNameFilter, roleFilter);
        return users;
    }

    public PagedData<Business> GetBusinesses(int? currentPage = 1, int? pageSize = 10, string? nameFilter = null,
        string? fullNameFilter = null)
    {
        PagedData<Business> businesses =
            BusinessRepository.GetPagedData(currentPage!.Value, pageSize!.Value, fullNameFilter, nameFilter);
        return businesses;
    }

    private static void ValidateUserArgs(CreateUserArgs args)
    {
        if (string.IsNullOrWhiteSpace(args.Name) ||
            string.IsNullOrWhiteSpace(args.Surname) ||
            string.IsNullOrWhiteSpace(args.Email) ||
            string.IsNullOrWhiteSpace(args.Password) ||
            string.IsNullOrWhiteSpace(args.Role))
        {
            throw new ArgumentException("All arguments are required.");
        }
    }

    private void EnsureUserEmailIsUnique(string email)
    {
        if (UserRepository.ExistsByEmail(email))
        {
            throw new InvalidOperationException("An user with that email already exists.");
        }
    }

    private Role GetRoleByName(string roleName)
    {
        return RoleRepository.Get(roleName) ?? throw new KeyNotFoundException("Role does not exist.");
    }

    private User CreateNewUser(CreateUserArgs args, Role role)
    {
        return new User(args.Name, args.Surname, args.Email, args.Password, role);
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
