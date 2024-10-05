using System.ComponentModel.DataAnnotations;

namespace BusinessLogic.Roles.Entities;

public record SystemPermission
{
    public const string CreateAdministrator = "create-administrator";
    public const string DeleteAdministrator = "delete-administrator";
    public const string CreateBusinessOwner = "create-business-owner";
    public const string GetAllUsers = "get-all-users";
    public const string GetAllBusinesses = "get-all-businesses";
    public const string CreateHome = "create-home";
    public const string AddMember = "add-member";
    public const string AddDevice = "add-device";
    public const string GetDevices = "get-devices";
    public const string GetMembers = "get-members";
    public const string CreateBusiness = "create-business";
    public const string CreateCamera = "create-camera";
    public const string CreateSensor = "create-sensor";

    [Key]
    public string Value { get; init; }

    public string? RoleName { get; set; }

    public SystemPermission(string value, string? roleName = null)
    {
        Value = value;
        RoleName = roleName;
    }

    public override string ToString()
    {
        return Value;
    }
}
