using System.ComponentModel.DataAnnotations;

namespace BusinessLogic.Roles.Entities;

public class Role
{
    public static Role Admin = new Role("Admin",
    [
        new SystemPermission("create-administrator"), new SystemPermission("delete-administrator"),
        new SystemPermission("create-business-owner"), new SystemPermission("get-all-users"),
        new SystemPermission("get-all-businesses")
    ]);

    public static Role HomeOwner = new Role("HomeOwner", [
        new SystemPermission("create-home"), new SystemPermission("add-member"), new SystemPermission("add-device"),
        new SystemPermission("get-devices"), new SystemPermission("get-members")
    ]);

    public static Role BusinessOwner = new Role("BusinessOwner", []);

    [Key] public string Name { get; init; } = string.Empty;
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
