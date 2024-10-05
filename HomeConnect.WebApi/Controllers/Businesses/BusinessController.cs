using BusinessLogic;
using BusinessLogic.Admins.Models;
using BusinessLogic.Admins.Services;
using BusinessLogic.BusinessOwners.Models;
using BusinessLogic.BusinessOwners.Services;
using HomeConnect.WebApi.Controllers.Businesses.Models;
using HomeConnect.WebApi.Filters;
using Microsoft.AspNetCore.Mvc;

namespace HomeConnect.WebApi.Controllers.Businesses;

[ApiController]
[Route("businesses")]
[AuthorizationFilter]
public class BusinessController(IAdminService adminService, IBusinessOwnerService businessOwnerService) : ControllerBase
{
    [HttpGet]
    public IActionResult GetBusinesses([FromQuery] int? currentPage = null, [FromQuery] int? pageSize = null,
        [FromQuery] string? nameFilter = null, [FromQuery] string? ownerFilter = null)
    {
        var businesses = adminService.GetBusinesses(currentPage, pageSize, nameFilter, ownerFilter);
        var response = ResponseFromBusinesses(businesses);
        return Ok(response);
    }

    [HttpPost]
    public CreateBusinessResponse CreateBusiness([FromBody] CreateBusinessRequest request)
    {
        var args = new CreateBusinessArgs() { Name = request.Name, OwnerId = request.OwnerId, Rut = request.Rut };
        var createdBusiness = businessOwnerService.CreateBusiness(args);
        return new CreateBusinessResponse { Rut = createdBusiness };
    }

    private static object ResponseFromBusinesses(PagedData<GetBusinessesArgs> businesses)
    {
        var response = new
        {
            businesses.Data,
            Pagination = new Pagination
            {
                Page = businesses.Page,
                PageSize = businesses.PageSize,
                TotalPages = businesses.TotalPages
            }
        };
        return response;
    }
}
