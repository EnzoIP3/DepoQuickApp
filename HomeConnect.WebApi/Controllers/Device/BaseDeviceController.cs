using BusinessLogic.Devices.Services;
using HomeConnect.WebApi.Controllers.Device.Models;
using Microsoft.AspNetCore.Mvc;

namespace HomeConnect.WebApi.Controllers.Device;

public class BaseDeviceController(IDeviceService deviceService) : ControllerBase
{
    [HttpPost("{hardwareId}/toggle")]
    public virtual ConnectionResponse Toggle([FromRoute] string hardwareId)
    {
        var connectionState = deviceService.Toogle(hardwareId);
        return new ConnectionResponse { ConnectionState = connectionState, HardwareId = hardwareId };
    }
}
