namespace BusinessLogic;

public class AdminService
{
    private IAdminRepository AdminRepository { get; init; }
    public AdminService(IAdminRepository adminRepository)
    {
        AdminRepository = adminRepository;
    }

    public void Create(AdminModel model)
    {
        if (AdminRepository.Exists(model.Username))
        {
            throw new Exception("Username already exists.");
        }

        var admin = new Admin(model.Username, model.Surname, model.Email, model.Password);

        AdminRepository.Add(admin);
    }

    public void Delete(string args)
    {
        if (!AdminRepository.Exists(args))
        {
            throw new Exception("Admin does not exist.");
        }
    }
}
