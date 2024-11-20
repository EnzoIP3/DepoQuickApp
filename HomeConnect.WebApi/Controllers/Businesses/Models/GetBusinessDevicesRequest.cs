namespace HomeConnect.WebApi.Controllers.Businesses.Models;

public record GetBusinessDevicesRequest
{
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}
