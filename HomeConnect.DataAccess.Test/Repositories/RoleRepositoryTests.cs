using BusinessLogic.Roles.Entities;
using FluentAssertions;
using HomeConnect.DataAccess.Repositories;

namespace HomeConnect.DataAccess.Test.Repositories;

[TestClass]
public class RoleRepositoryTests
{
    private readonly Context _context = DbContextBuilder.BuildTestDbContext();
    private RoleRepository _roleRepository = null!;
    private readonly Role _role = new Role { Name = "Role" };

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

    #region GetRole

    #region Success

    [TestMethod]
    public void GetRole_WhenRoleExists_ShouldReturnRole()
    {
        // Act
        var result = _roleRepository.Get("Role");

        // Assert
        result.Name.Should().Be("Role");
    }

    #endregion

    #region Error

    [TestMethod]
    public void GetRole_WhenRoleDoesNotExist_ShouldThrowException()
    {
        // Act
        Action action = () => _roleRepository.Get("Role2");

        // Assert
        action.Should().Throw<ArgumentException>();
    }

    #endregion

    #endregion

    #region Exists

    #region Success

    [TestMethod]
    public void Exists_WhenRoleExists_ShouldReturnTrue()
    {
        // Act
        var result = _roleRepository.Exists("Role");

        // Assert
        result.Should().BeTrue();
    }

    #endregion

    #endregion
}
