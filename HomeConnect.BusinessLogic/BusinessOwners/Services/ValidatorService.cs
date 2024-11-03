using BusinessLogic.BusinessOwners.Helpers;
using BusinessLogic.BusinessOwners.Models;
using ModeloValidador.Abstracciones;

namespace BusinessLogic.BusinessOwners.Services;

public class ValidatorService : IValidatorService
{
    private static readonly string Path = AppDomain.CurrentDomain.BaseDirectory + "Validators";
    private readonly AssemblyInterfaceLoader<IModeloValidador> _loadAssembly = new(Path);

    public List<ValidatorInfo> GetValidators()
    {
        return _loadAssembly.GetImplementations()
            .Select(validatorName => new ValidatorInfo { Name = validatorName })
            .ToList();
    }

    public IModeloValidador GetValidatorByName(string validatorName)
    {
        return _loadAssembly.GetImplementation(validatorName);
    }
}
