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
        User user = GetUserByEmail(args.UserEmail);
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

    public List<Home> GetHomesByOwnerId(Guid userId)
    {
        var homes = HomeRepository.GetHomesByUserId(userId);
        return homes;
    }

    public void NameHome(NameHomeArgs args)
    {
        ValidateNameHomeParameters(args.OwnerId, args.HomeId, args.NewName);
        Home home = GetHome(args.HomeId);
        HomeRepository.Rename(home, args.NewName);
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

    public IEnumerable<OwnedDevice> GetHomeDevices(string homeId)
    {
        Home home = GetHome(ValidateAndParseGuid(homeId));
        return OwnedDeviceRepository.GetOwnedDevicesByHome(home);
    }

    public void UpdateMemberNotifications(Guid memberId, bool? requestShouldBeNotified)
    {
        EnsureShouldBeNotifiedIsNotNull(requestShouldBeNotified);
        Member member = GetMemberById(memberId);
        ChangeMemberPermissions(requestShouldBeNotified!.Value, member);
    }

    public Member GetMemberById(Guid memberId)
    {
        if (!MemberRepository.Exists(memberId))
        {
            throw new ArgumentException("Member does not exist.");
        }

        return MemberRepository.Get(memberId);
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
        EnsureNoArgumentsAreEmpty(args);
        EnsureGuidIsValid(args.HomeOwnerId, "Home Owner ID");
        EnsureUserExistsById(args.HomeOwnerId);
    }

    private static void EnsureNoArgumentsAreEmpty(CreateHomeArgs args)
    {
        if (string.IsNullOrWhiteSpace(args.HomeOwnerId) || string.IsNullOrWhiteSpace(args.Address))
        {
            throw new ArgumentException("All arguments are required.");
        }
    }

    private void ValidateAddMemberArgs(AddMemberArgs args)
    {
        EnsureNoArgumentsAreMissing(args);
        EnsureGuidIsValid(args.HomeId, "Home ID");
        EnsureMemberIsNotAlreadyAdded(args);
        EnsureUserExistsByEmail(args.UserEmail);
    }

    private static void EnsureNoArgumentsAreMissing(AddMemberArgs args)
    {
        if (string.IsNullOrWhiteSpace(args.HomeId) || string.IsNullOrWhiteSpace(args.UserEmail))
        {
            throw new ArgumentException("All arguments are required.");
        }
    }

    private void EnsureAddressIsUnique(string address)
    {
        if (HomeRepository.GetByAddress(address) != null)
        {
            throw new InvalidOperationException("Address is already in use.");
        }
    }

    private User GetUserByEmail(string userEmail)
    {
        EnsureUserExistsByEmail(userEmail);
        return UserRepository.GetByEmail(userEmail);
    }

    private User GetUserById(string userId)
    {
        var guid = ValidateAndParseGuid(userId);
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
            throw new KeyNotFoundException("Home does not exist.");
        }
    }

    private void EnsureUserExistsByEmail(string userEmail)
    {
        if (!UserRepository.ExistsByEmail(userEmail))
        {
            throw new ArgumentException("User does not exist.");
        }
    }

    private void EnsureUserExistsById(string userId)
    {
        var guid = ValidateAndParseGuid(userId);
        if (!UserRepository.Exists(guid))
        {
            throw new ArgumentException("User does not exist.");
        }
    }

    private void EnsureMemberIsNotAlreadyAdded(AddMemberArgs args)
    {
        Home home = GetHome(Guid.Parse(args.HomeId));
        if (home.Members.Any(m => m.User.Id.ToString() == args.UserEmail))
        {
            throw new InvalidOperationException("The member is already added to the home.");
        }
    }

    private void ChangeMemberPermissions(bool requestShouldBeNotified, Member member)
    {
        var permission = new HomePermission(HomePermission.GetNotifications);
        var hasPermission = member.HasPermission(permission);
        if (requestShouldBeNotified && !hasPermission)
        {
            member.AddPermission(permission);
            MemberRepository.Update(member);
        }
        else if (!requestShouldBeNotified && hasPermission)
        {
            member.DeletePermission(permission);
            MemberRepository.Update(member);
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

    public List<HomePermission> GetHomePermissions(Guid homeId, Guid userId)
    {
        var home = GetHome(homeId);
        var member = GetMemberFromHome(home, userId);
        EnsureUserBelongsToHome(member, home, userId);
        return member == null ? HomePermission.AllPermissions : GetPermissionsForMember(member);
    }

    private void EnsureUserBelongsToHome(Member? member, Home home, Guid userId)
    {
        if (member == null && home.Owner.Id != userId)
        {
            throw new ArgumentException("You do not belong to this home.");
        }
    }

    private Member? GetMemberFromHome(Home home, Guid userId)
    {
        return home.Members.FirstOrDefault(m => m.User.Id == userId);
    }

    private List<HomePermission> GetPermissionsForMember(Member member)
    {
        return member.HomePermissions;
    }
}
