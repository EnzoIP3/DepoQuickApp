using BusinessLogic;
using FluentAssertions;
using HomeConnect.WebApi.Controllers.BusinessOwner;
using Moq;

namespace HomeConnect.WebApi.Test.Controllers;

[TestClass]
public class BusinessOwnerControllerTests
{
    private Mock<IAdminService> _adminService = null!;
    private BusinessOwnerController _controller = null!;

    [TestInitialize]
    public void Initialize()
    {
        _adminService = new Mock<IAdminService>();
        _controller = new BusinessOwnerController(_adminService.Object);
    }

    [TestMethod]
    public void CreateBusinessOwner_WhenCalledWithValidRequest_ReturnsCreatedResponse()
    {
        // Arrange
        var request = new CreateBusinessOwnerRequest
        {
            Name = "John", Surname = "Doe", Email = "email@email.com", Password = "password"
        };
        var userModel = new UserModel
        {
            Name = request.Name,
            Surname = request.Surname,
            Email = request.Email,
            Password = request.Password
        };
        var guid = Guid.NewGuid();
        _adminService.Setup(x => x.CreateBusinessOwner(userModel)).Returns(guid);

        // Act
        var response = _controller.CreateBusinessOwner(request, $"Bearer {guid}");

        // Assert
        _adminService.VerifyAll();
        response.Should().NotBeNull();
        response.Id.Should().Be(guid.ToString());
    }
}
