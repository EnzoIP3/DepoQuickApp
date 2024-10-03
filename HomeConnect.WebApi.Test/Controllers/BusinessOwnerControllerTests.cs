using BusinessLogic.Admins.Services;
using BusinessLogic.BusinessOwners.Entities;
using BusinessLogic.BusinessOwners.Services;
using BusinessLogic.Users.Entities;
using BusinessLogic.Users.Models;
using FluentAssertions;
using HomeConnect.WebApi.Controllers.BusinessOwner;
using Moq;

namespace HomeConnect.WebApi.Test.Controllers;

[TestClass]
public class BusinessOwnerControllerTests
{
    private Mock<IAdminService> _adminService = null!;
    private Mock<IBusinessOwnerService> _businessOwnerService = null!;
    private BusinessOwnerController _controller = null!;

    [TestInitialize]
    public void Initialize()
    {
        _adminService = new Mock<IAdminService>();
        _businessOwnerService = new Mock<IBusinessOwnerService>();
        _controller = new BusinessOwnerController(_adminService.Object);
    }

    [TestMethod]
    public void CreateBusinessOwner_WhenCalledWithValidRequest_ReturnsCreatedResponse()
    {
        // Arrange
        var request = new CreateBusinessOwnerRequest
        {
            Name = "John",
            Surname = "Doe",
            Email = "email@email.com",
            Password = "password"
        };
        var userModel = new CreateUserArgs
        {
            Name = request.Name,
            Surname = request.Surname,
            Email = request.Email,
            Password = request.Password
        };
        var guid = Guid.NewGuid();
        _adminService.Setup(x => x.CreateBusinessOwner(userModel)).Returns(guid);

        // Act
        var response = _controller.CreateBusinessOwner(request);

        // Assert
        _adminService.VerifyAll();
        response.Should().NotBeNull();
        response.Id.Should().Be(guid.ToString());
    }

    [TestMethod]
    public void CreateBusiness_WhenCalledWithValidRequest_ReturnsCreatedResponse()
    {
        // Arrange
        var request = new CreateBusinessRequest
        {
            Name = "OSE",
            Rut = "306869575",
            Owner = new User { Id = Guid.NewGuid() }
        };
        var business = new Business
        {
            Name = request.Name,
            Rut = request.Rut,
            Owner = request.Owner
        };
        _businessOwnerService.Setup(x => x.CreateBusiness(business.Owner.Email, business.Rut, business.Name)).Returns(business.Rut);

        // Act
        var response = _controller.CreateBusiness(request, $"Bearer {business.Owner.Id}");

        // Assert
        _businessOwnerService.VerifyAll();
        response.Should().NotBeNull();
        response.Id.Should().Be(business.Rut);
    }
}
