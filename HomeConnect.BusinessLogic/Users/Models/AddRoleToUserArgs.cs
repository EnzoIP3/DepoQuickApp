namespace BusinessLogic.Users.Models;

public record AddRoleToUserArgs
{
    public string UserId { get; set; } = null!;
    public string Role { get; set; } = null!;
}
