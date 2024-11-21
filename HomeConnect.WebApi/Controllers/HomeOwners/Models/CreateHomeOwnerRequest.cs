using BusinessLogic.Roles.Entities;
using BusinessLogic.Users.Models;

namespace HomeConnect.WebApi.Controllers.HomeOwners.Models;

public sealed record CreateHomeOwnerRequest
{
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public string? ProfilePicture { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }

    public CreateUserArgs ToArgs()
    {
        return new CreateUserArgs
        {
            Name = Name ?? string.Empty,
            Surname = Surname ?? string.Empty,
            Email = Email ?? string.Empty,
            Password = Password ?? string.Empty,
            Role = Role.HomeOwner,
            ProfilePicture = ProfilePicture ?? string.Empty
        };
    }
}
