using BusinessLogic;
using BusinessLogic.Roles.Entities;
using BusinessLogic.Users.Entities;
using FluentAssertions;
using HomeConnect.DataAccess.Repositories;

namespace HomeConnect.DataAccess.Test.Repositories;

[TestClass]
public class UserRepositoryTest
{
    private readonly Context _context = DbContextBuilder.BuildTestDbContext();
    private User _otherUser = null!;
    private UserRepository _userRepository = null!;
    private User _validUser = null!;

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
        _context.SaveChanges();
        _context.Users.Add(_otherUser);
        _context.SaveChanges();
    }

    [TestCleanup]
    public void Cleanup()
    {
        _context.Database.EnsureDeleted();
    }

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

    #region Update

    #region Success

    [TestMethod]
    public void Update_WhenUserExists_UpdatesUser()
    {
        // Arrange
        _validUser.AddRole(new Role("Added role", []));

        // Act
        _userRepository.Update(_validUser);

        // Assert
        User user = _userRepository.Get(_validUser.Id);
        user.Roles.Should().HaveCount(2);
    }

    #endregion

    #endregion

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
        var result = _userRepository.ExistsByEmail(_validUser.Email);

        // Assert
        result.Should().BeTrue();
    }

    #endregion

    #endregion

    #region GetUsers

    #region Success

    [TestMethod]
    public void GetUsers_WhenCalled_ReturnsPaginatedUsers()
    {
        // Arrange
        var filterArgs = new FilterArgs { CurrentPage = 1, PageSize = 2 };

        // Act
        PagedData<User> result = _userRepository.GetPaged(filterArgs);

        // Assert
        result.Data.Should().HaveCount(2);
        result.Data.Exists(u => u.Email == _validUser.Email).Should().BeTrue();
    }

    [TestMethod]
    public void GetUsers_WhenFilteredByFullName_ReturnsFilteredUsers()
    {
        // Arrange
        var filterArgs = new FilterArgs { FullNameFilter = "Jane" };

        // Act
        PagedData<User> result = _userRepository.GetPaged(filterArgs);

        // Assert
        result.Data.Should().HaveCount(1);
        result.Data.First().Email.Should().Be("jane.doe@example.com");
    }

    [TestMethod]
    public void GetUsers_WhenFilteredByFullNameAndRole_ReturnsFilteredUsers()
    {
        // Arrange
        var filterArgs = new FilterArgs { FullNameFilter = "J", RoleFilter = "Role 1" };

        // Act
        PagedData<User> result = _userRepository.GetPaged(filterArgs);

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
        User result = _userRepository.Get(_validUser.Id);

        // Assert
        result.Should().NotBeNull();
        result.Email.Should().Be(_validUser.Email);
    }

    [TestMethod]
    public void GetByEmail_WhenUserExists_ReturnsUser()
    {
        // Act
        User result = _userRepository.GetByEmail(_validUser.Email);

        // Assert
        result.Should().NotBeNull();
        result.Email.Should().Be(_validUser.Email);
    }

    #endregion

    #endregion

    #region Exists

    #region Success

    [TestMethod]
    public void Exists_WhenUserExists_ReturnsTrue()
    {
        // Act
        var exists = _userRepository.Exists(_validUser.Id);

        // Assert
        exists.Should().BeTrue();
    }

    [TestMethod]
    public void Exists_WhenUserDoesNotExist_ReturnsFalse()
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
}
