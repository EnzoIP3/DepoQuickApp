using BusinessLogic.BusinessOwners.Models;
using BusinessLogic.BusinessOwners.Services;
using BusinessLogic.Devices.Entities;
using BusinessLogic.Roles.Entities;
using BusinessLogic.Users.Entities;
using HomeConnect.WebApi.Controllers.Lamps.Models;
using HomeConnect.WebApi.Controllers.MotionSensors;
using HomeConnect.WebApi.Filters;
using Microsoft.AspNetCore.Mvc;

namespace HomeConnect.WebApi.Controllers.Lamps;

public class LampController(
    IBusinessOwnerService businessOwnerService)
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
}
