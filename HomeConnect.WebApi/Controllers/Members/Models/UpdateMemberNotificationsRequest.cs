namespace HomeConnect.WebApi.Controllers.Members.Models;

public record UpdateMemberNotificationsRequest
{
    public bool? ShouldBeNotified { get; set; }
}
