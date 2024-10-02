using System.ComponentModel.DataAnnotations;
using BusinessLogic.Users.Entities;

namespace BusinessLogic.HomeOwners.Entities;

public class Member
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    public Member()
    {
    }

    public Member(User user)
    {
        User = user;
    }

    public Member(User user, List<HomePermission> homePermissions)
    {
        User = user;
        HomePermissions = homePermissions;
    }

    public User User { get; init; } = null!;
    public List<HomePermission> HomePermissions { get; set; } = [];

    public void AddPermission(HomePermission permission)
    {
        EnsurePermissionDoesNotExist(permission);
        HomePermissions.Add(permission);
    }

    private void EnsurePermissionDoesNotExist(HomePermission permission)
    {
        if (HasPermission(permission))
        {
            throw new InvalidOperationException("Permission is already added");
        }
    }

    private void EnsurePermissionExists(HomePermission permission)
    {
        if (!HasPermission(permission))
        {
            throw new InvalidOperationException("Permission does not exist");
        }
    }

    public void DeletePermission(HomePermission permission)
    {
        EnsurePermissionExists(permission);
        HomePermissions.Remove(permission);
    }

    public bool HasPermission(HomePermission permission)
    {
        return HomePermissions.Contains(permission);
    }
}
