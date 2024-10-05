using BusinessLogic.BusinessOwners.Entities;
using BusinessLogic.BusinessOwners.Repositories;
using BusinessLogic.Roles.Repositories;
using BusinessLogic.Users.Entities;
using BusinessLogic.Users.Models;
using BusinessLogic.Users.Repositories;

namespace BusinessLogic.Admins.Services;

public class AdminService : IAdminService
{
    private IUserRepository UserRepository { get; init; }
    private IBusinessRepository BusinessRepository { get; init; }
    private IRoleRepository RoleRepository { get; init; }

    public AdminService(IUserRepository userRepository, IBusinessRepository businessRepository,
        IRoleRepository roleRepository)
    {
        UserRepository = userRepository;
        BusinessRepository = businessRepository;
        RoleRepository = roleRepository;
    }

    public Guid Create(CreateUserArgs args)
    {
        ValidateAdminModel(args);
        EnsureUserEmailIsUnique(Guid.Parse(args.Id));
        var role = RoleRepository.GetRole(args.Role);
        var admin = new User(args.Name, args.Surname, args.Email, args.Password, role);
        UserRepository.Add(admin);
        return admin.Id;
    }

    public void Delete(Guid id)
    {
        EnsureAdminExists(id);
        UserRepository.Delete(id);
    }

    private void ValidateAdminModel(CreateUserArgs args)
    {
        if (string.IsNullOrWhiteSpace(args.Name) ||
            string.IsNullOrWhiteSpace(args.Surname) ||
            string.IsNullOrWhiteSpace(args.Email) ||
            string.IsNullOrWhiteSpace(args.Password) ||
            string.IsNullOrWhiteSpace(args.Role))
        {
            throw new ArgumentException("Invalid input data.");
        }
    }

    private void EnsureUserEmailIsUnique(Guid id)
    {
        if (UserRepository.Exists(id))
        {
            throw new Exception("User already exists.");
        }
    }

    private void EnsureAdminExists(Guid id)
    {
        if (!UserRepository.Exists(id))
        {
            throw new Exception("Admin does not exist.");
        }
    }

    public Guid CreateBusinessOwner(CreateUserArgs args)
    {
        EnsureUserEmailIsUnique(Guid.Parse(args.Id));
        var role = RoleRepository.GetRole(args.Role);
        var user = new User(args.Name, args.Surname, args.Email, args.Password, role);
        UserRepository.Add(user);
        return user.Id;
    }

    public PagedData<User> GetUsers(int? currentPage = null, int? pageSize = null, string? fullNameFilter = null,
        string? roleFilter = null)
    {
        currentPage ??= 1;
        pageSize ??= 10;
        var users = UserRepository.GetAllPaged((int)currentPage, (int)pageSize, fullNameFilter, roleFilter);
        return new PagedData<User>
        {
            Data = users.Data,
            Page = users.Page,
            PageSize = users.PageSize,
            TotalPages = users.TotalPages
        };
    }

    public PagedData<Business> GetBusinesses(int? currentPage = null, int? pageSize = null, string? fullNameFilter = null, string? nameFilter = null)
    {
        currentPage ??= 1;
        pageSize ??= 10;
        var businesses = BusinessRepository.GetPagedData((int)currentPage, (int)pageSize, fullNameFilter, nameFilter);
        return new PagedData<Business>
        {
            Data = businesses.Data,
            Page = businesses.Page,
            PageSize = businesses.PageSize,
            TotalPages = businesses.TotalPages
        };
    }
}
