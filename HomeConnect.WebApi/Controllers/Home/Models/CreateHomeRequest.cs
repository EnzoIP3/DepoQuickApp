namespace HomeConnect.WebApi.Controllers.Home.Models;

public record CreateHomeRequest
{
    public string? Address { get; set; } = null!;
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public int? MaxMembers { get; set; }
}
