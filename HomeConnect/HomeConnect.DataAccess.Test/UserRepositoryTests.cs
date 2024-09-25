using BusinessLogic;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace HomeConnect.DataAccess.Test;

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
        var adminRole = _context.Set<Role>().First(r => r.Name == "Admin");
        _userRepository = new UserRepository(_context);
        _validUser = new User("John", "Doe", "john.doe@example.com", "Password#100", adminRole);
        _otherUser = new User("Jane", "Doe", "jane.doe@example.com", "Password#200", adminRole);
        _context.Users.Add(_validUser);
        _context.Users.Add(_otherUser);
        _context.SaveChanges();
    }

    [TestCleanup]
    public void Cleanup()
    {
        _context.Database.EnsureDeleted();
    }

    [TestMethod]
    public void Exists_WhenUserExists_ReturnsTrue()
    {
        // Act
        var result = _userRepository.Exists("john.doe@example.com");

        // Assert
        result.Should().BeTrue();
    }
}
