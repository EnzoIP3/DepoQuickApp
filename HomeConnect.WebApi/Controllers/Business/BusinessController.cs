using BusinessLogic;
using BusinessLogic.Admins.Models;
using BusinessLogic.Admins.Services;
using HomeConnect.WebApi.Filters;
using Microsoft.AspNetCore.Mvc;

namespace HomeConnect.WebApi.Controllers.Business;

[ApiController]
[Route("businesses")]
[AuthenticationFilter]
public class BusinessController(IAdminService adminService) : ControllerBase
{
    [AuthorizationFilter("get-all-businesses")]
    public IActionResult GetBusinesses([FromQuery] int? currentPage = null, [FromQuery] int? pageSize = null,
        [FromQuery] string? nameFilter = null, [FromQuery] string? ownerFilter = null)
    {
        var businesses = adminService.GetBusinesses(currentPage, pageSize, nameFilter, ownerFilter);
        var response = ResponseFromBusinesses(businesses);
        return Ok(response);
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
