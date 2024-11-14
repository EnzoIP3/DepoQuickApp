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

    #region GetMemberById

    #region Success

    [TestMethod]
    public void GetMemberById_WhenMemberExists_ReturnsMember()
    {
        // Act
        Member result = _homeRepository.GetMemberById(_member.Id);

        // Assert
        result.Should().BeEquivalentTo(_member);
    }

    #endregion

    #endregion

    #region UpdateMember

    #region Success

    [TestMethod]
    public void UpdateMember_WhenMemberExists_UpdatesMember()
    {
        // Arrange
        Member member = _home.Members.First();
        member.HomePermissions = [new HomePermission("ExamplePermission")];

        // Act
        _homeRepository.UpdateMember(member);

        // Assert
        _context.Homes.Should().Contain(h =>
            h.Members.Any(m => m.Id == member.Id && m.HomePermissions.Any(hp => hp.Value == "ExamplePermission")));
    }

    #endregion

    #endregion

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

    #region ExistsMember

    [TestMethod]
    public void ExistsMember_WhenMemberExists_ReturnsTrue()
    {
        // Act
        var result = _homeRepository.ExistsMember(_member.Id);

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

    #region AddRoom
    #region Success

    [TestMethod]
    public void AddRoom_WhenRoomIsValid_AddsRoom()
    {
        // Arrange
        var room = new Room { Id = Guid.NewGuid(), Name = "Living Room", Home = _home };

        // Act
        _homeRepository.AddRoom(room);

        // Assert
        _context.Rooms.Should().Contain(room);
    }
    #endregion

    #region ExistsRoom

    [TestMethod]
    public void ExistsRoom_WhenRoomExists_ReturnsTrue()
    {
        // Act
        var result = _homeRepository.ExistsRoom(_room.Id);

        // Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void ExistsRoom_WhenRoomDoesNotExist_ReturnsFalse()
    {
        // Arrange
        var roomId = Guid.NewGuid();

        // Act
        var result = _homeRepository.ExistsRoom(roomId);

        // Assert
        result.Should().BeFalse();
    }

    #endregion
}
