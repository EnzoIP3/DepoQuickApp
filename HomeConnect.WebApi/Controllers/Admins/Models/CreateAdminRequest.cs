namespace HomeConnect.WebApi.Controllers.Admin.Models;

public record CreateAdminRequest
{
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
}
