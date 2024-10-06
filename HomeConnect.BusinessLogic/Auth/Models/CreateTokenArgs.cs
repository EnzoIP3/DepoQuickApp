namespace BusinessLogic.Auth.Models;

public record CreateTokenArgs
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}
