using BusinessLogic;
using BusinessLogic.Admins.Services;
using HomeConnect.WebApi.Filters;
using Microsoft.AspNetCore.Mvc;

namespace HomeConnect.WebApi.Controllers.Business;

[ApiController]
[Route("businesses")]
[AuthorizationFilter]
public class BusinessController : ControllerBase
{
    private readonly IAdminService _adminService;

    public BusinessController(IAdminService adminService)
    {
        _adminService = adminService;
    }

    public IActionResult GetBusinesses([FromQuery] int? currentPage = null, [FromQuery] int? pageSize = null,
        [FromQuery] string? nameFilter = null, [FromQuery] string? ownerFilter = null)
    {
        var businesses = _adminService.GetBusinesses(currentPage, pageSize, nameFilter, ownerFilter);
        var response = ResponseFromBusinesses(businesses);
        return Ok(response);
    }

    private static object ResponseFromBusinesses(PagedData<BusinessLogic.BusinessOwners.Entities.Business> businesses)
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
