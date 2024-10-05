using BusinessLogic.Admins.Models;
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
        EnsureUserEmailIsUnique(args.Email);
        var role = RoleRepository.Get(args.Role);
        var admin = new User(args.Name, args.Surname, args.Email, args.Password, role);
        UserRepository.Add(admin);
        return admin.Id;
    }

    public void Delete(Guid id)
    {
        EnsureAdminExists(id);
        UserRepository.Delete(id);
    }

    private static void ValidateAdminModel(CreateUserArgs args)
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

    private void EnsureUserEmailIsUnique(string email)
    {
        if (UserRepository.Exists(email))
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
        EnsureUserEmailIsUnique(args.Email);
        var role = RoleRepository.Get(args.Role);
        var user = new User(args.Name, args.Surname, args.Email, args.Password, role);
        UserRepository.Add(user);
        return user.Id;
    }

    public PagedData<GetUsersArgs> GetUsers(int? currentPage = null, int? pageSize = null,
        string? fullNameFilter = null,
        string? roleFilter = null)
    {
        currentPage ??= 1;
        pageSize ??= 10;
        var users = UserRepository.GetAllPaged((int)currentPage, (int)pageSize, fullNameFilter, roleFilter);
        var data = users.Data.Select(x => new GetUsersArgs
        {
            Id = x.Id.ToString(),
            Name = x.Name,
            Surname = x.Surname,
            FullName = $"{x.Name} {x.Surname}",
            Role = x.Role.Name,
            CreatedAt = x.CreatedAt
        }).ToList();
        return new PagedData<GetUsersArgs>
        {
            Data = data, Page = users.Page, PageSize = users.PageSize, TotalPages = users.TotalPages
        };
    }

    public PagedData<GetBusinessesArgs> GetBusinesses(int? currentPage = null, int? pageSize = null,
        string? fullNameFilter = null, string? nameFilter = null)
    {
        currentPage ??= 1;
        pageSize ??= 10;
        var businesses = BusinessRepository.GetPagedData((int)currentPage, (int)pageSize, fullNameFilter,
            nameFilter);
        var data = businesses.Data.Select(x => new GetBusinessesArgs
        {
            Rut = x.Rut,
            Name = x.Name,
            OwnerEmail = x.Owner.Email,
            OwnerFullName = $"{x.Owner.Name} {x.Owner.Surname}"
        }).ToList();
        return new PagedData<GetBusinessesArgs>
        {
            Data = data, Page = businesses.Page, PageSize = businesses.PageSize, TotalPages = businesses.TotalPages
        };
    }
}
