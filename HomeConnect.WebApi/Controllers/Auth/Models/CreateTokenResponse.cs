using BusinessLogic.Users.Entities;

namespace HomeConnect.WebApi.Controllers.Auth.Models;

public record CreateTokenResponse
{
    public string UserId { get; set; } = null!;
    public string Token { get; set; } = null!;
    public List<string> Permissions { get; set; } = null!;

    public static CreateTokenResponse FromUserAndToken(string token, User user)
    {
        return new CreateTokenResponse
        {
            UserId = user.Id.ToString(),
            Token = token,
            Permissions = user.GetPermissions().Select(p => p.Value).ToList()
        };
    }
}
