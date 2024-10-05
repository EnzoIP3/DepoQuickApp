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

    private CreateUserArgs _args = new CreateUserArgs()
    {
        Email = "john.doe@gmail.com",
        Password = "password1M@",
        Name = "John",
        Surname = "Doe",
        Role = "Administrator"
    };

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
    public void CreateUser_WithInvalidRole_ShouldThrowException()
    {
        // Arrange
        _roleRepository.Setup(x => x.Exists(_args.Role)).Returns(false);

        // Act
        var act = () => _userService.CreateUser(_args);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void CreateUser_WithAlreadyExistingUser_ShouldThrowException()
    {
        // Arrange
        _roleRepository.Setup(x => x.Exists(_args.Role)).Returns(true);
        _userRepository.Setup(x => x.ExistsByEmail(_args.Email)).Returns(true);

        // Act
        var act = () => _userService.CreateUser(_args);

        // Assert
        act.Should().Throw<InvalidOperationException>();
    }

    [TestMethod]
    public void CreateUser_WithHomeOwnerRoleAndWithoutProfilePicture_ShouldThrowException()
    {
        // Arrange
        _args.Role = "HomeOwner";
        _args.ProfilePicture = null;
        _roleRepository.Setup(x => x.Exists(_args.Role)).Returns(true);
        _roleRepository.Setup(x => x.Get(_args.Role)).Returns(new Role(Role.HomeOwner, []));
        _userRepository.Setup(x => x.ExistsByEmail(_args.Email)).Returns(false);

        // Act
        var act = () => _userService.CreateUser(_args);

        // Assert
        act.Should().Throw<ArgumentException>();
    }
}
