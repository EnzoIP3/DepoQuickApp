namespace BusinessLogic.HomeOwners.Entities;

public class HomePermission
{
    public static List<HomePermission> AllPermissions =
    [
        new(GetHome),
        new(AddMember),
        new(AddDevice),
        new(GetDevices),
        new(GetNotifications),
        new(GetMembers),
        new(UpdateNotifications),
        new(NameHome)
    ];

    public const string GetHome = "get-home";
    public const string AddMember = "add-members";
    public const string AddDevice = "add-devices";
    public const string GetDevices = "get-devices";
    public const string GetNotifications = "get-notifications";
    public const string GetMembers = "get-members";
    public const string UpdateNotifications = "update-notifications";
    public const string NameHome = "name-home";

    public HomePermission(string value)
    {
        Value = value;
    }

    public Guid Id { get; set; } = new();
    public string Value { get; init; }
}
