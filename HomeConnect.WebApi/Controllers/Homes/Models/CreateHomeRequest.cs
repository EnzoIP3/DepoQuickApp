using BusinessLogic.HomeOwners.Models;
using BusinessLogic.Users.Entities;

namespace HomeConnect.WebApi.Controllers.Homes.Models;

public class CreateHomeRequest
{
    public string? Address { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public int MaxMembers { get; set; }
    public string? Name { get; set; }

    public CreateHomeArgs ToArgs(User user)
    {
        return new CreateHomeArgs
        {
            HomeOwnerId = user.Id.ToString(),
            Address = Address ?? string.Empty,
            Latitude = Latitude,
            Longitude = Longitude,
            MaxMembers = MaxMembers,
            Name = Name
        };
    }
}
