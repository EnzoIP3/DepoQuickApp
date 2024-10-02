namespace BusinessLogic.Roles.Repositories;

public interface IRoleRepository
{
    Entities.Role GetRole(string name);
}
