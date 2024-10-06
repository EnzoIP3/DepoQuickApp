namespace HomeConnect.WebApi.Controllers.Home.Models;

public record GetMembersResponse
{
    public List<ListMemberInfo> Members { get; set; } = null!;
}
