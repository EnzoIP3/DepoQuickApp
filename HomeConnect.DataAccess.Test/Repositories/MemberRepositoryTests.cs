using BusinessLogic.HomeOwners.Entities;
using BusinessLogic.Roles.Entities;
using BusinessLogic.Users.Entities;
using FluentAssertions;
using HomeConnect.DataAccess.Repositories;

namespace HomeConnect.DataAccess.Test.Repositories;

[TestClass]
public class MemberRepositoryTests
{
    private readonly Context _context = DbContextBuilder.BuildTestDbContext();
    private Member _member = null!;
    private MemberRepository _memberRepository = null!;

    [TestInitialize]
    public void Initialize()
    {
        _context.Database.EnsureCreated();
        _memberRepository = new MemberRepository(_context);
        var role = new Role("Role", []);
        var user = new User("Name", "Surname", "test@example.com", "Password1M@", role);
        var otherUser = new User("Name", "Surname", "test@example.com", "Password1M@", role);
        var home = new Home(user, "Address 123", 0, 0, 5);
        _member = new Member(otherUser) { Home = home };
        _memberRepository.Add(_member);
    }

    [TestCleanup]
    public void Cleanup()
    {
        _context.Database.EnsureDeleted();
    }

    #region Add

    [TestMethod]
    public void Add_WhenArgumentsAreValid_AddsMember()
    {
        // Arrange
        var role = new Role("Role 2", []);
        var user = new User("John", "Doe", "john@example.com", "Password1M@", role);
        var otherUser = new User("Name", "Surname", "test@example.com", "Password1M@", role);
        var home = new Home(user, "Address 123", 0, 0, 5);
        var member = new Member(otherUser) { Home = home };

        // Act
        _memberRepository.Add(member);

        // Assert
        _context.Members.Should().Contain(member);
    }

    #endregion

    #region Get

    [TestMethod]
    public void GetMemberById_WhenMemberExists_ReturnsMember()
    {
        // Act
        Member result = _memberRepository.Get(_member.Id);

        // Assert
        result.Should().BeEquivalentTo(_member);
    }

    #endregion

    #region Update

    [TestMethod]
    public void UpdateMember_WhenMemberExists_UpdatesMember()
    {
        // Arrange
        Member member = _memberRepository.Get(_member.Id);
        member.HomePermissions = [new HomePermission("ExamplePermission")];

        // Act
        _memberRepository.Update(member);

        // Assert
        Member updatedMember = _memberRepository.Get(member.Id);
        updatedMember.HomePermissions.Should().BeEquivalentTo(member.HomePermissions);
    }

    #endregion

    #region Exists

    #region ExistsMember

    [TestMethod]
    public void ExistsMember_WhenMemberExists_ReturnsTrue()
    {
        // Act
        var result = _memberRepository.Exists(_member.Id);

        // Assert
        result.Should().BeTrue();
    }

    #endregion

    #endregion
}
