namespace HomeConnect.WebApi.Controllers.Businesses.Models;

public record CreateBusinessRequest
{
    public string Name { get; set; } = null!;
    public string OwnerId { get; set; } = null!;
    public string Rut { get; set; } = null!;
}
