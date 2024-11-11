namespace HomeConnect.WebApi.Controllers.Homes.Models;

public record GetHomeResponse
{
    public string Id { get; init; } = null!;
    public string? Name { get; init; } = null!;
    public OwnerInfo Owner { get; init; } = null!;
    public string Address { get; init; } = null!;
    public double Latitude { get; init; }
    public double Longitude { get; init; }
    public int MaxMembers { get; init; }
}
