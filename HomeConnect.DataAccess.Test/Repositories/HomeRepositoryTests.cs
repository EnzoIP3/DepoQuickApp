using BusinessLogic.HomeOwners.Entities;
using BusinessLogic.Roles.Entities;
using BusinessLogic.Users.Entities;
using FluentAssertions;
using HomeConnect.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HomeConnect.DataAccess.Test.Repositories;

[TestClass]
public class HomeRepositoryTests
{
    private readonly Context _context = DbContextBuilder.BuildTestDbContext();
    private Home _home = null!;
    private User _homeOwner = null!;
    private HomeRepository _homeRepository = null!;
    private Member _member = null!;
    private User _otherOwner = null!;
    private Room _room = null!;

    [TestInitialize]
    public void Initialize()
    {
        _context.Database.EnsureCreated();

        Role homeOwnerRole = _context.Roles.First(r => r.Name == "HomeOwner");
        _homeOwner = new User("John", "Doe", "email@email.com", "Password#100", homeOwnerRole);
        _otherOwner = new User("Jane", "Doe", "email2@email.com", "Password#100", homeOwnerRole);
        _home = new Home(_homeOwner, "Main St 123", 12.5, 12.5, 5);
        _member = new Member(_otherOwner);
        _home.AddMember(_member);
        _room = new Room { Id = Guid.NewGuid(), Name = "Bath room", Home = _home };
        _context.Users.Add(_homeOwner);
        _context.Homes.Add(_home);
        _context.Rooms.Add(_room);

        _context.SaveChanges();

        _homeRepository = new HomeRepository(_context);
    }

    [TestCleanup]
    public void Cleanup()
    {
        _context.Database.EnsureDeleted();
    }

    #region Exists

    [TestMethod]
    public void Exists_WhenHomeExists_ReturnsTrue()
    {
        // Act
        var result = _homeRepository.Exists(_home.Id);

        // Assert
        result.Should().BeTrue();
    }

    #endregion

    #region Add

    #region Success

    [TestMethod]
    public void Add_WhenHomeDoesNotExist_AddsHome()
    {
        // Arrange
        var home = new Home(_homeOwner, "Main St 456", 12.5, 12.5, 5);

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
        Action action = () => _homeRepository.Add(home);

        // Assert
        action.Should().Throw<ArgumentException>().WithMessage("Home already exists");
    }

    #endregion

    #endregion

    #region Get

    #region Success

    [TestMethod]
    public void Get_WhenHomeExists_ReturnsHome()
    {
        // Act
        Home result = _homeRepository.Get(_home.Id);

        // Assert
        result.Should().BeEquivalentTo(_home);
    }

    #endregion

    #region Error

    [TestMethod]
    public void Get_WhenHomeDoesNotExist_ThrowsException()
    {
        // Act
        Func<Home> action = () => _homeRepository.Get(Guid.NewGuid());

        // Assert
        action.Should().Throw<ArgumentException>().WithMessage("Home does not exist");
    }

    #endregion

    #endregion

    #region GetByAddress

    #region Success

    [TestMethod]
    public void GetByAddress_WhenHomeDoesNotExist_ReturnsNull()
    {
        // Act
        Home? result = _homeRepository.GetByAddress("Main St 456");

        // Assert
        result.Should().BeNull();
    }

    [TestMethod]
    public void GetByAddress_WhenHomeExists_ReturnsHome()
    {
        // Act
        Home? result = _homeRepository.GetByAddress("Main St 123");

        // Assert
        result.Should().BeEquivalentTo(_home);
    }

    #endregion

    #endregion

    #region Rename

    [TestMethod]
    public void Rename_RenamesHome()
    {
        // Arrange
        var homeId = _home.Id;
        var newName = "New Home Name";

        // Act
        _homeRepository.Rename(_home, newName);

        // Assert
        var updatedHome = _context.Homes.Find(homeId);
        Assert.IsNotNull(updatedHome);
        Assert.AreEqual(newName, updatedHome.NickName);
    }

    #endregion

    #region GetHomesByUserId

    #region Success

    [TestMethod]
    public void GetHomesByUserId_WhenUserOwnsHomes_ReturnsHomes()
    {
        // Arrange
        var home2 = new Home(_homeOwner, "Main St 456", 12.5, 12.5, 5);
        _context.Homes.Add(home2);
        _context.SaveChanges();

        // Act
        var result = _homeRepository.GetHomesByUserId(_homeOwner.Id);

        // Assert
        result.Should().Contain(_home).And.Contain(home2);
    }

    [TestMethod]
    public void GetHomesByUserId_WhenUserIsMember_ReturnsHomes()
    {
        // Act
        var result = _homeRepository.GetHomesByUserId(_otherOwner.Id);

        // Assert
        result.Should().Contain(_home);
    }

    #endregion

    #endregion

    #region UpdateHome

    [TestMethod]
    public void UpdateHome_WhenRoomsAreUpdated_UpdatesRoomsList()
    {
        // Arrange
        var home = new Home(_homeOwner, "Main St 123", 12.5, 12.5, 5);
        var room = new Room { Id = Guid.NewGuid(), Name = "Living Room", Home = home };

        _context.Homes.Add(home);
        _context.Rooms.Add(room);
        _context.SaveChanges();

        // Act
        if (!home.Rooms.Any(r => r.Id == room.Id))
        {
            home.Rooms.Add(room);
        }

        _homeRepository.Update(home);

        // Assert
        var updatedHome = _context.Homes.Include(h => h.Rooms).FirstOrDefault(h => h.Id == home.Id);
        updatedHome.Should().NotBeNull();
        updatedHome.Rooms.Should().ContainSingle(r => r.Id == room.Id && r.Name == "Living Room");
    }

    #endregion
}
