using BusinessLogic.Roles.Entities;

namespace BusinessLogic.Roles.Repositories;

public interface IRoleRepository
{
    Role Get(string name);
    bool Exists(string name);
}
