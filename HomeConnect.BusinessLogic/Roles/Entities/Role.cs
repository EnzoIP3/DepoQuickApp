using System.ComponentModel.DataAnnotations;

namespace BusinessLogic.Roles.Entities;

public class Role
{
    public const string Admin = "Admin";
    public const string HomeOwner = "HomeOwner";
    public const string BusinessOwner = "BusinessOwner";

    public Role()
    {
    }

    public Role(string name, List<SystemPermission> permissions)
    {
        Name = name;
        Permissions = permissions;
    }

    [Key] public string Name { get; init; } = string.Empty;

    public List<SystemPermission> Permissions { get; set; } = [];

    public bool HasPermission(string permission)
    {
        return Permissions.Any(p => p.ToString() == permission);
    }
}
