namespace BusinessLogic;

public class Member
{
    public Guid Id { get; } = Guid.NewGuid();
    public User User { get; init; }
    public List<HomePermission> HomePermissions { get; } = [];

    public Member(User user)
    {
        User = user;
    }

    public void AddPermission(HomePermission permission)
    {
        EnsurePermissionDoesNotExist(permission);
        HomePermissions.Add(permission);
    }

    private void EnsurePermissionDoesNotExist(HomePermission permission)
    {
        if (HomePermissions.Contains(permission))
        {
            throw new InvalidOperationException("Permission is already added");
        }
    }
}
