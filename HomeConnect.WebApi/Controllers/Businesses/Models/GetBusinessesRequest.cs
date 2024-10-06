namespace HomeConnect.WebApi.Controllers.Businesses.Models;

public record GetBusinessesRequest
{
    public int? CurrentPage { get; init; }
    public int? PageSize { get; init; }
    public string? Name { get; init; }
    public string? Owner { get; init; }
}
