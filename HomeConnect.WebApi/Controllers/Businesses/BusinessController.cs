using BusinessLogic;
using BusinessLogic.Admins.Services;
using BusinessLogic.BusinessOwners.Entities;
using BusinessLogic.BusinessOwners.Models;
using BusinessLogic.BusinessOwners.Services;
using BusinessLogic.Roles.Entities;
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
        var userLoggedIn = HttpContext.Items[Item.UserLogged] as BusinessLogic.Users.Entities.User;
        var args = new CreateBusinessArgs
        {
            Name = request.Name ?? string.Empty,
            Logo = request.Logo ?? string.Empty,
            OwnerId = userLoggedIn?.Id.ToString() ?? string.Empty,
            Rut = request.Rut ?? string.Empty
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
                Page = businesses.Page, PageSize = businesses.PageSize, TotalPages = businesses.TotalPages
            }
        };
    }
}
