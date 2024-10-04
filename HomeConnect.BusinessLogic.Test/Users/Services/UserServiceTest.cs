using BusinessLogic.Roles.Entities;
using BusinessLogic.Roles.Repositories;
using BusinessLogic.Users.Entities;
using BusinessLogic.Users.Models;
using BusinessLogic.Users.Repositories;
using BusinessLogic.Users.Services;
using FluentAssertions;
using Moq;

namespace HomeConnect.BusinessLogic.Test.Users.Services;

[TestClass]
public class UserServiceTest
{
    private Mock<IUserRepository> _userRepository = null!;
    private Mock<IRoleRepository> _roleRepository = null!;
    private IUserService _userService = null!;

    [TestInitialize]
    public void Initialize()
    {
        _userRepository = new Mock<IUserRepository>(MockBehavior.Strict);
        _roleRepository = new Mock<IRoleRepository>(MockBehavior.Strict);
        _userService = new UserService(_userRepository.Object, _roleRepository.Object);
    }

    [TestMethod]
    public void CreateUser_WithValidArguments_ShouldCreateUser()
    {
        // Arrange
        var args = new CreateUserArgs()
        {
            Email = "john.doe@gmail.com",
            Password = "password1M@",
            Name = "John",
            Surname = "Doe",
            Role = "Administrator"
        };
        _userRepository.Setup(x => x.Add(It.IsAny<User>()));
        _roleRepository.Setup(x => x.Exists(args.Role)).Returns(true);
        _roleRepository.Setup(x => x.Get(args.Role)).Returns(new Role());

        // Act
        _userService.CreateUser(args);

        // Assert
        _userRepository.Verify(x => x.Add(It.IsAny<User>()), Times.Once);
    }

    [TestMethod]
    public void CreateUser_WithInvalidRole_ShouldThrowException()
    {
        // Arrange
        var args = new CreateUserArgs()
        {
            Email = "john.doe@gmail.com",
            Password = "password1M@",
            Name = "John",
            Surname = "Doe",
            Role = "Administrator"
        };
        _roleRepository.Setup(x => x.Exists(args.Role)).Returns(false);

        // Act
        var act = () => _userService.CreateUser(args);

        // Assert
        act.Should().Throw<ArgumentException>();
    }
}
