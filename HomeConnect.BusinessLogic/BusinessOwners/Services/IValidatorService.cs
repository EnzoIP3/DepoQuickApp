using BusinessLogic.BusinessOwners.Models;
using ModeloValidador.Abstracciones;

namespace BusinessLogic.BusinessOwners.Services;

public interface IValidatorService
{
    public List<ValidatorInfo> GetValidators();
    public IModeloValidador GetValidatorByName(string validatorName);
    bool Exists(string argsValidator);
    Guid? GetValidatorIdByName(string validatorName);
    public IModeloValidador GetValidator(Guid? validatorId);
}
