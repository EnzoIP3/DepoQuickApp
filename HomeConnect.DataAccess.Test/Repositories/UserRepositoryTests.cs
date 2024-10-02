using BusinessLogic.Roles.Entities;
using BusinessLogic.Users.Entities;
using FluentAssertions;
using HomeConnect.DataAccess.Users;

namespace HomeConnect.DataAccess.Test.Users;

[TestClass]
public class UserRepositoryTest
{
    private const string ValidUserEmail = "john.doe@example.com";
    private readonly Context _context = DbContextBuilder.BuildTestDbContext();
    private UserRepository _userRepository = null!;
    private User _validUser = null!;
    private User _otherUser = null!;

    [TestInitialize]
    public void Initialize()
    {
        _context.Database.EnsureCreated();
        var adminRole = _context.Roles.First(r => r.Name == "Admin");
        var businessOwnerRole = _context.Roles.First(r => r.Name == "Business Owner");
        _userRepository = new UserRepository(_context);
        _validUser = new User("John", "Doe", ValidUserEmail, "Password#100", adminRole);
        _otherUser = new User("Jane", "Doe", "jane.doe@example.com", "Password#200", businessOwnerRole);
        _context.Users.Add(_validUser);
        _context.Users.Add(_otherUser);
        _context.SaveChanges();
    }

    [TestCleanup]
    public void Cleanup()
    {
        _context.Database.EnsureDeleted();
    }

    #region Exists

    #region Success

    [TestMethod]
    public void Exists_WhenUserExists_ReturnsTrue()
    {
        // Act
        var result = _userRepository.Exists("john.doe@example.com");

        // Assert
        result.Should().BeTrue();
    }

    #endregion

    #endregion

    #region Add

    #region Success

    [TestMethod]
    public void Add_WhenUserDoesNotExist_AddsUser()
    {
        // Arrange
        var user = new User("Name", "Surname", "test@example.com", "12345678M@", new Role("Role", []));

        // Act
        _userRepository.Add(user);
        var exists = _userRepository.Exists(user.Email);

        // Assert
        exists.Should().BeTrue();
    }

    #endregion

    #region Error

    [TestMethod]
    public void Add_WhenUserAlreadyExists_ThrowsException()
    {
        // Arrange
        var user = new User("John", "Doe", ValidUserEmail, "12345678M@", new Role("Role", []));

        // Act
        var act = () => _userRepository.Add(user);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    #endregion

    #endregion

    #region Delete

    #region Success

    [TestMethod]
    public void Delete_WhenUserExists_DeletesUser()
    {
        // Act
        _userRepository.Delete(ValidUserEmail);

        // Assert
        _userRepository.Exists(ValidUserEmail).Should().BeFalse();
    }

    #endregion

    #region Error

    [TestMethod]
    public void Delete_WhenUserDoesNotExist_ThrowsException()
    {
        // Act
        var act = () => _userRepository.Delete("nonexistent@example.com");

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    #endregion

    #endregion

    #region GetUsers

    #region Success

    [TestMethod]
    public void GetUsers_WhenCalled_ReturnsPaginatedUsers()
    {
        // Act
        var result = _userRepository.GetUsers(1, 2);

        // Assert
        result.Should().HaveCount(2);
        result.Exists(u => u.Email == ValidUserEmail).Should().BeTrue();
    }

    [TestMethod]
    public void GetUsers_WhenFilteredByFullName_ReturnsFilteredUsers()
    {
        // Act
        var result = _userRepository.GetUsers(1, 10, fullNameFilter: "Jane");

        // Assert
        result.Should().HaveCount(1);
        result.First().Email.Should().Be("jane.doe@example.com");
    }

    [TestMethod]
    public void GetUsers_WhenFilteredByFullNameAndRole_ReturnsFilteredUsers()
    {
        // Act
        var result = _userRepository.GetUsers(1, 10, "J", "Admin");

        // Assert
        result.Should().HaveCount(1);
        result.First().Email.Should().Be(ValidUserEmail);
    }

    #endregion

    #endregion

    #region Get
    #region Success
    [TestMethod]
    public void Get_WhenUserExists_ReturnsUser()
    {
        // Act
        var result = _userRepository.Get(_validUser.Id);

        // Assert
        result.Should().BeEquivalentTo(_validUser);
    }
    #endregion
    #region Error
    [TestMethod]
    public void Get_WhenUserDoesNotExist_ShouldThrowException()
    {
        // Arrange
        var nonExistentUserId = Guid.NewGuid();

        // Act
        Action action = () => _userRepository.Get(nonExistentUserId);

        // Assert
        action.Should().Throw<ArgumentException>();
    }
    #endregion
    #endregion

    #region Exists
    #region Success

    [TestMethod]
    public void Exists_WhenUserExists_ShouldReturnTrue()
    {
        // Act
        var exists = _userRepository.Exists(_validUser.Id);

        // Assert
        exists.Should().BeTrue();
    }

    [TestMethod]
    public void Exists_WhenUserDoesNotExist_ShouldReturnFalse()
    {
        // Arrange
        var nonExistentUserId = Guid.NewGuid();

        // Act
        var exists = _userRepository.Exists(nonExistentUserId);

        // Assert
        exists.Should().BeFalse();
    }

    #endregion

    #endregion

    #region Delete
    #region Success

    [TestMethod]
    public void Delete_WhenUserExists_ShouldDeleteUser()
    {
        // Act
        _userRepository.Delete(_validUser.Id);

        // Assert
        _userRepository.Exists(_validUser.Id).Should().BeFalse();
    }

    #endregion

    #region Error

    [TestMethod]
    public void Delete_WhenUserDoesNotExist_ShouldThrowException()
    {
        // Arrange
        var nonExistentUserId = Guid.NewGuid();

        // Act
        Action act = () => _userRepository.Delete(nonExistentUserId);

        // Assert
        act.Should().Throw<ArgumentException>();
    }
    #endregion

    #endregion
}
