using BusinessLogic.BusinessOwners.Helpers;
using BusinessLogic.BusinessOwners.Models;

namespace BusinessLogic.Devices.Services;

public class ImporterService : IImporterService
{
    public ImporterService(IAssemblyInterfaceLoader<IDeviceImporter> loadAssembly)
    {
        LoadAssembly = loadAssembly;
    }

    private IAssemblyInterfaceLoader<IDeviceImporter> LoadAssembly { get; }
    private static readonly string Path = AppDomain.CurrentDomain.BaseDirectory + "Importers";
    public List<string> GetImporters()
    {
        return LoadAssembly.GetImplementationsList(Path);
    }

    public List<string> ImportDevices(ImportDevicesArgs args)
    {
        throw new NotImplementedException();
    }

    public List<string> GetImportFiles()
    {
        throw new NotImplementedException();
    }
}
