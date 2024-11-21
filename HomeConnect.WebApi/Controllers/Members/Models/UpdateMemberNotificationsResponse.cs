namespace HomeConnect.WebApi.Controllers.Members.Models;

public sealed record UpdateMemberNotificationsResponse
{
    public string MemberId { get; set; } = null!;
    public bool ShouldBeNotified { get; set; }
}
