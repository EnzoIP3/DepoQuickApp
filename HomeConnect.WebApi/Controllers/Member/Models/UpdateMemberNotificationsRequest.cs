namespace HomeConnect.WebApi.Controllers.Member;

public record UpdateMemberNotificationsRequest
{
    public bool ShouldBeNotified { get; set; }
}
