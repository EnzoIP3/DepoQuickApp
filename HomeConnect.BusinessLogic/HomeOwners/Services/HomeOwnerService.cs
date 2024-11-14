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
        User user = GetUserById(args.UserId);
        Home home = GetHome(Guid.Parse(args.HomeId));
        Member member = CreateMember(user, args);
        home.AddMember(member);
        MemberRepository.Add(member);
        return member.Id;
    }

    public void AddDeviceToHome(AddDevicesArgs addDevicesArgs)
    {
        EnsureDevicesAreNotEmpty(addDevicesArgs);
        ValidateAddDeviceModel(addDevicesArgs);
        Home home = GetHome(Guid.Parse(addDevicesArgs.HomeId));
        List<Device> devices = GetDevices(addDevicesArgs.DeviceIds);
        AddDevicesToHome(home, devices);
    }

    public Home GetHome(Guid homeId)
    {
        EnsureHomeExists(homeId);
        return HomeRepository.Get(homeId);
    }

    public List<Home> GetHomesByOwnerId(Guid ownerId)
    {
        User user = UserRepository.Get(ownerId);
        List<Member> members = MemberRepository.GetMembersByUserId(ownerId);
        var homes = members.Select(m => m.Home).ToList();
        return homes;
    }

    public void NameHome(Guid ownerId, Guid homeId, string newName)
    {
        ValidateNameHomeParameters(ownerId, homeId, newName);

        Home home = GetHome(homeId);
        HomeRepository.Rename(home, newName);
    }

    public Room CreateRoom(string homeId, string name)
    {
        throw new NotImplementedException();
    }

    public Guid AddOwnedDeviceToRoom(string roomId, string requestDeviceId)
    {
        throw new NotImplementedException();
    }

    private void ValidateNameHomeParameters(Guid ownerId, Guid homeId, string newName)
    {
        if (ownerId == Guid.Empty)
        {
            throw new ArgumentException("Owner ID cannot be empty");
        }

        if (homeId == Guid.Empty)
        {
            throw new ArgumentException("Home ID cannot be empty");
        }

        if (string.IsNullOrEmpty(newName))
        {
            throw new ArgumentException("New name cannot be null or empty");
        }
    }

    public List<Member> GetHomeMembers(string homeId)
    {
        Home home = GetHome(ValidateAndParseGuid(homeId));
        return home.Members;
    }

   public IEnumerable<OwnedDevice> GetHomeDevices(string homeId, string? roomId = null)
    {
        var home = GetHome(ValidateAndParseGuid(homeId));
        var devicesQuery = OwnedDeviceRepository.GetOwnedDevicesByHome(home);

        if (!string.IsNullOrEmpty(roomId))
        {
            devicesQuery = devicesQuery.Where(od => od.Room.Id.ToString() == roomId);
        }

        return devicesQuery.ToList();
    }

    public void UpdateMemberNotifications(Guid memberId, bool? requestShouldBeNotified)
    {
        EnsureShouldBeNotifiedIsNotNull(requestShouldBeNotified);
        Member member = GetMemberById(memberId);
        ChangeMemberPermissions(requestShouldBeNotified!.Value, member);
    }

    public Member GetMemberById(Guid memberId)
    {
        if (!HomeRepository.ExistsMember(memberId))
        {
            throw new ArgumentException("Member does not exist.");
        }

        return HomeRepository.GetMemberById(memberId);
    }

    private static void EnsureDevicesAreNotEmpty(AddDevicesArgs addDevicesArgs)
    {
        if (!addDevicesArgs.DeviceIds.Any())
        {
            throw new ArgumentException("At least one device must be added to the home.");
        }
    }

    private static void EnsureShouldBeNotifiedIsNotNull(bool? requestShouldBeNotified)
    {
        if (requestShouldBeNotified == null)
        {
            throw new ArgumentException("ShouldBeNotified must be provided.");
        }
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
        if (string.IsNullOrWhiteSpace(args.HomeId) || string.IsNullOrWhiteSpace(args.UserId))
        {
            throw new ArgumentException("All arguments are required.");
        }

        EnsureGuidIsValid(args.HomeId, "Home ID");
        EnsureGuidIsValid(args.UserId, "Member ID");
        EnsureMemberIsNotAlreadyAdded(args);
        EnsureUserExists(args.UserId);
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
        Guid guid = ValidateAndParseGuid(userId);
        EnsureUserExists(userId);
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

    private List<Device> GetDevices(IEnumerable<string> deviceIds)
    {
        return deviceIds.Select(id => DeviceRepository.Get(Guid.Parse(id))).ToList();
    }

    private void AddDevicesToHome(Home home, List<Device> devices)
    {
        devices.ForEach(device =>
        {
            OwnedDeviceRepository.Add(OwnedDeviceFactory.CreateOwnedDevice(home, device));
        });
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
        if (home.Members.Any(m => m.User.Id.ToString() == args.UserId))
        {
            throw new InvalidOperationException("The member is already added to the home.");
        }
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
