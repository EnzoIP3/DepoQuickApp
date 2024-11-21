namespace HomeConnect.WebApi.Controllers.Auth.Models;

public record CreateTokenResponse
{
    public string UserId { get; set; } = null!;
    public string Token { get; set; } = null!;
    public Dictionary<string, List<string>> Roles { get; set; } = null!;
}
