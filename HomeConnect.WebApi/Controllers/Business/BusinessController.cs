using BusinessLogic;
using HomeConnect.WebApi.Filters;
using HomeConnect.WebApi.Test.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace HomeConnect.WebApi.Controllers;

[ApiController]
[Route("businesses")]
[AuthorizationFilter]
public class BusinessController(IAdminService adminService) : ControllerBase
{
    public IActionResult GetBusinesses([FromQuery] int? currentPage = null, [FromQuery] int? pageSize = null,
        [FromQuery] string? nameFilter = null, [FromQuery] string? ownerFilter = null)
    {
        var businesses = adminService.GetBusiness(currentPage, pageSize, nameFilter, ownerFilter);
        var response = ResponseFromBusinesses(businesses);
        return Ok(response);
    }

    private static object ResponseFromBusinesses(PagedData<ListBusinessModel> businesses)
    {
        var response = new
        {
            businesses.Data,
            Pagination = new Pagination
            {
                Page = businesses.Page, PageSize = businesses.PageSize, TotalPages = businesses.TotalPages
            }
        };
        return response;
    }
}
