using BusinessLogic.Auth.Models;

namespace HomeConnect.WebApi.Controllers.Auth.Models;

public record CreateTokenRequest
{
    public string? Email { get; set; }
    public string? Password { get; set; }

    public CreateTokenArgs ToCreateTokenArgs()
    {
        return new CreateTokenArgs { Email = Email!, Password = Password! };
    }
}
