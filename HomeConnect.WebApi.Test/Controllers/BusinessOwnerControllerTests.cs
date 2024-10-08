using BusinessLogic.Admins.Services;
using BusinessLogic.Roles.Entities;
using BusinessLogic.Users.Entities;
using BusinessLogic.Users.Models;
using BusinessLogic.Users.Services;
using FluentAssertions;
using HomeConnect.WebApi.Controllers.BusinessOwners;
using HomeConnect.WebApi.Controllers.BusinessOwners.Models;
using Moq;

namespace HomeConnect.WebApi.Test.Controllers;

[TestClass]
public class BusinessOwnerControllerTests
{
    private Mock<IUserService> _userService = null!;
    private CreateBusinessOwnerRequest _businessOwnerRequest = null!;
    private BusinessOwnerController _controller = null!;
    private Guid _guid;
    private CreateUserArgs _userModel = null!;

    [TestInitialize]
    public void Initialize()
    {
        _userService = new Mock<IUserService>();
        _controller = new BusinessOwnerController(_userService.Object);

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
        _userService.Setup(x => x.CreateUser(_userModel)).Returns(new User() { Id = _guid });

        // Act
        CreateBusinessOwnerResponse response = _controller.CreateBusinessOwner(_businessOwnerRequest);

        // Assert
        _userService.VerifyAll();
        response.Should().NotBeNull();
        response.Id.Should().Be(_guid.ToString());
    }

    #endregion
}
