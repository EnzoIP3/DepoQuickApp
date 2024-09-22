namespace BusinessLogic;

public class AdminService
{
    private IAdminRepository AdminRepository { get; init; }
    private IBusinessOwnerRepository BusinessOwnerRepository { get; init; }

    public AdminService(IAdminRepository adminRepository, IBusinessOwnerRepository businessOwnerRepository)
    {
        AdminRepository = adminRepository;
        BusinessOwnerRepository = businessOwnerRepository;
    }

    public void Create(UserModel model)
    {
        ValidateAdminModel(model);
        EnsureAdminEmailIsUnique(model.Email);

        var admin = new User(model.Name, model.Surname, model.Email, model.Password, model.Role);
        AdminRepository.Add(admin);
    }

    public void Delete(string email)
    {
        EnsureAdminExists(email);
        AdminRepository.Delete(email);
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

    private void EnsureAdminEmailIsUnique(string email)
    {
        if (AdminRepository.Exists(email))
        {
            throw new Exception("User already exists.");
        }
    }

    private void EnsureAdminExists(string email)
    {
        if (!AdminRepository.Exists(email))
        {
            throw new Exception("Admin does not exist.");
        }
    }

    public void CreateBusinessOwner(UserModel model)
    {
        if (BusinessOwnerRepository.Exists(model.Email))
        {
            throw new Exception("User already exists.");
        }

        var user = new User(model.Name, model.Surname, model.Email, model.Password, model.Role);
        BusinessOwnerRepository.Add(user);
    }
}
