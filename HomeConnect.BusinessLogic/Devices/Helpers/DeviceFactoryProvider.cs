using BusinessLogic.BusinessOwners.Services;
using BusinessLogic.Devices.Entities;

namespace BusinessLogic.Devices.Helpers;

public class DeviceFactoryProvider(IBusinessOwnerService businessOwnerService)
{
    private IBusinessOwnerService BusinessOwnerService { get; set; } = businessOwnerService;

    public IDeviceFactory GetFactory(DeviceType deviceType)
    {
        return deviceType switch
        {
            DeviceType.Camera => new CameraFactory(BusinessOwnerService),
            _ => new DefaultDeviceFactory(BusinessOwnerService)
        };
    }
}
