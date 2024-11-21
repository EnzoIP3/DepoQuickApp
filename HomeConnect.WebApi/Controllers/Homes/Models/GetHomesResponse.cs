using BusinessLogic.HomeOwners.Entities;
using HomeConnect.WebApi.Controllers.HomeOwners.Models;

namespace HomeConnect.WebApi.Controllers.Homes.Models;

public class GetHomesResponse
{
    public List<ListHomeInfo> Homes { get; set; } = null!;

    public static GetHomesResponse FromHomes(List<Home> homes)
    {
        var homeInfos = homes.Select(h => new ListHomeInfo
        {
            Id = h.Id.ToString(),
            Name = h.NickName,
            Address = h.Address,
            Latitude = h.Latitude,
            Longitude = h.Longitude,
            MaxMembers = h.MaxMembers
        }).ToList();
        return new GetHomesResponse { Homes = homeInfos };
    }
}
