using BusinessLogic.Devices.Services;
using HomeConnect.WebApi.Controllers.Devices.Models;
using Microsoft.AspNetCore.Mvc;

namespace HomeConnect.WebApi.Controllers.Devices;

public class BaseDeviceController(IDeviceService deviceService) : ControllerBase
{
    [HttpPost("{hardwareId}/toggle")]
    public virtual ConnectionResponse Toggle([FromRoute] string hardwareId)
    {
        var connectionState = deviceService.ToggleDevice(hardwareId);
        return new ConnectionResponse { Connected = connectionState, HardwareId = hardwareId };
    }
}
