namespace BusinessLogic;

public class HomeOwnerService
{
    private readonly IHomeRepository _homeRepository;
    private readonly IUserRepository _userRepository;
    private readonly IDeviceRepository _deviceRepository;
    private readonly IOwnedDeviceRepository _ownedDeviceRepository;

    public HomeOwnerService(IHomeRepository homeRepository, IUserRepository userRepository,
        IDeviceRepository deviceRepository, IOwnedDeviceRepository ownedDeviceRepository)
    {
        _homeRepository = homeRepository;
        _userRepository = userRepository;
        _deviceRepository = deviceRepository;
        _ownedDeviceRepository = ownedDeviceRepository;
    }

    public void CreateHome(CreateHomeModel model)
    {
        EnsureCreateHomeModelIsValid(model);
        var user = _userRepository.Get(model.HomeOwnerEmail);
        var home = new Home(user, model.Address, model.Latitude, model.Longitude, model.MaxMembers);
        _homeRepository.Add(home);
    }

    private static void EnsureCreateHomeModelIsValid(CreateHomeModel model)
    {
        if (string.IsNullOrWhiteSpace(model.HomeOwnerEmail) || string.IsNullOrWhiteSpace(model.Address))
        {
            throw new ArgumentException("All arguments are required");
        }
    }

    public void AddMemberToHome(AddMemberModel model)
    {
        EnsureAddMemberModelIsValid(model);
        EnsureHomeIdIsValidGuid(model.HomeId);
        var user = _userRepository.Get(model.HomeOwnerEmail);
        var home = _homeRepository.Get(Guid.Parse(model.HomeId));
        var permissions = new List<HomePermission>();

        if (model.CanAddDevices)
        {
            permissions.Add(new HomePermission("canAddDevices"));
        }

        if (model.CanListDevices)
        {
            permissions.Add(new HomePermission("canListDevices"));
        }

        var member = new Member(user, permissions);
        home.AddMember(member);
    }

    private static void EnsureHomeIdIsValidGuid(string homeId)
    {
        if (!Guid.TryParse(homeId, out _))
        {
            throw new ArgumentException("HomeId must be a valid guid");
        }
    }

    private static void EnsureAddMemberModelIsValid(AddMemberModel model)
    {
        if (string.IsNullOrWhiteSpace(model.HomeId) || string.IsNullOrWhiteSpace(model.HomeOwnerEmail))
        {
            throw new ArgumentException("All arguments are required");
        }
    }

    public void AddDeviceToHome(AddDeviceModel addDeviceModel)
    {
        EnsureHomeIdIsValidGuid(addDeviceModel.HomeId);
        var home = _homeRepository.Get(Guid.Parse(addDeviceModel.HomeId));
        var devices = addDeviceModel.DeviceIds.Select(id => _deviceRepository.Get(Guid.Parse(id))).ToList();
        devices.ForEach((device) => _ownedDeviceRepository.Add(new OwnedDevice(home, device)));
    }
}
