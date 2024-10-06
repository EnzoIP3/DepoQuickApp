using BusinessLogic;
using BusinessLogic.Admins.Services;
using BusinessLogic.BusinessOwners.Models;
using BusinessLogic.BusinessOwners.Services;
using BusinessLogic.Roles.Entities;
using HomeConnect.WebApi.Controllers.Businesses.Models;
using HomeConnect.WebApi.Filters;
using Microsoft.AspNetCore.Mvc;

namespace HomeConnect.WebApi.Controllers.Business;

[ApiController]
[Route("businesses")]
[AuthenticationFilter]
public class BusinessController(IAdminService adminService, IBusinessOwnerService businessOwnerService) : ControllerBase
{
    [HttpGet]
    [AuthorizationFilter(SystemPermission.GetAllBusinesses)]
    public GetBusinessesResponse GetBusinesses([FromQuery] GetBusinessesRequest request)
    {
        var businesses = adminService.GetBusinesses(request.CurrentPage, request.PageSize, request.Name, request.Owner);
        var response = ResponseFromBusinesses(businesses);
        return response;
    }

    [HttpPost]
    [AuthorizationFilter(SystemPermission.CreateBusiness)]
    public CreateBusinessResponse CreateBusiness([FromBody] CreateBusinessRequest request)
    {
        var args = new CreateBusinessArgs() { Name = request.Name, OwnerId = request.OwnerId, Rut = request.Rut };
        var createdBusiness = businessOwnerService.CreateBusiness(args);
        return new CreateBusinessResponse { Rut = createdBusiness };
    }

    private static GetBusinessesResponse ResponseFromBusinesses(
        PagedData<BusinessLogic.BusinessOwners.Entities.Business> businesses)
    {
        return new GetBusinessesResponse
        {
            Businesses = businesses.Data.Select(b => new ListBusinessInfo()
            {
                Name = b.Name,
                OwnerEmail = b.Owner.Email,
                OwnerName = b.Owner.Name,
                OwnerSurname = b.Owner.Surname,
                Rut = b.Rut
            }).ToList(),
            Pagination = new Pagination()
            {
                Page = businesses.Page, PageSize = businesses.PageSize, TotalPages = businesses.TotalPages
            }
        };
    }
}
