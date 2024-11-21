using BusinessLogic.BusinessOwners.Models;
using BusinessLogic.Helpers;
using ModeloValidador.Abstracciones;

namespace BusinessLogic.BusinessOwners.Services;

public class ValidatorService : IValidatorService
{
    public ValidatorService(IAssemblyInterfaceLoader<IModeloValidador> loadAssembly)
    {
        _loadAssembly = loadAssembly;
    }

    private readonly IAssemblyInterfaceLoader<IModeloValidador> _loadAssembly;
    private static readonly string Path = AppDomain.CurrentDomain.BaseDirectory + "Validators";

    public List<ValidatorInfo> GetValidators()
    {
        return _loadAssembly.GetImplementationsList(Path)
            .Select(validatorName => new ValidatorInfo { Name = validatorName })
            .ToList();
    }

    public IModeloValidador GetValidatorByName(string validatorName)
    {
        return _loadAssembly.GetImplementationByName(validatorName, Path);
    }

    public bool Exists(string argsValidator)
    {
        return _loadAssembly.GetImplementationsList(Path).Contains(argsValidator);
    }

    public Guid? GetValidatorIdByName(string validatorName)
    {
        return _loadAssembly.GetImplementationIdByName(validatorName, Path);
    }

    public IModeloValidador GetValidator(Guid? validatorId)
    {
        return _loadAssembly.GetImplementationById(validatorId, Path);
    }
}
