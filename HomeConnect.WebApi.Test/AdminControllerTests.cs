using BusinessLogic;
using FluentAssertions;
using HomeConnect.WebApi.Controllers;
using Moq;

namespace HomeConnect.WebApi.Test;

[TestClass]
public class AdminControllerTests
{
    private Mock<IAdminService> _adminService = null!;
    private AdminController _controller = null!;

    [TestInitialize]
    public void Initialize()
    {
        _adminService = new Mock<IAdminService>();
        _controller = new AdminController(_adminService.Object);
    }

    [TestMethod]
    public void CreateAdmin_WhenCalledWithValidRequest_ReturnsCreatedResponse()
    {
        // Arrange
        var request = new CreateAdminRequest
        {
            Name = "John", Surname = "Doe", Email = "email@email.com", Password = "password"
        };
        var guid = Guid.NewGuid();
        _adminService.Setup(x => x.Create(It.IsAny<UserModel>())).Returns(guid);

        // Act
        var response = _controller.CreateAdmin(request, $"Bearer {guid}");

        // Assert
        _adminService.VerifyAll();
        response.Should().NotBeNull();
        response.Id.Should().Be(guid.ToString());
    }
}
