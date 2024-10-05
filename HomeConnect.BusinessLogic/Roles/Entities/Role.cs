using System.ComponentModel.DataAnnotations;

namespace BusinessLogic.Roles.Entities;

public class Role
{
    public static string Admin = "Admin";
    public static string HomeOwner = "HomeOwner";
    public static string BusinessOwner = "BusinessOwner";

    [Key]
    public string Name { get; init; } = string.Empty;
    public List<SystemPermission> Permissions { get; init; } = [];

    public Role()
    {
    }

    public Role(string name, List<SystemPermission> permissions)
    {
        Name = name;
        Permissions = permissions;
    }

    public bool HasPermission(string permission)
    {
        return Permissions.Any(p => p.ToString() == permission);
    }
}
