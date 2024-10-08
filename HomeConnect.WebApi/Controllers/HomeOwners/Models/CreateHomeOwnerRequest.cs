namespace HomeConnect.WebApi.Controllers.HomeOwners.Models;

public record CreateHomeOwnerRequest
{
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public string? ProfilePicture { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
}
