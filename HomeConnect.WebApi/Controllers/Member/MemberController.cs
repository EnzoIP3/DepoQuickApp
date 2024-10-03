using HomeConnect.WebApi.Filters;
using HomeConnect.WebApi.Test.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HomeConnect.WebApi.Controllers.Member;

[ApiController]
[Route("members")]
[AuthorizationFilter]
public class MemberController() : ControllerBase
{
    [HttpPatch("{membersId}/notifications")]
    public UpdateMemberNotificationsResponse UpdateMemberNotifications([FromRoute] string membersId,
        [FromBody] UpdateMemberNotificationsRequest request)
    {
        throw new NotImplementedException();
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
