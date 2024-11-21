using BusinessLogic.HomeOwners.Entities;

namespace HomeConnect.WebApi.Controllers.Homes.Models;

public sealed record GetMembersResponse
{
    public List<ListMemberInfo> Members { get; set; } = null!;

    public static GetMembersResponse FromMembers(List<Member> members)
    {
        var memberInfos = members.Select(m => new ListMemberInfo
        {
            Id = m.Id.ToString(),
            Name = m.User.Name,
            Surname = m.User.Surname,
            Photo = m.User.ProfilePicture ?? string.Empty,
            Permissions = m.HomePermissions.Select(p => p.Value).ToList()
        }).ToList();
        return new GetMembersResponse { Members = memberInfos };
    }
}
