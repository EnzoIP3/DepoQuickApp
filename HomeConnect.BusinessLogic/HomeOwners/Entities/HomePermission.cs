using System.ComponentModel.DataAnnotations;

namespace BusinessLogic.HomeOwners.Entities;

public class HomePermission
{
    public const string AddMember = "add-members";
    public const string AddDevice = "add-devices";
    public const string GetDevices = "get-devices";
    public const string GetNotifications = "get-notifications";

    public HomePermission(string value)
    {
        Value = value;
    }

    public Guid Id { get; set; } = new();
    public string Value { get; init; }
}
