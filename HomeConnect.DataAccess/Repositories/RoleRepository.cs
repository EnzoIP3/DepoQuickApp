using BusinessLogic.Roles.Entities;
using BusinessLogic.Roles.Repositories;

namespace HomeConnect.DataAccess.Repositories;

public class RoleRepository : IRoleRepository
{
    private readonly Context _context;

    public RoleRepository(Context context)
    {
        _context = context;
    }

    public Role GetRole(string name)
    {
        Role? role = _context.Roles.FirstOrDefault(r => r.Name == name);
        EnsureRoleIsNotNull(role);
        return role!;
    }

    private static void EnsureRoleIsNotNull(Role? role)
    {
        if (role == null)
        {
            throw new ArgumentException("Role does not exist");
        }
    }
}
