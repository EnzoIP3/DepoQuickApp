using BusinessLogic.BusinessOwners.Models;
using BusinessLogic.Users.Entities;

namespace HomeConnect.WebApi.Controllers.Businesses.Models;

public record GetBusinessDevicesRequest
{
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 10;

    public GetBusinessDevicesArgs ToGetBusinessDevicesArgs(string businessId, User? user)
    {
        return new GetBusinessDevicesArgs { Rut = businessId, User = user!, CurrentPage = Page, PageSize = PageSize };
    }
}
