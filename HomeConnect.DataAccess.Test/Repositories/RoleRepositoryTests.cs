using BusinessLogic.Roles.Entities;
using FluentAssertions;
using HomeConnect.DataAccess.Repositories;

namespace HomeConnect.DataAccess.Test.Repositories;

[TestClass]
public class RoleRepositoryTests
{
    private readonly Context _context = DbContextBuilder.BuildTestDbContext();
    private readonly Role _role = new() { Name = "Role" };
    private RoleRepository _roleRepository = null!;

    [TestInitialize]
    public void Initialize()
    {
        _context.Database.EnsureCreated();
        _roleRepository = new RoleRepository(_context);
        _context.Roles.Add(_role);
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
    public void Exists_WhenRoleExists_ReturnsTrue()
    {
        // Act
        var result = _roleRepository.Exists("Role");

        // Assert
        result.Should().BeTrue();
    }

    #endregion

    #endregion

    #region GetRole

    #region Success

    [TestMethod]
    public void GetRole_WhenRoleExists_ReturnsRole()
    {
        // Act
        Role result = _roleRepository.Get("Role");

        // Assert
        result.Name.Should().Be("Role");
    }

    #endregion

    #endregion
}
