using BusinessLogic.Roles.Entities;
using BusinessLogic.Users.Entities;
using FluentAssertions;
using HomeConnect.DataAccess.Repositories;

namespace HomeConnect.DataAccess.Test.Repositories;

[TestClass]
public class UserRepositoryTest
{
    private readonly Context _context = DbContextBuilder.BuildTestDbContext();
    private UserRepository _userRepository = null!;
    private User _validUser = null!;
    private User _otherUser = null!;

    [TestInitialize]
    public void Initialize()
    {
        _context.Database.EnsureCreated();
        var role = new Role("Role 1", []);
        var otherRole = new Role("Role 2", []);
        _context.SaveChanges();
        _userRepository = new UserRepository(_context);
        _validUser = new User("John", "Doe", "john.doe@example.com", "Password#100", role);
        _otherUser = new User("Jane", "Doe", "jane.doe@example.com", "Password#200", otherRole);
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
    public void ExistsById_WhenUserExists_ReturnsTrue()
    {
        // Act
        var result = _userRepository.Exists(_validUser.Id);

        // Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void ExistsByEmail_WhenUserExists_ReturnsTrue()
    {
        // Act
        var result = _userRepository.Exists(_validUser.Email);

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
        var exists = _userRepository.Exists(user.Id);

        // Assert
        exists.Should().BeTrue();
    }

    #endregion

    #endregion

    #region Delete

    #region Success

    [TestMethod]
    public void Delete_WhenUserExists_DeletesUser()
    {
        // Act
        _userRepository.Delete(_validUser.Id);

        // Assert
        _userRepository.Exists(_validUser.Id).Should().BeFalse();
    }

    #endregion

    #endregion

    #region GetUsers

    #region Success

    [TestMethod]
    public void GetUsers_WhenCalled_ReturnsPaginatedUsers()
    {
        // Act
        var result = _userRepository.GetAllPaged(1, 2);

        // Assert
        result.Data.Should().HaveCount(2);
        result.Data.Exists(u => u.Email == _validUser.Email).Should().BeTrue();
    }

    [TestMethod]
    public void GetUsers_WhenFilteredByFullName_ReturnsFilteredUsers()
    {
        // Act
        var result = _userRepository.GetAllPaged(1, 10, fullNameFilter: "Jane");

        // Assert
        result.Data.Should().HaveCount(1);
        result.Data.First().Email.Should().Be("jane.doe@example.com");
    }

    [TestMethod]
    public void GetUsers_WhenFilteredByFullNameAndRole_ReturnsFilteredUsers()
    {
        // Act
        var result = _userRepository.GetAllPaged(1, 10, "J", "Role 1");

        // Assert
        result.Data.Should().HaveCount(1);
        result.Data.First().Email.Should().Be(_validUser.Email);
    }

    #endregion

    #endregion

    #region Get

    #region Success

    [TestMethod]
    public void GetById_WhenUserExists_ReturnsUser()
    {
        // Act
        var result = _userRepository.Get(_validUser.Id);

        // Assert
        result.Should().NotBeNull();
        result.Email.Should().Be(_validUser.Email);
    }

    [TestMethod]
    public void GetByEmail_WhenUserExists_ReturnsUser()
    {
        // Act
        var result = _userRepository.Get(_validUser.Email);

        // Assert
        result.Should().NotBeNull();
        result.Email.Should().Be(_validUser.Email);
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

    #endregion
}
