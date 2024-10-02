using BusinessLogic.HomeOwners.Entities;

namespace BusinessLogic.HomeOwners.Repositories;

public interface IHomeRepository
{
    void Add(Home home);
    Home Get(Guid homeId);
}
