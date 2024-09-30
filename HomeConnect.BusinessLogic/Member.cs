namespace BusinessLogic;

public class Member
{
    public Member(User user)
    {
        User = user;
    }

    public Member(User user, List<HomePermission> homePermissions)
    {
        throw new NotImplementedException();
    }

    public Guid Id { get; } = Guid.NewGuid();
    public User User { get; init; }
    public List<HomePermission> HomePermissions { get; } = [];

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
