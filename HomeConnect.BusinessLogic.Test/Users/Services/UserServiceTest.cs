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
    private readonly CreateUserArgs _args = new()
    {
        Email = "john.doe@gmail.com",
        Password = "password1M@",
        Name = "John",
        Surname = "Doe",
        Role = "Administrator"
    };

    private Mock<IRoleRepository> _roleRepository = null!;
    private Mock<IUserRepository> _userRepository = null!;
    private IUserService _userService = null!;

    [TestInitialize]
    public void Initialize()
    {
        _userRepository = new Mock<IUserRepository>(MockBehavior.Strict);
        _roleRepository = new Mock<IRoleRepository>(MockBehavior.Strict);
        _userService = new UserService(_userRepository.Object, _roleRepository.Object);
    }

    [TestMethod]
    public void CreateUser_WhenArgumentsAreValid_CreatesUser()
    {
        // Arrange
        _userRepository.Setup(x => x.Add(It.IsAny<User>()));
        _userRepository.Setup(x => x.ExistsByEmail(_args.Email)).Returns(false);
        _roleRepository.Setup(x => x.Exists(_args.Role)).Returns(true);
        _roleRepository.Setup(x => x.Get(_args.Role)).Returns(new Role());

        // Act
        _userService.CreateUser(_args);

        // Assert
        _userRepository.Verify(x => x.Add(It.IsAny<User>()), Times.Once);
    }

    [TestMethod]
    public void CreateUser_WhenRoleIsInvalid_ThrowsException()
    {
        // Arrange
        _roleRepository.Setup(x => x.Exists(_args.Role)).Returns(false);

        // Act
        Func<User> act = () => _userService.CreateUser(_args);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void CreateUser_WhenUserAlreadyExists_ThrowsException()
    {
        // Arrange
        _roleRepository.Setup(x => x.Exists(_args.Role)).Returns(true);
        _userRepository.Setup(x => x.ExistsByEmail(_args.Email)).Returns(true);

        // Act
        Func<User> act = () => _userService.CreateUser(_args);

        // Assert
        act.Should().Throw<InvalidOperationException>();
    }

    [TestMethod]
    public void CreateUser_WhenHomeOwnerRoleAndNoProfilePicture_ThrowsException()
    {
        // Arrange
        _args.Role = "HomeOwner";
        _args.ProfilePicture = null;
        _roleRepository.Setup(x => x.Exists(_args.Role)).Returns(true);
        _roleRepository.Setup(x => x.Get(_args.Role)).Returns(new Role(Role.HomeOwner, []));
        _userRepository.Setup(x => x.ExistsByEmail(_args.Email)).Returns(false);

        // Act
        Func<User> act = () => _userService.CreateUser(_args);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void Exists_WhenUserIsNotAValidGuid_ReturnsFalse()
    {
        // Arrange
        var requestUserId = "invalidGuid";

        // Act
        var result = _userService.Exists(requestUserId);

        // Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void Exists_WhenUserIdCorrespondsToARegisteredUser_ReturnsTrue()
    {
        // Arrange
        var requestUserId = Guid.NewGuid().ToString();
        _userRepository.Setup(x => x.Exists(Guid.Parse(requestUserId))).Returns(true);

        // Act
        var result = _userService.Exists(requestUserId);

        // Assert
        result.Should().BeTrue();
    }
}
