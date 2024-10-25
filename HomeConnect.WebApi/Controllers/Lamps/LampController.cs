using BusinessLogic.BusinessOwners.Models;
using BusinessLogic.BusinessOwners.Services;
using BusinessLogic.Devices.Entities;
using BusinessLogic.Devices.Services;
using BusinessLogic.Roles.Entities;
using BusinessLogic.Users.Entities;
using HomeConnect.WebApi.Controllers.Devices.Models;
using HomeConnect.WebApi.Controllers.Lamps.Models;
using HomeConnect.WebApi.Filters;
using Microsoft.AspNetCore.Mvc;

namespace HomeConnect.WebApi.Controllers.Lamps;

public class LampController(
    IBusinessOwnerService businessOwnerService,
    IDeviceService deviceService)
    : ControllerBase
{
    [HttpPost]
    [AuthenticationFilter]
    [AuthorizationFilter(SystemPermission.CreateLamp)]
    public CreateLampResponse CreateLamp([FromBody] CreateLampRequest request)
    {
        var userLoggedIn = HttpContext.Items[Item.UserLogged] as User;
        var args = new CreateDeviceArgs
        {
            Owner = userLoggedIn!,
            Description = request.Description ?? string.Empty,
            MainPhoto = request.MainPhoto ?? string.Empty,
            ModelNumber = request.ModelNumber,
            Name = request.Name ?? string.Empty,
            SecondaryPhotos = request.SecondaryPhotos,
            Type = "Lamp"
        };

        Device createdLamp = businessOwnerService.CreateDevice(args);

        return new CreateLampResponse { Id = createdLamp.Id };
    }

    [HttpPost("{hardwareId}/turnOn")]
    public NotifyResponse TurnOn([FromRoute] string hardwareId)
    {
        deviceService.TurnLamp(hardwareId, true);
        return new NotifyResponse { HardwareId = hardwareId };
    }

    [HttpPost("{hardwareId}/turnOff")]
    public NotifyResponse TurnOff([FromRoute] string hardwareId)
    {
        deviceService.TurnLamp(hardwareId, false);
        return new NotifyResponse { HardwareId = hardwareId };
    }
}
