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
public class MemberController : ControllerBase
{
    private readonly IHomeOwnerService _homeOwnerService;

    public MemberController(IHomeOwnerService homeOwnerService)
    {
        _homeOwnerService = homeOwnerService;
    }

    [HttpPatch("{membersId}/notifications")]
    [AuthorizationFilter(SystemPermission.UpdateMember)]
    [HomeAuthorizationFilter(HomePermission.UpdateNotifications)]
    public UpdateMemberNotificationsResponse UpdateMemberNotifications([FromRoute] string membersId,
        [FromBody] UpdateMemberNotificationsRequest request)
    {
        _homeOwnerService.UpdateMemberNotifications(Guid.Parse(membersId), request.ShouldBeNotified);
        return new UpdateMemberNotificationsResponse
        {
            MemberId = membersId, ShouldBeNotified = request.ShouldBeNotified!.Value
        };
    }
}
