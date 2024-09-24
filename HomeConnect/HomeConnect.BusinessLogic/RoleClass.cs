namespace BusinessLogic;

public class RoleClass
{
    private string Name { get; init; }
    private List<SystemPermission> Permissions { get; init; }

    public RoleClass(string name, List<SystemPermission> permissions)
    {
        Name = name;
        Permissions = permissions;
    }

    public object HasPermission(string permission)
    {
        if (Permissions.Any(p => p.ToString() == permission))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
