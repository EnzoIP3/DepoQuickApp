using BusinessLogic.Devices.Entities;
using BusinessLogic.Devices.Repositories;
using BusinessLogic.HomeOwners.Entities;
using BusinessLogic.HomeOwners.Models;
using BusinessLogic.HomeOwners.Repositories;
using BusinessLogic.Users.Repositories;

namespace BusinessLogic.HomeOwners.Services;

public class HomeOwnerService : IHomeOwnerService
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

    public Guid CreateHome(CreateHomeArgs args)
    {
        EnsureCreateHomeModelIsValid(args);
        EnsureAddressIsUnique(args.Address);
        EnsureUserExists(args.HomeOwnerId);
        var user = _userRepository.Get(Guid.Parse(args.HomeOwnerId));
        var home = new Home(user, args.Address, args.Latitude, args.Longitude, args.MaxMembers);
        _homeRepository.Add(home);
        return home.Id;
    }

    private void EnsureAddressIsUnique(string address)
    {
        var home = _homeRepository.GetByAddress(address);
        if (home != null)
        {
            throw new ArgumentException("Address is already in use");
        }
    }

    private void EnsureUserExists(string homeOwnerId)
    {
        if (!_userRepository.Exists(Guid.Parse(homeOwnerId)))
        {
            throw new ArgumentException("User does not exist");
        }
    }

    private static void EnsureCreateHomeModelIsValid(CreateHomeArgs args)
    {
        if (string.IsNullOrWhiteSpace(args.HomeOwnerId) || string.IsNullOrWhiteSpace(args.Address))
        {
            throw new ArgumentException("All arguments are required");
        }
    }

    public Guid AddMemberToHome(AddMemberArgs args)
    {
        EnsureAddMemberModelIsValid(args);
        EnsureGuidIsValid(args.HomeId);
        EnsureMemberIsNotAlreadyAdded(args);
        EnsureGuidIsValid(args.HomeOwnerId);
        EnsureUserExists(args.HomeOwnerId);
        var user = _userRepository.Get(Guid.Parse(args.HomeOwnerId));
        var home = GetHome(Guid.Parse(args.HomeId));
        var permissions = new List<HomePermission>();

        if (args.CanAddDevices)
        {
            permissions.Add(new HomePermission(HomePermission.AddDevices));
        }

        if (args.CanListDevices)
        {
            permissions.Add(new HomePermission(HomePermission.GetDevices));
        }

        var member = new Member(user, permissions);
        home.AddMember(member);
        return user.Id;
    }

    private void EnsureMemberIsNotAlreadyAdded(AddMemberArgs args)
    {
        var home = GetHome(Guid.Parse(args.HomeId));
        if (home.Members.Any(m => m.User.Id.ToString() == args.HomeOwnerId))
        {
            throw new ArgumentException("Member is already added to the home");
        }
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
        var home = GetHome(Guid.Parse(addDevicesArgs.HomeId));
        EnsureDevicesAreNotAdded(addDevicesArgs.DeviceIds, home);

        var devices = GetDevices(addDevicesArgs.DeviceIds);
        AddDevicesToHome(home, devices);
    }

    public Home GetHome(Guid homeId)
    {
        EnsureHomeExists(homeId);
        return _homeRepository.Get(homeId);
    }

    private void EnsureHomeExists(Guid homeId)
    {
        if (!_homeRepository.Exists(homeId))
        {
            throw new ArgumentException("Home does not exist");
        }
    }

    private void EnsureDevicesAreNotAdded(IEnumerable<string> argsDeviceIds, Home home)
    {
        var deviceIds = argsDeviceIds.ToList();
        var ownedDevices = _ownedDeviceRepository.GetOwnedDevicesByHome(home);
        var ownedDeviceIds = ownedDevices.Select(od => od.Device.Id.ToString()).ToList();
        var duplicateDevices = deviceIds.Intersect(ownedDeviceIds).ToList();
        if (duplicateDevices.Any())
        {
            throw new ArgumentException(
                $"Devices with ids {string.Join(", ", duplicateDevices)} are already added to the home");
        }
    }

    private static void ValidateAddDeviceModel(AddDevicesArgs addDevicesArgs)
    {
        EnsureGuidIsValid(addDevicesArgs.HomeId);
        EnsureGuidsAreValid(addDevicesArgs.DeviceIds);
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
        var home = GetHome(Guid.Parse(homeId));
        return home.Members;
    }

    public IEnumerable<OwnedDevice> GetHomeDevices(string homeId)
    {
        EnsureGuidIsValid(homeId);
        var home = GetHome(Guid.Parse(homeId));
        return _ownedDeviceRepository.GetOwnedDevicesByHome(home);
    }

    public void UpdateMemberNotifications(Guid memberId, bool requestShouldBeNotified)
    {
        EnsureMemberExists(memberId);
        var member = _homeRepository.GetMemberById(memberId);
        var hasPermission = member.HasPermission(new HomePermission(HomePermission.GetNotifications));
        ChangeMemberPermissions(requestShouldBeNotified, hasPermission, member);
    }

    private void EnsureMemberExists(Guid memberId)
    {
        if (!_homeRepository.ExistsMember(memberId))
        {
            throw new ArgumentException("Member does not exist");
        }
    }

    private void ChangeMemberPermissions(bool requestShouldBeNotified, bool hasPermission, Member member)
    {
        if (requestShouldBeNotified && !hasPermission)
        {
            member.AddPermission(new HomePermission(HomePermission.GetNotifications));
            _homeRepository.UpdateMember(member);
        }
        else if (!requestShouldBeNotified && hasPermission)
        {
            member.DeletePermission(new HomePermission(HomePermission.GetNotifications));
            _homeRepository.UpdateMember(member);
        }
    }
}
