using BusinessLogic.HomeOwners.Entities;
using BusinessLogic.Users.Entities;
using FluentAssertions;
using HomeConnect.DataAccess.Repositories;

namespace HomeConnect.DataAccess.Test.Repositories;

[TestClass]
public class RoomRepositoryTests
{
    private readonly Context _context = DbContextBuilder.BuildTestDbContext();
    private Home _home = null!;
    private RoomRepository _roomRepository = null!;

    [TestInitialize]
    public void Initialize()
    {
        _context.Database.EnsureCreated();
        _roomRepository = new RoomRepository(_context);
        _home = new Home(new User(), "Address 123", 0, 0, 1);
    }

    [TestCleanup]
    public void Cleanup()
    {
        _context.Database.EnsureDeleted();
    }

    #region Add

    [TestMethod]
    public void Add_WhenArgumentsAreValid_AddsRoom()
    {
        // Arrange
        var room = new Room("Room", _home);

        // Act
        _roomRepository.Add(room);

        // Assert
        _context.Rooms.Should().Contain(room);
    }

    #endregion

    #region Get

    [TestMethod]
    public void Get_WhenRoomExists_ReturnsRoom()
    {
        // Arrange
        var room = new Room("Room", _home);
        _context.Rooms.Add(room);
        _context.SaveChanges();

        // Act
        Room result = _roomRepository.Get(room.Id);

        // Assert
        result.Should().Be(room);
    }

    #endregion

    #region Exists

    [TestMethod]
    public void Exists_WhenRoomExists_ReturnsTrue()
    {
        // Arrange
        var room = new Room("Room", _home);
        _context.Rooms.Add(room);
        _context.SaveChanges();

        // Act
        var result = _roomRepository.Exists(room.Id);

        // Assert
        result.Should().BeTrue();
    }

    #endregion

    #region Update

    [TestMethod]
    public void Update_WhenRoomExists_UpdatesRoom()
    {
        // Arrange
        var room = new Room("Room", _home);
        _context.Rooms.Add(room);
        _context.SaveChanges();
        room.Name = "Updated Room";

        // Act
        _roomRepository.Update(room);

        // Assert
        _context.Rooms.First(r => r.Id == room.Id).Name.Should().Be("Updated Room");
    }

    #endregion

    #region GetRooms

    [TestMethod]
    public void GetRoomsByHomeId_WhenCalled_ReturnsOnlyRoomsAssociatedWithHome()
    {
        // Arrange
        var homeId = Guid.NewGuid();
        var home = new Home(new User(), "Address 123", 0, 0, 1) { Id = homeId };
        var room1 = new Room("Room1", home);
        var room2 = new Room("Room2", new Home(new User(), "Amarales 3420", 0, 0, 1));
        _context.Homes.Add(home);
        _context.Rooms.AddRange(room1, room2);
        _context.SaveChanges();

        // Act
        List<Room> result = _roomRepository.GetRoomsByHomeId(homeId);

        // Assert
        result.Should().HaveCount(1);
        result.Should().Contain(r => r.Name == "Room1");
        result.Should().NotContain(r => r.Name == "Room2");
    }

    #endregion
}
