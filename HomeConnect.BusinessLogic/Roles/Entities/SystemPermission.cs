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
    public const string UpdateMember = "update-member";
    public const string GetMembers = "get-members";
    public const string CreateBusiness = "create-business";
    public const string CreateCamera = "create-camera";
    public const string CreateSensor = "create-sensor";

    public SystemPermission()
    {
    }

    public SystemPermission(string value, List<Role>? roles = null)
    {
        Value = value;
        Roles = roles;
    }

    [Key]
    public string Value { get; init; } = null!;

    public List<Role>? Roles { get; set; }

    public override string ToString()
    {
        return Value;
    }
}
