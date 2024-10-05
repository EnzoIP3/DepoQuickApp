using BusinessLogic.Admins.Services;
using BusinessLogic.BusinessOwners.Entities;
using BusinessLogic.BusinessOwners.Models;
using BusinessLogic.BusinessOwners.Services;
using BusinessLogic.Users.Models;
using HomeConnect.WebApi.Controllers.Businesses;
using HomeConnect.WebApi.Controllers.BusinessOwner.Models;
using HomeConnect.WebApi.Controllers.Camera.Models;
using HomeConnect.WebApi.Filters;
using Microsoft.AspNetCore.Mvc;

namespace HomeConnect.WebApi.Controllers.BusinessOwner;

[ApiController]
[Route("business_owners")]
[AuthorizationFilter]
public class BusinessOwnerController(IAdminService adminService, IBusinessOwnerService businessOwnerService)
    : ControllerBase
{
    public CreateBusinessOwnerResponse CreateBusinessOwner(CreateBusinessOwnerRequest request)
    {
        CreateUserArgs createUserArgs = UserModelFromRequest(request);
        var businessOwnerId = adminService.CreateBusinessOwner(createUserArgs);
        return new CreateBusinessOwnerResponse() { Id = businessOwnerId.ToString() };
    }

    private static CreateUserArgs UserModelFromRequest(CreateBusinessOwnerRequest request)
    {
        var userModel = new CreateUserArgs
        {
            Name = request.Name, Surname = request.Surname, Email = request.Email, Password = request.Password
        };
        return userModel;
    }

    [HttpPost]
    [Route("sensors")]
    public CreateDeviceResponse CreateDevice([FromBody] CreateDeviceRequest request)
    {
        var args = new CreateDeviceArgs()
        {
            BusinessRut = request.BusinessRut,
            Description = request.Description,
            MainPhoto = request.MainPhoto,
            ModelNumber = request.ModelNumber,
            Name = request.Name,
            SecondaryPhotos = request.SecondaryPhotos,
            Type = request.Type
        };

        var createdDevice = businessOwnerService.CreateDevice(args);

        return new CreateDeviceResponse { Id = createdDevice };
    }
}
