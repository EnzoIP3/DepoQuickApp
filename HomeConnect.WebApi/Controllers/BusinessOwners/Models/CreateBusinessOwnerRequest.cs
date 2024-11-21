using BusinessLogic.Roles.Entities;
using BusinessLogic.Users.Models;

namespace HomeConnect.WebApi.Controllers.BusinessOwners.Models;

public record CreateBusinessOwnerRequest
{
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }

    public CreateUserArgs ToCreateUserArgs()
    {
        return new CreateUserArgs
        {
            Name = Name!,
            Surname = Surname!,
            Email = Email!,
            Password = Password!,
            Role = Role.BusinessOwner
        };
    }
}
