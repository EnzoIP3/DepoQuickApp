namespace HomeConnect.WebApi.Controllers.Member;

public struct UpdateMemberNotificationsResponse
{
    public string MemberId { get; set; }
    public bool ShouldBeNotified { get; set; }
}
