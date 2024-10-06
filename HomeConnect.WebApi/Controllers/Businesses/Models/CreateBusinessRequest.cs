namespace HomeConnect.WebApi.Controllers.Businesses.Models;

public record CreateBusinessRequest
{
    public string? Name { get; set; }
    public string? Logo { get; set; }
    public string? Rut { get; set; }
}
