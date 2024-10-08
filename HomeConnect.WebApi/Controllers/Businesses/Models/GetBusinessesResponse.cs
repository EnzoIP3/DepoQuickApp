namespace HomeConnect.WebApi.Controllers.Businesses.Models;

public record GetBusinessesResponse
{
    public List<ListBusinessInfo> Businesses { get; init; } = null!;
    public Pagination Pagination { get; init; } = null!;
}
