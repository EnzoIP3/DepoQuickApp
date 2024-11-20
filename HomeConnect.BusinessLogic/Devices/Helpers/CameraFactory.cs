using BusinessLogic.BusinessOwners.Models;
using BusinessLogic.BusinessOwners.Services;
using BusinessLogic.Users.Entities;
using DeviceImporter.Models;

namespace BusinessLogic.Devices.Helpers;

public class CameraFactory(IBusinessOwnerService businessOwnerService) : IDeviceFactory
{
    public void CreateDevice(User user, DeviceArgs deviceArg)
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
        businessOwnerService.CreateCamera(cameraArgs);
    }
}
