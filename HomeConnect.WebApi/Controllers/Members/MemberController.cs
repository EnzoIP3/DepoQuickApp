using BusinessLogic.HomeOwners.Services;
using HomeConnect.WebApi.Filters;
using Microsoft.AspNetCore.Mvc;

namespace HomeConnect.WebApi.Controllers.Member;

[ApiController]
[Route("members")]
[AuthenticationFilter]
public class MemberController(IHomeOwnerService homeOwnerService) : ControllerBase
{
    [HttpPatch("{membersId}/notifications")]
    public UpdateMemberNotificationsResponse UpdateMemberNotifications([FromRoute] string membersId,
        [FromBody] UpdateMemberNotificationsRequest request)
    {
        homeOwnerService.UpdateMemberNotifications(Guid.Parse(membersId), request.ShouldBeNotified);
        var response = new UpdateMemberNotificationsResponse
        {
            MemberId = membersId, ShouldBeNotified = request.ShouldBeNotified
        };
        return response;
    }
}
