namespace HomeConnect.WebApi.Controllers.Member;

public record UpdateMemberNotificationsResponse
{
    public string MemberId { get; set; } = null!;
    public bool ShouldBeNotified { get; set; }
}
