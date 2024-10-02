using BusinessLogic.Roles.Entities;
using FluentAssertions;
using HomeConnect.DataAccess.Roles;

namespace HomeConnect.DataAccess.Test.Roles;

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
        var result = _roleRepository.GetRole("Role");

        // Assert
        result.Name.Should().Be("Role");
    }

    #endregion

    #region Error

    [TestMethod]
    public void GetRole_WhenRoleDoesNotExist_ShouldThrowException()
    {
        // Act
        Action action = () => _roleRepository.GetRole("Role2");

        // Assert
        action.Should().Throw<ArgumentException>();
    }

    #endregion

    #endregion
}
