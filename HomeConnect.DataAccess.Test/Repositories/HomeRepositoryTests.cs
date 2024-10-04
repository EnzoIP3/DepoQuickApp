using BusinessLogic.HomeOwners.Entities;
using BusinessLogic.Roles.Entities;
using BusinessLogic.Users.Entities;
using FluentAssertions;
using HomeConnect.DataAccess.Repositories;

namespace HomeConnect.DataAccess.Test.Repositories;

[TestClass]
public class HomeRepositoryTests
{
    private readonly Context _context = DbContextBuilder.BuildTestDbContext();
    private HomeRepository _homeRepository = null!;
    private User _homeOwner = null!;
    private Home _home = null!;

    [TestInitialize]
    public void Initialize()
    {
        _context.Database.EnsureCreated();

        var homeOwnerRole = new Role { Name = "HomeOwner", Permissions = new List<SystemPermission>() };
        _homeOwner = new User("John", "Doe", "email@email.com", "Password#100", homeOwnerRole);
        _home = new Home(_homeOwner, "Main St 123", 12.5, 12.5, 5);
        _context.Users.Add(_homeOwner);
        _context.Homes.Add(_home);

        _context.SaveChanges();

        _homeRepository = new HomeRepository(_context);
    }

    [TestCleanup]
    public void Cleanup()
    {
        _context.Database.EnsureDeleted();
    }

    #region Add

    #region Success
    [TestMethod]
    public void Add_WhenHomeDoesNotExist_AddsHome()
    {
        // Arrange
        var home = new Home(_homeOwner, "Main St 123", 12.5, 12.5, 5);

        // Act
        _homeRepository.Add(home);

        // Assert
        _context.Homes.Should().Contain(home);
    }
    #endregion

    #region Error
    [TestMethod]
    public void Add_WhenHomeExists_ThrowsException()
    {
        // Arrange
        var home = new Home(_homeOwner, "Main St 123", 12.5, 12.5, 5);

        // Act
        var action = () => _homeRepository.Add(home);

        // Assert
        action.Should().Throw<ArgumentException>().WithMessage("Home already exists");
    }
    #endregion
    #endregion
}
