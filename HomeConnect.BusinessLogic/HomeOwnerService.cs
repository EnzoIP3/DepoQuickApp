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
        EnsureGuidIsValid(model.HomeId);
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

    private static void EnsureGuidIsValid(string homeId)
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
        ValidateAddDeviceModel(addDeviceModel);
        var home = GetHome(addDeviceModel.HomeId);
        var devices = GetDevices(addDeviceModel.DeviceIds);
        AddDevicesToHome(home, devices);
    }

    private static void ValidateAddDeviceModel(AddDeviceModel addDeviceModel)
    {
        EnsureGuidIsValid(addDeviceModel.HomeId);
        EnsureGuidsAreValid(addDeviceModel.DeviceIds);
    }

    private Home GetHome(string homeId)
    {
        return _homeRepository.Get(Guid.Parse(homeId));
    }

    private List<Device> GetDevices(IEnumerable<string> deviceIds)
    {
        return deviceIds.Select(id => _deviceRepository.Get(Guid.Parse(id))).ToList();
    }

    private void AddDevicesToHome(Home home, List<Device> devices)
    {
        devices.ForEach(device => _ownedDeviceRepository.Add(new OwnedDevice(home, device)));
    }

    private static void EnsureGuidsAreValid(IEnumerable<string> deviceIds)
    {
        deviceIds.ToList().ForEach(EnsureGuidIsValid);
    }

    public List<Member> GetHomeMembers(string homeId)
    {
        EnsureGuidIsValid(homeId);
        var home = GetHome(homeId);
        return home.Members;
    }

    public IEnumerable<OwnedDevice> GetHomeDevices(string homeId)
    {
        throw new NotImplementedException();
    }
}
