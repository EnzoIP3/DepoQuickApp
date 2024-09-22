namespace BusinessLogic;

public class AdminService
{
    private IUserRepository UserRepository { get; init; }

    public AdminService(IUserRepository userRepository)
    {
        UserRepository = userRepository;
    }

    public void Create(UserModel model)
    {
        ValidateAdminModel(model);
        EnsureUserEmailIsUnique(model.Email);

        var admin = new User(model.Name, model.Surname, model.Email, model.Password, model.Role);
        UserRepository.Add(admin);
    }

    public void Delete(string email)
    {
        EnsureAdminExists(email);
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

    private void EnsureAdminExists(string email)
    {
        if (!UserRepository.Exists(email))
        {
            throw new Exception("Admin does not exist.");
        }
    }

    public void CreateBusinessOwner(UserModel model)
    {
        EnsureUserEmailIsUnique(model.Email);

        var user = new User(model.Name, model.Surname, model.Email, model.Password, model.Role);
        UserRepository.Add(user);
    }

    public List<ListUserModel> GetUsers(int? currentPage = null, int? pageSize = null, string? fullNameFilter = null, string? roleFilter = null)
    {
        currentPage ??= 1;
        pageSize ??= 10;
        var users = UserRepository.GetUsers((int)currentPage, (int)pageSize, fullNameFilter, roleFilter);
        return users.Select(x => new ListUserModel
        {
            Name = x.Name,
            Surname = x.Surname,
            FullName = $"{x.Name} {x.Surname}",
            Role = x.Role.ToString(),
            CreatedAt = x.CreatedAt
        }).ToList();
    }
}
