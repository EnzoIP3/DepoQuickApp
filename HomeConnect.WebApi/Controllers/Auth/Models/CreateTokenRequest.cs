namespace HomeConnect.WebApi.Controllers.Auth.Models;

public struct CreateTokenRequest
{
    public string? Email { get; set; }
    public string? Password { get; set; }
}
