namespace HomeConnect.WebApi.Controllers.Members.Models;

public record UpdateMemberNotificationsResponse
{
    public string MemberId { get; set; } = null!;
    public bool ShouldBeNotified { get; set; }
}
