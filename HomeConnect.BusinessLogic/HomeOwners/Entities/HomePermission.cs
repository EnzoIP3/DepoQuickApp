using System.ComponentModel.DataAnnotations;

namespace BusinessLogic.HomeOwners.Entities;

public class HomePermission
{
    public const string AddMember = "add-members";
    public const string AddDevice = "add-devices";
    public const string GetDevices = "get-devices";
    public const string GetNotifications = "get-notifications";

    [Key]
    public string Value { get; init; }

    public HomePermission(string value)
    {
        Value = value;
    }
}
