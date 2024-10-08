namespace HomeConnect.WebApi.Controllers.Homes.Models;

public record GetMembersResponse
{
    public List<ListMemberInfo> Members { get; set; } = null!;
}
