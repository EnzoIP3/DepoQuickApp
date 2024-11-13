using BusinessLogic.BusinessOwners.Models;
using BusinessLogic.BusinessOwners.Services;
using BusinessLogic.Devices.Entities;
using BusinessLogic.Devices.Importer;
using BusinessLogic.Helpers;
using BusinessLogic.Users.Entities;

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
    public List<string> GetImporters()
    {
        return LoadAssembly.GetImplementationsList(ImportersPath);
    }

    public List<string> ImportDevices(ImportDevicesArgs args)
    {
        IDeviceImporter importer = LoadAssembly.GetImplementationByName(args.ImporterName, ImportersPath);
        var deviceArgs = importer.ImportDevices(Path.Combine(ImporterFilesPath, args.FileName));
        CreateDevicesFromArgs(deviceArgs, args.User);
        return deviceArgs.Select(deviceArg => deviceArg.Name).ToList();
    }

    private void CreateDevicesFromArgs(List<DeviceArgs> deviceArgs, User user)
    {
        foreach (var deviceArg in deviceArgs)
        {
            var deviceType = Enum.Parse<DeviceType>(deviceArg.Type);
            switch (deviceType)
            {
                case DeviceType.Camera:
                    CreateCamera(user, deviceArg);
                    break;
                default:
                    CreateDevice(user, deviceArg);
                    break;
            }
        }
    }

    private void CreateCamera(User user, DeviceArgs deviceArg)
    {
        var cameraArgs = new CreateCameraArgs
        {
            Owner = user,
            ModelNumber = deviceArg.ModelNumber,
            Name = deviceArg.Name,
            Description = deviceArg.Description,
            MainPhoto = deviceArg.MainPhoto,
            SecondaryPhotos = deviceArg.SecondaryPhotos,
            MotionDetection = deviceArg.MotionDetection,
            PersonDetection = deviceArg.PersonDetection,
            Exterior = deviceArg.IsExterior,
            Interior = deviceArg.IsInterior
        };
        BusinessOwnerService.CreateCamera(cameraArgs);
    }

    private void CreateDevice(User user, DeviceArgs deviceArg)
    {
        var newDeviceArgs = new CreateDeviceArgs
        {
            Owner = user,
            ModelNumber = deviceArg.ModelNumber,
            Name = deviceArg.Name,
            Description = deviceArg.Description,
            MainPhoto = deviceArg.MainPhoto,
            SecondaryPhotos = deviceArg.SecondaryPhotos,
            Type = deviceArg.Type
        };
        BusinessOwnerService.CreateDevice(newDeviceArgs);
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
