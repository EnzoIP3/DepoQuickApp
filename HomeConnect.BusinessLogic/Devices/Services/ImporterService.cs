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
    private static readonly string ImportersPath = AppDomain.CurrentDomain.BaseDirectory + "Importers";
    private static readonly string ImporterFilesPath = AppDomain.CurrentDomain.BaseDirectory + "ImportFiles";

    private readonly IBusinessOwnerService _businessOwnerService;
    private readonly IAssemblyInterfaceLoader<IDeviceImporter> _loadAssembly;

    public ImporterService(IAssemblyInterfaceLoader<IDeviceImporter> loadAssembly,
        IBusinessOwnerService businessOwnerService)
    {
        _loadAssembly = loadAssembly;
        _businessOwnerService = businessOwnerService;
    }

    public List<ImporterData> GetImporters()
    {
        List<string> importers = _loadAssembly.GetImplementationsList(ImportersPath);
        return importers.Select(importer => new ImporterData
        {
            Name = importer,
            Parameters = _loadAssembly.GetImplementationByName(importer, ImportersPath).GetParams()
        }).ToList();
    }

    public List<string> ImportDevices(ImportDevicesArgs args)
    {
        IDeviceImporter importer = _loadAssembly.GetImplementationByName(args.ImporterName, ImportersPath);
        List<DeviceArgs> deviceArgs = importer.ImportDevices(args.Parameters);
        CreateDevicesFromArgs(deviceArgs, args.User);
        return deviceArgs.Select(deviceArg => deviceArg.Name).ToList();
    }

    public List<string> GetImportFiles()
    {
        if (!Directory.Exists(ImporterFilesPath))
        {
            Directory.CreateDirectory(ImporterFilesPath);
        }

        return Directory.GetFiles(ImporterFilesPath).Select(Path.GetFileName).ToList();
    }

    private void CreateDevicesFromArgs(List<DeviceArgs> deviceArgs, User user)
    {
        var factoryProvider = new DeviceFactoryProvider(_businessOwnerService);
        foreach (DeviceArgs deviceArg in deviceArgs)
        {
            DeviceType deviceType = Enum.Parse<DeviceType>(deviceArg.Type);
            IDeviceFactory factory = factoryProvider.GetFactory(deviceType);
            factory.CreateDevice(user, deviceArg);
        }
    }
}
