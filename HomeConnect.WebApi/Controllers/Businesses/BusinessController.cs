using BusinessLogic;
using BusinessLogic.Admins.Services;
using BusinessLogic.BusinessOwners.Entities;
using BusinessLogic.BusinessOwners.Models;
using BusinessLogic.BusinessOwners.Services;
using BusinessLogic.Devices.Entities;
using BusinessLogic.Roles.Entities;
using BusinessLogic.Users.Entities;
using HomeConnect.WebApi.Controllers.Businesses.Models;
using HomeConnect.WebApi.Filters;
using Microsoft.AspNetCore.Mvc;

namespace HomeConnect.WebApi.Controllers.Businesses;

[ApiController]
[Route("businesses")]
[AuthenticationFilter]
public class BusinessController(IAdminService adminService, IBusinessOwnerService businessOwnerService) : ControllerBase
{
    [HttpGet]
    [AuthorizationFilter(SystemPermission.GetAllBusinesses)]
    public GetBusinessesResponse GetBusinesses([FromQuery] GetBusinessesRequest request)
    {
        PagedData<Business> businesses =
            adminService.GetBusinesses(request.CurrentPage, request.PageSize, request.Name, request.OwnerName);
        GetBusinessesResponse response = ResponseFromBusinesses(businesses);
        return response;
    }

    [HttpPost]
    [AuthorizationFilter(SystemPermission.CreateBusiness)]
    public CreateBusinessResponse CreateBusiness([FromBody] CreateBusinessRequest request)
    {
        var userLoggedIn = HttpContext.Items[Item.UserLogged] as User;
        var args = new CreateBusinessArgs
        {
            Name = request.Name ?? string.Empty,
            Logo = request.Logo ?? string.Empty,
            OwnerId = userLoggedIn?.Id.ToString() ?? string.Empty,
            Rut = request.Rut ?? string.Empty,
            Validator = request.Validator ?? string.Empty
        };
        Business business = businessOwnerService.CreateBusiness(args);
        return new CreateBusinessResponse { Rut = business.Rut };
    }

    private static GetBusinessesResponse ResponseFromBusinesses(
        PagedData<Business> businesses)
    {
        return new GetBusinessesResponse
        {
            Businesses = businesses.Data.Select(b => new ListBusinessInfo
            {
                Name = b.Name,
                OwnerEmail = b.Owner.Email,
                OwnerName = b.Owner.Name,
                OwnerSurname = b.Owner.Surname,
                Rut = b.Rut
            }).ToList(),
            Pagination = new Pagination
            {
                Page = businesses.Page,
                PageSize = businesses.PageSize,
                TotalPages = businesses.TotalPages
            }
        };
    }

    [HttpPatch("{businessId}/validator")]
    [AuthorizationFilter(SystemPermission.UpdateBusinessValidator)]
    public UpdateValidatorResponse UpdateValidator(string businessId, [FromBody] UpdateValidatorRequest request)
    {
        var userLoggedIn = HttpContext.Items[Item.UserLogged] as User;
        var args = new UpdateValidatorArgs
        {
            BusinessRut = businessId,
            Validator = request.Validator ?? string.Empty,
            OwnerId = userLoggedIn?.Id.ToString() ?? string.Empty
        };
        businessOwnerService.UpdateValidator(args);
        return new UpdateValidatorResponse { BusinessRut = args.BusinessRut, Validator = args.Validator };
    }

    [HttpGet("{businessId}/devices")]
    [AuthorizationFilter(SystemPermission.GetBusinessDevices)]
    public GetDevicesResponse GetDevices(string businessId, [FromQuery] GetBusinessDevicesRequest request)
    {
        var userLoggedIn = HttpContext.Items[Item.UserLogged] as User;
        var args = CreateGetBusinessDevicesArgs(businessId, request, userLoggedIn);
        PagedData<Device> devices = businessOwnerService.GetDevices(args);
        GetDevicesResponse response = ResponseFromDevices(devices);
        return response;
    }

    private static GetBusinessDevicesArgs CreateGetBusinessDevicesArgs(string businessId, GetBusinessDevicesRequest request,
        User? userLoggedIn)
    {
        return new GetBusinessDevicesArgs
        {
            Rut = businessId,
            User = userLoggedIn!,
            CurrentPage = request.Page,
            PageSize = request.PageSize
        };
    }

    private GetDevicesResponse ResponseFromDevices(PagedData<Device> devices)
    {
        return new GetDevicesResponse
        {
            Devices = devices.Data.Select(d => new DeviceInfo
            {
                Id = d.Id.ToString(),
                Name = d.Name,
                ModelNumber = d.ModelNumber,
                Description = d.Description,
                MainPhoto = d.MainPhoto,
                SecondaryPhotos = d.SecondaryPhotos,
                Type = d.Type.ToString(),
                BusinessName = d.Business.Name
            }).ToList(),
            Pagination = new Pagination
            {
                Page = devices.Page,
                PageSize = devices.PageSize,
                TotalPages = devices.TotalPages
            }
        };
    }
}
