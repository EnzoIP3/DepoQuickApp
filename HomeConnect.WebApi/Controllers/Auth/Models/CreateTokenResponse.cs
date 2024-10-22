namespace HomeConnect.WebApi.Controllers.Auth.Models;

public record CreateTokenResponse
{
    public string Token { get; set; } = null!;
    public List<string> Roles { get; set; } = null!;
}
