using FluentAssertions;
using HomeConnect.WebApi.Controllers.Member;
using Moq;

namespace HomeConnect.WebApi.Test.Controllers;

[TestClass]
public class MemberControllerTests
{
    private Mock<IHomeOwnerService> _homeOwnerService = null!;
    private MemberController _memberController = null!;

    [TestInitialize]
    public void Initialize()
    {
        _homeOwnerService = new Mock<IHomeOwnerService>();
        _memberController = new MemberController();
    }

    #region UpdateMemberNotifications
    [TestMethod]
    public void UpdateMemberNotifications_WhenCalled_ShouldUpdateMemberNotifications()
    {
        // Arrange
        var memberId = Guid.NewGuid().ToString();
        var request = new UpdateMemberNotificationsRequest
        {
            ShouldBeNotified = true
        };
        _homeOwnerService.Setup(x => x.UpdateMemberNotifications(Guid.Parse(memberId), request.ShouldBeNotified));
        var expectedResult = new UpdateMemberNotificationsResponse
        {
            MemberId = memberId,
            ShouldBeNotified = request.ShouldBeNotified
        };

        // Act
        var result = _memberController.UpdateMemberNotifications(memberId, request);

        // Assert
        _homeOwnerService.Verify(x => x.UpdateMemberNotifications(Guid.Parse(memberId), request.ShouldBeNotified));
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedResult);
    }
    #endregion
}
