namespace BusinessLogic.HomeOwners.Entities;

public class HomePermission
{
    public const string GetHome = "get-home";
    public const string AddMember = "add-members";
    public const string AddDevice = "add-devices";
    public const string GetDevices = "get-devices";
    public const string GetNotifications = "get-notifications";
    public const string GetMembers = "get-members";
    public const string UpdateNotifications = "update-notifications";
    public const string MoveDevice = "move-device";
    public const string NameDevice = "name-device";
    public const string NameHome = "name-home";
    public const string CreateRoom = "create-room";
    public const string AddDeviceToRoom = "add-device-to-room";

    public static readonly List<HomePermission> AllPermissions =
    [
        new(GetHome),
        new(AddMember),
        new(AddDevice),
        new(GetDevices),
        new(GetNotifications),
        new(GetMembers),
        new(UpdateNotifications),
        new(NameHome),
        new(NameDevice),
        new(MoveDevice),
        new(CreateRoom),
        new(AddDeviceToRoom)
    ];

    public HomePermission(string value)
    {
        Value = value;
    }

    public Guid Id { get; set; } = new();
    public string Value { get; init; }
}
