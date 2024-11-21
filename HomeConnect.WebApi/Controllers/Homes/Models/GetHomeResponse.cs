using BusinessLogic.HomeOwners.Entities;

namespace HomeConnect.WebApi.Controllers.Homes.Models;

public sealed record GetHomeResponse
{
    public string Id { get; init; } = null!;
    public string? Name { get; init; }
    public OwnerInfo Owner { get; init; } = null!;
    public string Address { get; init; } = null!;
    public double Latitude { get; init; }
    public double Longitude { get; init; }
    public int MaxMembers { get; init; }

    public static GetHomeResponse FromHome(Home home)
    {
        return new GetHomeResponse
        {
            Id = home.Id.ToString(),
            Name = home.NickName,
            Address = home.Address,
            Latitude = home.Latitude,
            Longitude = home.Longitude,
            MaxMembers = home.MaxMembers,
            Owner = new OwnerInfo
            {
                Id = home.Owner.Id.ToString(),
                Name = home.Owner.Name,
                Surname = home.Owner.Surname,
                ProfilePicture = home.Owner.ProfilePicture
            }
        };
    }
}
