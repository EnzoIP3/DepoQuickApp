using HomeConnect.WebApi.Controllers.HomeOwners.Models;

namespace HomeConnect.WebApi.Controllers.Homes.Models;
public class GetHomesResponse
{
    public List<ListHomeInfo> Homes { get; set; } = null!;
}
