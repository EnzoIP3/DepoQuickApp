namespace HomeConnect.WebApi.Controllers.Auth.Models;

public record CreateTokenResponse
{
    public string Token { get; set; } = null!;
    public List<string> Permissions { get; set; } = null!;
}
