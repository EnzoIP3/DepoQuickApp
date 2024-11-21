namespace HomeConnect.WebApi.Controllers.Businesses.Models;

public sealed record ListBusinessInfo
{
    public string Name { get; init; } = null!;
    public string OwnerName { get; init; } = null!;
    public string OwnerSurname { get; init; } = null!;
    public string OwnerEmail { get; init; } = null!;
    public string Rut { get; init; } = null!;
    public string Logo { get; init; } = null!;
}
