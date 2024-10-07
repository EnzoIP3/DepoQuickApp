using BusinessLogic.Devices.Entities;
using BusinessLogic.Devices.Repositories;
using BusinessLogic.HomeOwners.Entities;
using BusinessLogic.HomeOwners.Models;
using BusinessLogic.HomeOwners.Repositories;
using BusinessLogic.Users.Entities;
using BusinessLogic.Users.Repositories;

namespace BusinessLogic.HomeOwners.Services;

public class HomeOwnerService : IHomeOwnerService
{
    public HomeOwnerService(
        IHomeRepository homeRepository,
        IUserRepository userRepository,
        IDeviceRepository deviceRepository,
        IOwnedDeviceRepository ownedDeviceRepository,
        IMemberRepository memberRepository)
    {
        HomeRepository = homeRepository;
        UserRepository = userRepository;
        DeviceRepository = deviceRepository;
        OwnedDeviceRepository = ownedDeviceRepository;
        MemberRepository = memberRepository;
    }

    private IHomeRepository HomeRepository { get; }
    private IUserRepository UserRepository { get; }
    private IDeviceRepository DeviceRepository { get; }
    private IOwnedDeviceRepository OwnedDeviceRepository { get; }
    private IMemberRepository MemberRepository { get; }

    public Guid CreateHome(CreateHomeArgs args)
    {
        ValidateCreateHomeArgs(args);
        EnsureAddressIsUnique(args.Address);
        User user = GetUserById(args.HomeOwnerId);
        var home = new Home(user, args.Address, args.Latitude, args.Longitude, args.MaxMembers);
        HomeRepository.Add(home);
        return home.Id;
    }

    public Guid AddMemberToHome(AddMemberArgs args)
    {
        ValidateAddMemberArgs(args);
        User user = GetUserById(args.MemberId);
        Home home = GetHome(Guid.Parse(args.HomeId));
        var member = CreateMember(user, args);
        home.AddMember(member);
        MemberRepository.Add(member);
        return user.Id;
    }

    public void AddDeviceToHome(AddDevicesArgs addDevicesArgs)
    {
        if (!addDevicesArgs.DeviceIds.Any())
        {
            throw new ArgumentException("At least one device must be added to the home.");
        }

        ValidateAddDeviceModel(addDevicesArgs);
        Home home = GetHome(Guid.Parse(addDevicesArgs.HomeId));
        EnsureDevicesAreNotAdded(addDevicesArgs.DeviceIds, home);
        List<Device> devices = GetDevices(addDevicesArgs.DeviceIds);
        AddDevicesToHome(home, devices);
    }

    public Home GetHome(Guid homeId)
    {
        EnsureHomeExists(homeId);
        return HomeRepository.Get(homeId);
    }

    public List<Member> GetHomeMembers(string homeId)
    {
        var home = GetHome(ValidateAndParseGuid(homeId));
        return home.Members;
    }

    public IEnumerable<OwnedDevice> GetHomeDevices(string homeId)
    {
        var home = GetHome(ValidateAndParseGuid(homeId));
        return OwnedDeviceRepository.GetOwnedDevicesByHome(home);
    }

    public void UpdateMemberNotifications(Guid memberId, bool requestShouldBeNotified)
    {
        Member member = GetMemberById(memberId);
        ChangeMemberPermissions(requestShouldBeNotified, member);
    }

    private void ValidateCreateHomeArgs(CreateHomeArgs args)
    {
        if (string.IsNullOrWhiteSpace(args.HomeOwnerId) || string.IsNullOrWhiteSpace(args.Address))
        {
            throw new ArgumentException("All arguments are required.");
        }

        EnsureUserExists(args.HomeOwnerId);
    }

    private void ValidateAddMemberArgs(AddMemberArgs args)
    {
        if (string.IsNullOrWhiteSpace(args.HomeId) || string.IsNullOrWhiteSpace(args.MemberId))
        {
            throw new ArgumentException("All arguments are required.");
        }

        EnsureGuidIsValid(args.HomeId, "Home ID");
        EnsureGuidIsValid(args.MemberId, "Member ID");
        EnsureMemberIsNotAlreadyAdded(args);
        EnsureUserExists(args.MemberId);
    }

    private void EnsureAddressIsUnique(string address)
    {
        if (HomeRepository.GetByAddress(address) != null)
        {
            throw new InvalidOperationException("Address is already in use.");
        }
    }

    private User GetUserById(string userId)
    {
        var guid = ValidateAndParseGuid(userId);
        if (!UserRepository.Exists(guid))
        {
            throw new ArgumentException("User does not exist.");
        }

        return UserRepository.Get(guid);
    }

    private Member CreateMember(User user, AddMemberArgs args)
    {
        var permissions = new List<HomePermission>();
        if (args.CanAddDevices)
        {
            permissions.Add(new HomePermission(HomePermission.AddDevice));
        }

        if (args.CanListDevices)
        {
            permissions.Add(new HomePermission(HomePermission.GetDevices));
        }

        return new Member(user, permissions);
    }

    private void ValidateAddDeviceModel(AddDevicesArgs addDevicesArgs)
    {
        EnsureGuidIsValid(addDevicesArgs.HomeId, "Home ID");
        foreach (var id in addDevicesArgs.DeviceIds)
        {
            EnsureGuidIsValid(id, "Device ID");
        }
    }

    private void EnsureDevicesAreNotAdded(IEnumerable<string> deviceIds, Home home)
    {
        var existingDeviceIds = OwnedDeviceRepository
            .GetOwnedDevicesByHome(home)
            .Select(od => od.Device.Id.ToString());

        var duplicateDevices = deviceIds.Intersect(existingDeviceIds).ToList();
        if (duplicateDevices.Any())
        {
            throw new InvalidOperationException(
                $"Devices with ids {string.Join(", ", duplicateDevices)} are already added to the home");
        }
    }

    private List<Device> GetDevices(IEnumerable<string> deviceIds)
    {
        return deviceIds.Select(id => DeviceRepository.Get(Guid.Parse(id))).ToList();
    }

    private void AddDevicesToHome(Home home, List<Device> devices)
    {
        devices.ForEach(device => OwnedDeviceRepository.Add(new OwnedDevice(home, device)));
    }

    private void EnsureHomeExists(Guid homeId)
    {
        if (!HomeRepository.Exists(homeId))
        {
            throw new ArgumentException("Home does not exist.");
        }
    }

    private void EnsureUserExists(string userId)
    {
        if (!UserRepository.Exists(Guid.Parse(userId)))
        {
            throw new ArgumentException("User does not exist.");
        }
    }

    private void EnsureMemberIsNotAlreadyAdded(AddMemberArgs args)
    {
        Home home = GetHome(Guid.Parse(args.HomeId));
        if (home.Members.Any(m => m.User.Id.ToString() == args.MemberId))
        {
            throw new InvalidOperationException("The member is already added to the home.");
        }
    }

    private Member GetMemberById(Guid memberId)
    {
        if (!HomeRepository.ExistsMember(memberId))
        {
            throw new ArgumentException("Member does not exist.");
        }

        return HomeRepository.GetMemberById(memberId);
    }

    private void ChangeMemberPermissions(bool requestShouldBeNotified, Member member)
    {
        var hasPermission = member.HasPermission(new HomePermission(HomePermission.GetNotifications));
        if (requestShouldBeNotified && !hasPermission)
        {
            member.AddPermission(new HomePermission(HomePermission.GetNotifications));
            HomeRepository.UpdateMember(member);
        }
        else if (!requestShouldBeNotified && hasPermission)
        {
            member.DeletePermission(new HomePermission(HomePermission.GetNotifications));
            HomeRepository.UpdateMember(member);
        }
    }

    private void EnsureGuidIsValid(string id, string name)
    {
        if (!Guid.TryParse(id, out _))
        {
            throw new ArgumentException($"{name} must be a valid GUID.");
        }
    }

    private Guid ValidateAndParseGuid(string id)
    {
        if (!Guid.TryParse(id, out Guid parsedGuid))
        {
            throw new ArgumentException("ID must be a valid GUID.");
        }

        return parsedGuid;
    }
}
