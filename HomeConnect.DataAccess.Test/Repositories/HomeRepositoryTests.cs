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
    private User _otherOwner = null!;
    private Member _member = null!;
    private Home _home = null!;

    [TestInitialize]
    public void Initialize()
    {
        _context.Database.EnsureCreated();

        var homeOwnerRole = new Role { Name = "HomeOwner", Permissions = new List<SystemPermission>() };
        _homeOwner = new User("John", "Doe", "email@email.com", "Password#100", homeOwnerRole);
        _otherOwner = new User("Jane", "Doe", "email2@email.com", "Password#100", homeOwnerRole);
        _home = new Home(_homeOwner, "Main St 123", 12.5, 12.5, 5);
        _member = new Member(_otherOwner);
        _home.AddMember(_member);
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
        var action = () => _homeRepository.Add(home);

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
        var result = _homeRepository.Get(_home.Id);

        // Assert
        result.Should().BeEquivalentTo(_home);
    }
    #endregion
    #region Error
    [TestMethod]
    public void Get_WhenHomeDoesNotExist_ShouldThrowException()
    {
        // Act
        var action = () => _homeRepository.Get(Guid.NewGuid());

        // Assert
        action.Should().Throw<ArgumentException>().WithMessage("Home does not exist");
    }
    #endregion
    #endregion

    #region GetMemberById
    #region Success
    [TestMethod]
    public void GetMemberById_WhenMemberExists_ReturnsMember()
    {
        // Act
        var result = _homeRepository.GetMemberById(_member.Id);

        // Assert
        result.Should().BeEquivalentTo(_member);
    }
    #endregion
    #region Error
    [TestMethod]
    public void GetMemberById_WhenMemberDoesNotExist_ShouldThrowException()
    {
        // Act
        var action = () => _homeRepository.GetMemberById(Guid.NewGuid());

        // Assert
        action.Should().Throw<ArgumentException>().WithMessage("Member does not exist");
    }
    #endregion
    #endregion

    #region UpdateMember
    #region Error
    [TestMethod]
    public void UpdateMember_WhenMemberDoesNotExist_ShouldThrowException()
    {
        // Arrange
        var member = new Member(_homeOwner);

        // Act
        var action = () => _homeRepository.UpdateMember(member);

        // Assert
        action.Should().Throw<ArgumentException>().WithMessage("Member does not exist");
    }
    #endregion
    #endregion
}
