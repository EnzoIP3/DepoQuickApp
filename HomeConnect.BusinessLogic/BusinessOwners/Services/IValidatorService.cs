using BusinessLogic.BusinessOwners.Models;
using BusinessLogic.BusinessOwners.Validator;

namespace BusinessLogic.BusinessOwners.Services;

public interface IValidatorService
{
    public List<ValidatorInfo> GetValidators();
    public IModeloValidador GetValidatorByName(string validatorName);
}
