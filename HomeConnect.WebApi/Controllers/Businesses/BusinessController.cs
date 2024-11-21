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
public class BusinessController : ControllerBase
{
    private readonly IAdminService _adminService;
    private readonly IBusinessOwnerService _businessOwnerService;

    public BusinessController(IAdminService adminService, IBusinessOwnerService businessOwnerService)
    {
        _adminService = adminService;
        _businessOwnerService = businessOwnerService;
    }

    [HttpGet]
    [AuthorizationFilter(SystemPermission.GetAllBusinesses)]
    public GetBusinessesResponse GetBusinesses([FromQuery] GetBusinessesRequest request)
    {
        PagedData<Business> businesses =
            _adminService.GetBusinesses(request.ToGetBusinessesArgs());
        return GetBusinessesResponse.FromBusinesses(businesses);
    }

    [HttpPost]
    [AuthorizationFilter(SystemPermission.CreateBusiness)]
    public CreateBusinessResponse CreateBusiness([FromBody] CreateBusinessRequest request)
    {
        var userLoggedIn = HttpContext.Items[Item.UserLogged] as User;
        Business business = _businessOwnerService.CreateBusiness(request.ToCreateBusinessArgs(userLoggedIn));
        return new CreateBusinessResponse { Rut = business.Rut };
    }

    [HttpPatch("{businessId}/validator")]
    [AuthorizationFilter(SystemPermission.UpdateBusinessValidator)]
    public UpdateValidatorResponse UpdateValidator(string businessId, [FromBody] UpdateValidatorRequest request)
    {
        var userLoggedIn = HttpContext.Items[Item.UserLogged] as User;
        _businessOwnerService.UpdateValidator(request.ToUpdateValidatorArgs(businessId, userLoggedIn));
        return new UpdateValidatorResponse { BusinessRut = businessId, Validator = request.Validator! };
    }

    [HttpGet("{businessId}/devices")]
    [AuthorizationFilter(SystemPermission.GetBusinessDevices)]
    public GetBusinessDevicesResponse GetDevices(string businessId, [FromQuery] GetBusinessDevicesRequest request)
    {
        var userLoggedIn = HttpContext.Items[Item.UserLogged] as User;
        PagedData<Device> devices =
            _businessOwnerService.GetDevices(request.ToGetBusinessDevicesArgs(businessId, userLoggedIn));
        return GetBusinessDevicesResponse.FromDevices(devices);
    }
}
