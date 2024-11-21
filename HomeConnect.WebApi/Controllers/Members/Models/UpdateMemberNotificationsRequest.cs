namespace HomeConnect.WebApi.Controllers.Members.Models;

public sealed record UpdateMemberNotificationsRequest
{
    public bool? ShouldBeNotified { get; set; }
}
