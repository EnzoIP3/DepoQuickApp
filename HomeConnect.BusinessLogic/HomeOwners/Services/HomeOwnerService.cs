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
    private readonly IDeviceRepository _deviceRepository;

    private readonly IHomeRepository _homeRepository;
    private readonly IMemberRepository _memberRepository;
    private readonly IOwnedDeviceRepository _ownedDeviceRepository;
    private readonly IRoomRepository _roomRepository;
    private readonly IUserRepository _userRepository;

    public HomeOwnerService(
        IHomeRepository homeRepository,
        IUserRepository userRepository,
        IDeviceRepository deviceRepository,
        IOwnedDeviceRepository ownedDeviceRepository,
        IMemberRepository memberRepository,
        IRoomRepository roomRepository)
    {
        _homeRepository = homeRepository;
        _userRepository = userRepository;
        _deviceRepository = deviceRepository;
        _ownedDeviceRepository = ownedDeviceRepository;
        _memberRepository = memberRepository;
        _roomRepository = roomRepository;
    }

    public Guid CreateHome(CreateHomeArgs args)
    {
        ValidateCreateHomeArgs(args);
        EnsureAddressIsUnique(args.Address);
        User user = GetUserById(args.HomeOwnerId);
        var home = new Home(user, args.Address, args.Latitude, args.Longitude, args.MaxMembers);
        _homeRepository.Add(home);
        return home.Id;
    }

    public Guid AddMemberToHome(AddMemberArgs args)
    {
        ValidateAddMemberArgs(args);
        User user = GetUserByEmail(args.UserEmail);
        Home home = GetHome(Guid.Parse(args.HomeId));
        Member member = CreateMember(user, args);
        home.AddMember(member);
        member.AddPermission(new HomePermission(HomePermission.GetHome));
        _memberRepository.Add(member);
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
        return _homeRepository.Get(homeId);
    }

    public List<Home> GetHomesByOwnerId(Guid userId)
    {
        List<Home> homes = _homeRepository.GetHomesByUserId(userId);
        return homes;
    }

    public void NameHome(NameHomeArgs args)
    {
        ValidateNameHomeParameters(args.OwnerId, args.HomeId, args.NewName);
        Home home = GetHome(args.HomeId);
        _homeRepository.Rename(home, args.NewName);
    }

    public void NameDevice(NameDeviceArgs args)
    {
        ValidateNameDeviceArgs(args);
        OwnedDevice device = _ownedDeviceRepository.GetByHardwareId(Guid.Parse(args.HardwareId));
        EnsureDeviceIsNotNull(device);
        _ownedDeviceRepository.Rename(device, args.NewName);
    }

    public OwnedDevice GetOwnedDeviceByHardwareId(string hardwareId)
    {
        Guid guid = ValidateAndParseGuid(hardwareId);
        EnsureOwnedDeviceExists(guid);
        return _ownedDeviceRepository.GetByHardwareId(guid);
    }

    public Room CreateRoom(string homeId, string name)
    {
        ValidateRoomParameters(homeId, name);
        if (!_homeRepository.Exists(Guid.Parse(homeId)))
        {
            throw new KeyNotFoundException("Home does not exist.");
        }

        Home home = _homeRepository.Get(Guid.Parse(homeId));
        var room = new Room(name, home);
        _roomRepository.Add(room);
        return room;
    }

    public Room GetRoom(string roomId)
    {
        Guid guid = ValidateAndParseGuid(roomId);
        EnsureRoomExists(guid);
        return _roomRepository.Get(guid);
    }

    public Guid AddOwnedDeviceToRoom(string roomId, string ownedDeviceId)
    {
        var roomGuid = Guid.Parse(roomId);
        var ownedDeviceGuid = Guid.Parse(ownedDeviceId);

        if (!_roomRepository.Exists(roomGuid))
        {
            throw new ArgumentException("Invalid room ID.");
        }

        Room room = _roomRepository.Get(roomGuid);

        if (!_ownedDeviceRepository.Exists(ownedDeviceGuid))
        {
            throw new ArgumentException("That device does not exist.");
        }

        OwnedDevice ownedDevice = _ownedDeviceRepository.GetOwnedDeviceById(ownedDeviceGuid);

        room.AddOwnedDevice(ownedDevice);
        _roomRepository.Update(room);
        return ownedDevice.HardwareId;
    }

    public List<Room> GetRoomsByHomeId(string homesId)
    {
        List<Room> rooms = _roomRepository.GetRoomsByHomeId(Guid.Parse(homesId));
        return rooms;
    }

    public List<Member> GetHomeMembers(string homeId)
    {
        Home home = GetHome(ValidateAndParseGuid(homeId));
        return home.Members;
    }

    public IEnumerable<OwnedDevice> GetHomeDevices(string homeId, string? roomId = null)
    {
        Home home = GetHome(ValidateAndParseGuid(homeId));
        IEnumerable<OwnedDevice> devicesQuery = _ownedDeviceRepository.GetOwnedDevicesByHome(home);

        if (!string.IsNullOrEmpty(roomId))
        {
            devicesQuery = devicesQuery.Where(od => od.Room != null && od.Room.Id.ToString() == roomId);
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
        if (!_memberRepository.Exists(memberId))
        {
            throw new ArgumentException("Member does not exist.");
        }

        return _memberRepository.Get(memberId);
    }

    public List<HomePermission> GetHomePermissions(Guid homeId, Guid userId)
    {
        Home home = GetHome(homeId);
        Member? member = GetMemberFromHome(home, userId);
        EnsureUserBelongsToHome(member, home, userId);
        return member == null ? HomePermission.AllPermissions : GetPermissionsForMember(member);
    }

    private void ValidateNameDeviceArgs(NameDeviceArgs args)
    {
        EnsureOwnerIdIsNotEmpty(args);
        EnsureHardwareIdIsNotEmpty(args);
        EnsureHardwareIdIsAGuid(args);
        EnsureNewNameIsNotEmpty(args);
    }

    private static void EnsureNewNameIsNotEmpty(NameDeviceArgs args)
    {
        if (string.IsNullOrEmpty(args.NewName))
        {
            throw new ArgumentException("New name cannot be empty");
        }
    }

    private static void EnsureHardwareIdIsAGuid(NameDeviceArgs args)
    {
        if (!Guid.TryParse(args.HardwareId, out _))
        {
            throw new ArgumentException("Hardware ID must be a valid GUID");
        }
    }

    private static void EnsureHardwareIdIsNotEmpty(NameDeviceArgs args)
    {
        if (args.HardwareId == string.Empty)
        {
            throw new ArgumentException("Hardware ID cannot be empty");
        }
    }

    private static void EnsureOwnerIdIsNotEmpty(NameDeviceArgs args)
    {
        if (args.OwnerId == Guid.Empty)
        {
            throw new ArgumentException("Owner ID cannot be empty");
        }
    }

    private static void EnsureDeviceIsNotNull(OwnedDevice device)
    {
        if (device == null)
        {
            throw new ArgumentException("Device does not exist");
        }
    }

    private void EnsureOwnedDeviceExists(Guid hardwareId)
    {
        if (!_ownedDeviceRepository.Exists(hardwareId))
        {
            throw new KeyNotFoundException("Device does not exist in this home.");
        }
    }

    private void EnsureRoomExists(Guid guid)
    {
        if (!_roomRepository.Exists(guid))
        {
            throw new KeyNotFoundException("Room does not exist.");
        }
    }

    private void ValidateRoomParameters(string homeId, string name)
    {
        if (string.IsNullOrWhiteSpace(homeId))
        {
            throw new ArgumentException("Home ID cannot be null or empty.");
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Room name cannot be null or empty.");
        }
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
        if (_homeRepository.GetByAddress(address) != null)
        {
            throw new InvalidOperationException("Address is already in use.");
        }
    }

    private User GetUserByEmail(string userEmail)
    {
        EnsureUserExistsByEmail(userEmail);
        return _userRepository.GetByEmail(userEmail);
    }

    private User GetUserById(string userId)
    {
        Guid guid = ValidateAndParseGuid(userId);
        return _userRepository.Get(guid);
    }

    private Member CreateMember(User user, AddMemberArgs args)
    {
        var member = new Member(user);
        AddPermissionsToMember(member, args);
        return member;
    }

    private static void AddPermissionsToMember(Member member, AddMemberArgs args)
    {
        args.Permissions.ForEach(permission =>
        {
            member.AddPermission(new HomePermission(permission));
        });
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
        return deviceIds.Select(id => _deviceRepository.Get(Guid.Parse(id))).ToList();
    }

    private void AddDevicesToHome(Home home, List<Device> devices)
    {
        devices.ForEach(device =>
        {
            _ownedDeviceRepository.Add(OwnedDeviceFactory.CreateOwnedDevice(home, device));
        });
    }

    private void EnsureHomeExists(Guid homeId)
    {
        if (!_homeRepository.Exists(homeId))
        {
            throw new KeyNotFoundException("Home does not exist.");
        }
    }

    private void EnsureUserExistsByEmail(string userEmail)
    {
        if (!_userRepository.ExistsByEmail(userEmail))
        {
            throw new ArgumentException("User does not exist.");
        }
    }

    private void EnsureUserExistsById(string userId)
    {
        Guid guid = ValidateAndParseGuid(userId);
        if (!_userRepository.Exists(guid))
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
            _memberRepository.Update(member);
        }
        else if (!requestShouldBeNotified && hasPermission)
        {
            member.DeletePermission(permission);
            _memberRepository.Update(member);
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
