using System.ComponentModel.DataAnnotations;
using BusinessLogic.Users.Entities;

namespace BusinessLogic.HomeOwners.Entities;

public class Member
{
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

    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    public User User { get; init; } = null!;
    public Guid UserId { get; set; }

    public List<HomePermission> HomePermissions { get; set; } = [];

    public Home Home { get; set; } = null!;
    public Guid HomeId { get; set; }

    public void AddPermission(HomePermission permission)
    {
        EnsurePermissionDoesNotExist(permission);
        HomePermissions.Add(permission);
    }

    private void EnsurePermissionDoesNotExist(HomePermission permission)
    {
        if (HasPermission(permission))
        {
            throw new InvalidOperationException("That permission is already added.");
        }
    }

    private void EnsurePermissionExists(HomePermission permission)
    {
        if (!HasPermission(permission))
        {
            throw new ArgumentException("That permission does not exist");
        }
    }

    public void DeletePermission(HomePermission permission)
    {
        EnsurePermissionExists(permission);
        HomePermissions.RemoveAll(p => p.Value == permission.Value);
    }

    public bool HasPermission(HomePermission permission)
    {
        return HomePermissions.Any(p => p.Value == permission.Value);
    }
}
