using BusinessLogic.Devices.Entities;
using BusinessLogic.Devices.Repositories;
using BusinessLogic.HomeOwners.Entities;
using BusinessLogic.HomeOwners.Models;
using BusinessLogic.HomeOwners.Repositories;
using BusinessLogic.Users.Repositories;
using HomeConnect.WebApi.Test.Controllers;

namespace BusinessLogic.HomeOwners.Services;

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

    public void CreateHome(CreateHomeArgs args)
    {
        EnsureCreateHomeModelIsValid(args);
        var user = _userRepository.Get(Guid.Parse(args.HomeOwnerId));
        var home = new Home(user, args.Address, args.Latitude, args.Longitude, args.MaxMembers);
        _homeRepository.Add(home);
    }

    private static void EnsureCreateHomeModelIsValid(CreateHomeArgs args)
    {
        if (string.IsNullOrWhiteSpace(args.HomeOwnerId) || string.IsNullOrWhiteSpace(args.Address))
        {
            throw new ArgumentException("All arguments are required");
        }
    }

    public void AddMemberToHome(AddMemberArgs args)
    {
        EnsureAddMemberModelIsValid(args);
        EnsureGuidIsValid(args.HomeId);
        var user = _userRepository.Get(Guid.Parse(args.HomeOwnerId));
        var home = _homeRepository.Get(Guid.Parse(args.HomeId));
        var permissions = new List<HomePermission>();

        if (args.CanAddDevices)
        {
            permissions.Add(new HomePermission("canAddDevices"));
        }

        if (args.CanListDevices)
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

    private static void EnsureAddMemberModelIsValid(AddMemberArgs args)
    {
        if (string.IsNullOrWhiteSpace(args.HomeId) || string.IsNullOrWhiteSpace(args.HomeOwnerId))
        {
            throw new ArgumentException("All arguments are required");
        }
    }

    public void AddDeviceToHome(AddDevicesArgs addDevicesArgs)
    {
        ValidateAddDeviceModel(addDevicesArgs);
        var home = GetHome(addDevicesArgs.HomeId);
        var devices = GetDevices(addDevicesArgs.DeviceIds);
        AddDevicesToHome(home, devices);
    }

    private static void ValidateAddDeviceModel(AddDevicesArgs addDevicesArgs)
    {
        EnsureGuidIsValid(addDevicesArgs.HomeId);
        EnsureGuidsAreValid(addDevicesArgs.DeviceIds);
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
        EnsureGuidIsValid(homeId);
        var home = GetHome(homeId);
        return _ownedDeviceRepository.GetOwnedDevicesByHome(home);
    }
}
