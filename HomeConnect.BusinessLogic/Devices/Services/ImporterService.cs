using BusinessLogic.BusinessOwners.Models;
using BusinessLogic.BusinessOwners.Services;
using BusinessLogic.Devices.Entities;
using BusinessLogic.Devices.Helpers;
using BusinessLogic.Devices.Models;
using BusinessLogic.Helpers;
using BusinessLogic.Users.Entities;
using DeviceImporter;
using DeviceImporter.Models;

namespace BusinessLogic.Devices.Services;

public class ImporterService : IImporterService
{
    public ImporterService(IAssemblyInterfaceLoader<IDeviceImporter> loadAssembly, IBusinessOwnerService businessOwnerService)
    {
        LoadAssembly = loadAssembly;
        BusinessOwnerService = businessOwnerService;
    }

    private IBusinessOwnerService BusinessOwnerService { get; }
    private IAssemblyInterfaceLoader<IDeviceImporter> LoadAssembly { get; }
    private static readonly string ImportersPath = AppDomain.CurrentDomain.BaseDirectory + "Importers";
    private static readonly string ImporterFilesPath = AppDomain.CurrentDomain.BaseDirectory + "ImportFiles";
    public List<ImporterData> GetImporters()
    {
        var importers = LoadAssembly.GetImplementationsList(ImportersPath);
        return importers.Select(importer => new ImporterData
        {
            Name = importer,
            Parameters = LoadAssembly.GetImplementationByName(importer, ImportersPath).GetParams()
        }).ToList();
    }

    public List<string> ImportDevices(ImportDevicesArgs args)
    {
        IDeviceImporter importer = LoadAssembly.GetImplementationByName(args.ImporterName, ImportersPath);
        var deviceArgs = importer.ImportDevices(args.Parameters);
        CreateDevicesFromArgs(deviceArgs, args.User);
        return deviceArgs.Select(deviceArg => deviceArg.Name).ToList();
    }

    private void CreateDevicesFromArgs(List<DeviceArgs> deviceArgs, User user)
    {
        var factoryProvider = new DeviceFactoryProvider(BusinessOwnerService);
        foreach (var deviceArg in deviceArgs)
        {
            var deviceType = Enum.Parse<DeviceType>(deviceArg.Type);
            var factory = factoryProvider.GetFactory(deviceType);
            factory.CreateDevice(user, deviceArg);
        }
    }

    public List<string> GetImportFiles()
    {
        if (!Directory.Exists(ImporterFilesPath))
        {
            Directory.CreateDirectory(ImporterFilesPath);
        }

        return Directory.GetFiles(ImporterFilesPath).Select(Path.GetFileName).ToList();
    }
}
