using BusinessLogic.BusinessOwners.Models;
using BusinessLogic.BusinessOwners.Services;
using BusinessLogic.Devices.Importer;
using BusinessLogic.Users.Entities;

namespace BusinessLogic.Devices.Helpers;

public class DefaultDeviceFactory(IBusinessOwnerService businessOwnerService) : IDeviceFactory
{
    public void CreateDevice(User user, DeviceArgs deviceArg)
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
        businessOwnerService.CreateDevice(newDeviceArgs);
    }
}
