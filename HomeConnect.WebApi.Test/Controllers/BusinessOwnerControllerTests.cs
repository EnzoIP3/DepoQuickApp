using BusinessLogic.Admins.Services;
using BusinessLogic.Roles.Entities;
using BusinessLogic.Users.Models;
using FluentAssertions;
using HomeConnect.WebApi.Controllers.BusinessOwner;
using HomeConnect.WebApi.Controllers.BusinessOwner.Models;
using Moq;

namespace HomeConnect.WebApi.Test.Controllers;

[TestClass]
public class BusinessOwnerControllerTests
{
    private Mock<IAdminService> _adminService = null!;
    private CreateBusinessOwnerRequest _businessOwnerRequest = null!;
    private BusinessOwnerController _controller = null!;
    private Guid _guid;
    private CreateUserArgs _userModel = null!;

    [TestInitialize]
    public void Initialize()
    {
        _adminService = new Mock<IAdminService>();
        _controller = new BusinessOwnerController(_adminService.Object);

        _businessOwnerRequest = new CreateBusinessOwnerRequest
        {
            Name = "John", Surname = "Doe", Email = "email@email.com", Password = "password"
        };
        _userModel = new CreateUserArgs
        {
            Name = _businessOwnerRequest.Name,
            Surname = _businessOwnerRequest.Surname,
            Email = _businessOwnerRequest.Email,
            Password = _businessOwnerRequest.Password,
            Role = Role.BusinessOwner
        };
        _guid = Guid.NewGuid();
    }

    #region CreateBusinessOwner

    [TestMethod]
    public void CreateBusinessOwner_WhenCalledWithValidRequest_ReturnsCreatedResponse()
    {
        // Arrange
        _adminService.Setup(x => x.CreateBusinessOwner(_userModel)).Returns(_guid);

        // Act
        CreateBusinessOwnerResponse response = _controller.CreateBusinessOwner(_businessOwnerRequest);

        // Assert
        _adminService.VerifyAll();
        response.Should().NotBeNull();
        response.Id.Should().Be(_guid.ToString());
    }

    #endregion
}
