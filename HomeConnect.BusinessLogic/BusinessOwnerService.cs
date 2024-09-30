namespace BusinessLogic;

public class BusinessOwnerService
{
    public IDeviceRepository DeviceRepository { get; init; }
    public IUserRepository UserRepository { get; init; }
    public IBusinessRepository BusinessRepository { get; init; }
    public IRoleRepository RoleRepository { get; init; }

    public BusinessOwnerService(IUserRepository userRepository, IBusinessRepository businessRepository,
        IRoleRepository roleRepository, IDeviceRepository deviceRepository)
    {
        UserRepository = userRepository;
        BusinessRepository = businessRepository;
        RoleRepository = roleRepository;
        DeviceRepository = deviceRepository;
    }

    public void CreateBusiness(string ownerEmail, string businessRut, string businessName)
    {
        var owner = VerifyOwnerExists(ownerEmail);

        var existingBusiness = BusinessRepository.GetBusinessByOwner(ownerEmail);
        if (existingBusiness != null)
        {
            throw new InvalidOperationException("Owner already has a business");
        }

        var existingBusinessByRut = BusinessRepository.GetBusinessByRut(businessRut);
        if (existingBusinessByRut != null)
        {
            throw new InvalidOperationException("RUT already exists");
        }

        var business = new Business(businessRut, businessName, owner);
        BusinessRepository.Add(business);
    }

    public void CreateDevice(string name, int modelNumber, string description, string mainPhoto, List<string> secondaryPhotos, string type)
    {
        var device = new Device(name, modelNumber, description, mainPhoto, secondaryPhotos, type);
        DeviceRepository.Add(device);
    }

    private User VerifyOwnerExists(string ownerEmail)
    {
        var owner = UserRepository.GetUser(ownerEmail);
        if (owner == null)
        {
            throw new ArgumentException("Owner does not exist");
        }

        return owner;
    }
}
