namespace BusinessLogic.Auth.Models;

public struct CreateTokenArgs
{
    public string Email { get; set; }
    public string Password { get; set; }
}
