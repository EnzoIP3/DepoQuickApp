using BusinessLogic.HomeOwners.Entities;
using BusinessLogic.Users.Entities;
using FluentAssertions;
using HomeConnect.DataAccess.Repositories;

namespace HomeConnect.DataAccess.Test.Repositories;

[TestClass]
public class RoomRepositoryTests
{
    private readonly Context _context = DbContextBuilder.BuildTestDbContext();
    private RoomRepository _roomRepository = null!;
    private Home _home = null!;

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

    [TestMethod]
    public void Get_WhenRoomExists_ReturnsRoom()
    {
        // Arrange
        var room = new Room("Room", _home);
        _context.Rooms.Add(room);
        _context.SaveChanges();

        // Act
        var result = _roomRepository.Get(room.Id);

        // Assert
        result.Should().Be(room);
    }

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
}
