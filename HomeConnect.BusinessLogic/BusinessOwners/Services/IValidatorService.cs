using BusinessLogic.BusinessOwners.Models;

namespace BusinessLogic.BusinessOwners.Services;

public interface IValidatorService
{
    public List<ValidatorInfo> GetValidators();
}
