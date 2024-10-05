using System.ComponentModel.DataAnnotations;

namespace BusinessLogic.Roles.Entities;

public record SystemPermission
{
    public static string CreateAdministrator = "create-administrator";
    public static string DeleteAdministrator = "delete-administrator";
    public static string CreateBusinessOwner = "create-business-owner";
    public static string GetAllUsers = "get-all-users";
    public static string GetAllBusinesses = "get-all-businesses";
    public static string CreateHome = "create-home";
    public static string AddMember = "add-member";
    public static string AddDevice = "add-device";
    public static string GetDevices = "get-devices";
    public static string GetMembers = "get-members";

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
