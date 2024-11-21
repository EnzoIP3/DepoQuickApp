using BusinessLogic.BusinessOwners.Entities;

namespace BusinessLogic.BusinessOwners.Repositories;

public interface IBusinessRepository
{
    Business Get(string rut);
    bool Exists(string rut);
    Business GetByOwnerId(Guid ownerId);
    bool ExistsByOwnerId(Guid ownerId);
    void Add(Business business);
    PagedData<Business> GetPaged(FilterArgs args);
    void UpdateValidator(string argsBusinessRut, Guid? validatorId);
}
