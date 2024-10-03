using HomeConnect.WebApi.Filters;
using HomeConnect.WebApi.Test.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HomeConnect.WebApi.Controllers.Member;

[ApiController]
[Route("members")]
[AuthorizationFilter]
public class MemberController(IHomeOwnerService homeOwnerService) : ControllerBase
{
    [HttpPatch("{membersId}/notifications")]
    public UpdateMemberNotificationsResponse UpdateMemberNotifications([FromRoute] string membersId,
        [FromBody] UpdateMemberNotificationsRequest request)
    {
        homeOwnerService.UpdateMemberNotifications(Guid.Parse(membersId), request.ShouldBeNotified);
        var response = new UpdateMemberNotificationsResponse
        {
            MemberId = membersId,
            ShouldBeNotified = request.ShouldBeNotified
        };
        return response;
    }
}

public struct UpdateMemberNotificationsRequest
{
    public bool ShouldBeNotified { get; set; }
}

public struct UpdateMemberNotificationsResponse
{
    public string MemberId { get; set; }
    public bool ShouldBeNotified { get; set; }
}
