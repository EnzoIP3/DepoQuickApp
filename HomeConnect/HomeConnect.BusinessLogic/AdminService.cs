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

    public void Create(AdminModel model)
    {
        ValidateAdminModel(model);
        EnsureAdminEmailIsUnique(model.Email);

        var admin = new Admin(model.Name, model.Surname, model.Email, model.Password);
        AdminRepository.Add(admin);
    }

    public void Delete(string email)
    {
        EnsureAdminExists(email);
        AdminRepository.Delete(email);
    }

    private void ValidateAdminModel(AdminModel model)
    {
        if (string.IsNullOrWhiteSpace(model.Name) ||
            string.IsNullOrWhiteSpace(model.Surname) ||
            string.IsNullOrWhiteSpace(model.Email) ||
            string.IsNullOrWhiteSpace(model.Password))
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

    public void CreateBusinessOwner(BusinessOwnerModel args)
    {
        if (BusinessOwnerRepository.Exists(args.Email))
        {
            throw new Exception("User already exists.");
        }
    }
}
