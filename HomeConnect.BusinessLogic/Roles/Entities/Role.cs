using System.ComponentModel.DataAnnotations;

namespace BusinessLogic.Roles.Entities;

public class Role
{
    public const string Admin = "Admin";
    public const string HomeOwner = "HomeOwner";
    public const string BusinessOwner = "BusinessOwner";

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
