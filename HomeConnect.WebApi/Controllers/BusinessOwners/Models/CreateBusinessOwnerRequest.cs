namespace HomeConnect.WebApi.Controllers.BusinessOwners.Models;

public record CreateBusinessOwnerRequest
{
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
}
