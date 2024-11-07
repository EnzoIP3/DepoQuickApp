using BusinessLogic.BusinessOwners.Helpers;
using BusinessLogic.BusinessOwners.Models;
using ModeloValidador.Abstracciones;

namespace BusinessLogic.BusinessOwners.Services;

public class ValidatorService : IValidatorService
{
    public ValidatorService(IAssemblyInterfaceLoader<IModeloValidador> loadAssembly)
    {
        LoadAssembly = loadAssembly;
    }

    private IAssemblyInterfaceLoader<IModeloValidador> LoadAssembly { get; }
    private static readonly string Path = AppDomain.CurrentDomain.BaseDirectory + "Validators";

    public List<ValidatorInfo> GetValidators()
    {
        return LoadAssembly.GetImplementationsList(Path)
            .Select(validatorName => new ValidatorInfo { Name = validatorName })
            .ToList();
    }

    public IModeloValidador GetValidatorByName(string validatorName)
    {
        return LoadAssembly.GetImplementation(validatorName, Path);
    }
}
