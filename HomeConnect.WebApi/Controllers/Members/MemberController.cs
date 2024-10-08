using BusinessLogic.HomeOwners.Entities;
using BusinessLogic.HomeOwners.Services;
using BusinessLogic.Roles.Entities;
using HomeConnect.WebApi.Controllers.Members.Models;
using HomeConnect.WebApi.Filters;
using Microsoft.AspNetCore.Mvc;

namespace HomeConnect.WebApi.Controllers.Members;

[ApiController]
[Route("members")]
[AuthenticationFilter]
public class MemberController(IHomeOwnerService homeOwnerService) : ControllerBase
{
    [HttpPatch("{membersId}/notifications")]
    [AuthorizationFilter(SystemPermission.UpdateMember)]
    [HomeAuthorizationFilter(HomePermission.UpdateNotifications)]
    public UpdateMemberNotificationsResponse UpdateMemberNotifications([FromRoute] string membersId,
        [FromBody] UpdateMemberNotificationsRequest request)
    {
        homeOwnerService.UpdateMemberNotifications(Guid.Parse(membersId), request.ShouldBeNotified);
        var response = new UpdateMemberNotificationsResponse
        {
            MemberId = membersId,
            ShouldBeNotified = request.ShouldBeNotified!.Value
        };
        return response;
    }
}
