namespace BusinessLogic;

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

    public Guid Create(UserModel model)
    {
        ValidateAdminModel(model);
        EnsureUserEmailIsUnique(Guid.Parse(model.Id));
        var role = RoleRepository.GetRole(model.Role);
        var admin = new User(model.Name, model.Surname, model.Email, model.Password, role);
        UserRepository.Add(admin);
        return admin.Id;
    }

    public void Delete(Guid id)
    {
        EnsureAdminExists(id);
        UserRepository.Delete(id);
    }

    private void ValidateAdminModel(UserModel model)
    {
        if (string.IsNullOrWhiteSpace(model.Name) ||
            string.IsNullOrWhiteSpace(model.Surname) ||
            string.IsNullOrWhiteSpace(model.Email) ||
            string.IsNullOrWhiteSpace(model.Password) ||
            string.IsNullOrWhiteSpace(model.Role))
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

    public Guid CreateBusinessOwner(UserModel model)
    {
        EnsureUserEmailIsUnique(Guid.Parse(model.Id));
        var role = RoleRepository.GetRole(model.Role);
        var user = new User(model.Name, model.Surname, model.Email, model.Password, role);
        UserRepository.Add(user);
        return user.Id;
    }

    public PagedData<ListUserModel> GetUsers(int? currentPage = null, int? pageSize = null, string? fullNameFilter = null,
        string? roleFilter = null)
    {
        currentPage ??= 1;
        pageSize ??= 10;
        var users = UserRepository.GetUsers((int)currentPage, (int)pageSize, fullNameFilter, roleFilter);
        var data = users.Data.Select(x => new ListUserModel
        {
            Id = x.Id.ToString(),
            Name = x.Name,
            Surname = x.Surname,
            FullName = $"{x.Name} {x.Surname}",
            Role = x.Role.Name,
            CreatedAt = x.CreatedAt
        }).ToList();
        return new PagedData<ListUserModel>
        {
            Data = data,
            Page = users.Page,
            PageSize = users.PageSize,
            TotalPages = users.TotalPages
        };
    }

    public PagedData<ListBusinessModel> GetBusiness(int? currentPage = null, int? pageSize = null,
        string? fullNameFilter = null, string? nameFilter = null)
    {
        currentPage ??= 1;
        pageSize ??= 10;
        var businesses = BusinessRepository.GetBusinesses((int)currentPage, (int)pageSize, fullNameFilter,
            nameFilter);
        var data = businesses.Data.Select(x => new ListBusinessModel
        {
            Rut = x.Rut,
            Name = x.Name,
            OwnerEmail = x.Owner.Email,
            OwnerFullName = $"{x.Owner.Name} {x.Owner.Surname}"
        }).ToList();
        return new PagedData<ListBusinessModel>
        {
            Data = data,
            Page = businesses.Page,
            PageSize = businesses.PageSize,
            TotalPages = businesses.TotalPages
        };
    }
}
