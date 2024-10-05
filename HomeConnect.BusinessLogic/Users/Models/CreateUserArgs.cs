namespace BusinessLogic.Users.Models;

public struct CreateUserArgs
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Role { get; set; }
    public string? ProfilePicture { get; set; }
}
