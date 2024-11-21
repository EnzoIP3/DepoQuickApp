namespace HomeConnect.WebApi.Controllers.Businesses.Models;

public sealed record CreateBusinessResponse
{
    public string Rut { get; set; } = null!;
}
