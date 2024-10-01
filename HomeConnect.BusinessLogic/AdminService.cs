namespace BusinessLogic;

public class AdminService
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

    public void Create(UserModel model)
    {
        ValidateAdminModel(model);
        EnsureUserEmailIsUnique(model.Email);
        var role = RoleRepository.GetRole(model.Role);
        var admin = new User(model.Name, model.Surname, model.Email, model.Password, role);
        UserRepository.Add(admin);
    }

    public void Delete(string email)
    {
        EnsureUserExists(email);
        UserRepository.Delete(email);
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

    private void EnsureUserEmailIsUnique(string email)
    {
        if (UserRepository.Exists(email))
        {
            throw new Exception("User already exists.");
        }
    }

    private void EnsureUserExists(string email)
    {
        if (!UserRepository.Exists(email))
        {
            throw new Exception("Admin does not exist.");
        }
    }

    public void CreateBusinessOwner(UserModel model)
    {
        EnsureUserEmailIsUnique(model.Email);
        var role = RoleRepository.GetRole(model.Role);
        var user = new User(model.Name, model.Surname, model.Email, model.Password, role);
        UserRepository.Add(user);
    }

    public List<ListUserModel> GetUsers(int? currentPage = null, int? pageSize = null, string? fullNameFilter = null,
        string? roleFilter = null)
    {
        currentPage ??= 1;
        pageSize ??= 10;
        var users = UserRepository.GetUsers((int)currentPage, (int)pageSize, fullNameFilter, roleFilter);
        return users.Select(x => new ListUserModel
        {
            Name = x.Name,
            Surname = x.Surname,
            FullName = $"{x.Name} {x.Surname}",
            Role = x.Role.Name,
            CreatedAt = x.CreatedAt
        }).ToList();
    }

    public List<ListBusinessModel> GetBusiness(int? currentPage = null, int? pageSize = null,
        string? fullNameFilter = null, string? nameFilter = null)
    {
        currentPage ??= 1;
        pageSize ??= 10;
        var businesses = BusinessRepository.GetBusinesses((int)currentPage, (int)pageSize, fullNameFilter,
            nameFilter);
        return businesses.Select(x => new ListBusinessModel
        {
            Rut = x.Rut,
            Name = x.Name,
            OwnerEmail = x.Owner.Email,
            OwnerFullName = $"{x.Owner.Name} {x.Owner.Surname}"
        }).ToList();
    }
}
