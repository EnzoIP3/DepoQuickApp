using BusinessLogic.BusinessOwners.Helpers;
using BusinessLogic.BusinessOwners.Models;
using BusinessLogic.BusinessOwners.Services;
using BusinessLogic.Devices.Entities;
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
    private static readonly string Path = AppDomain.CurrentDomain.BaseDirectory + "Importers";
    public List<string> GetImporters()
    {
        return LoadAssembly.GetImplementationsList(Path);
    }

    public List<string> ImportDevices(ImportDevicesArgs args)
    {
        IDeviceImporter importer = LoadAssembly.GetImplementation(args.ImporterName, Path);
        var deviceArgs = importer.ImportDevices(args.Route);
        CreateDevices(deviceArgs, args.User);
        return deviceArgs.Select(deviceArg => deviceArg.Name).ToList();
    }

    private void CreateDevices(List<DeviceArgs> deviceArgs, User user)
    {
        foreach (var deviceArg in deviceArgs)
        {
            var deviceType = Enum.Parse<DeviceType>(deviceArg.Type);
            switch (deviceType)
            {
                case DeviceType.Camera:
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
                    break;
                default:
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
                    break;
            }
        }
    }

    public List<string> GetImportFiles()
    {
        throw new NotImplementedException();
    }
}
