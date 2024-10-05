using BusinessLogic.Admins.Services;
using BusinessLogic.BusinessOwners.Services;
using BusinessLogic.Users.Models;
using HomeConnect.WebApi.Filters;
using Microsoft.AspNetCore.Mvc;

namespace HomeConnect.WebApi.Controllers.BusinessOwner;

[ApiController]
[Route("business_owners")]
[AuthenticationFilter]
public class BusinessOwnerController(IAdminService adminService, IBusinessOwnerService businessOwnerService) : ControllerBase
{
    [HttpPost]
    [AuthorizationFilter("create-business-owner")]
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
            Name = request.Name,
            Surname = request.Surname,
            Email = request.Email,
            Password = request.Password
        };
        return userModel;
    }

    [HttpPost]
    [Route("businesses")]
    public CreateBusinessResponse CreateBusiness([FromBody] CreateBusinessRequest request)
    {
        var business = new BusinessLogic.BusinessOwners.Entities.Business
        {
            Name = request.Name,
            Rut = request.Rut,
            Owner = request.Owner,
        };

        var createdBusiness = businessOwnerService.CreateBusiness(business.Owner.Email, business.Rut, business.Name);

        return new CreateBusinessResponse { Id = createdBusiness };
    }

    [HttpPost]
    [Route("sensors")]
    public CreateDeviceResponse CreateDevice([FromBody] CreateDeviceRequest request)
    {
        var device = new BusinessLogic.Devices.Entities.Device
        {
            Business = request.Business,
            Description = request.Description,
            MainPhoto = request.MainPhoto,
            ModelNumber = request.ModelNumber,
            Name = request.Name,
            SecondaryPhotos = request.SecondaryPhotos,
            Type = request.Type,
            Id = request.Id
        };

        var createdDevice = businessOwnerService.CreateDevice(device.Name, device.ModelNumber, device.Description, device.MainPhoto, device.SecondaryPhotos, device.Type, device.Business);

        return new CreateDeviceResponse { Id = createdDevice };
    }

    [HttpPost]
    [Route("cameras")]
    public CreateCameraResponse CreateCamera([FromBody] CreateCameraRequest request)
    {
        var camera = new BusinessLogic.Devices.Entities.Camera
        {
            Business = request.Business,
            Description = request.Description,
            MainPhoto = request.MainPhoto,
            ModelNumber = request.ModelNumber,
            Name = request.Name,
            SecondaryPhotos = request.SecondaryPhotos,
            MotionDetection = request.MotionDetection,
            PersonDetection = request.PersonDetection,
            IsExterior = request.IsExterior,
            IsInterior = request.IsInterior
        };

        var createdCamera = businessOwnerService.CreateCamera(camera.Name, camera.ModelNumber, camera.Description, camera.MainPhoto, camera.SecondaryPhotos, camera.Business, camera.MotionDetection, camera.PersonDetection, camera.IsExterior, camera.IsInterior);

        return new CreateCameraResponse { Id = createdCamera };
    }
}
