using BusinessLogic.Users.Entities;

namespace HomeConnect.WebApi.Controllers.Auth.Models;

public sealed record CreateTokenResponse
{
    public string UserId { get; set; } = null!;
    public string Token { get; set; } = null!;
    public Dictionary<string, List<string>> Roles { get; set; } = null!;

    public static CreateTokenResponse FromUserAndToken(string token, User user)
    {
        return new CreateTokenResponse
        {
            UserId = user.Id.ToString(),
            Token = token,
            Roles = user.GetRolesAndPermissions()
                .ToDictionary(x => x.Key.Name, x => x.Value.Select(y => y.Value).ToList())
        };
    }
}
