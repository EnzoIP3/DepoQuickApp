using BusinessLogic.BusinessOwners.Models;
using ModeloValidador.Abstracciones;

namespace BusinessLogic.BusinessOwners.Services;

public interface IValidatorService
{
    public List<ValidatorInfo> GetValidators();
    public IModeloValidador GetValidatorByName(string validatorName);
}
