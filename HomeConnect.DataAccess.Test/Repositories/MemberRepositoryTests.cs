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
    private MemberRepository _memberRepository = null!;

    [TestInitialize]
    public void Initialize()
    {
        _context.Database.EnsureCreated();
        _memberRepository = new MemberRepository(_context);
    }

    [TestCleanup]
    public void Cleanup()
    {
        _context.Database.EnsureDeleted();
    }

    [TestMethod]
    public void Add_WhenArgumentsAreValid_AddsMember()
    {
        // Arrange
        var role = new Role("Role", []);
        var user = new User("Name", "Surname", "test@example.com", "Password1M@", role);
        var otherUser = new User("Name", "Surname", "test@example.com", "Password1M@", role);
        var home = new Home(user, "Address 123", 0, 0, 5);
        var member = new Member(otherUser) { Home = home };

        // Act
        _memberRepository.Add(member);

        // Assert
        _context.Members.Should().Contain(member);
    }

    #region GetMembersByUserId
    [TestMethod]
    public void GetMembersByUserId_WhenCalled_ReturnsCorrectMembers()
    {
        // Arrange
        var role = new Role("Role", []);
        var user = new User("Name", "Surname", "test@example.com", "Password1M@", role);
        var home = new Home(user, "Address 123", 0, 0, 5);
        var member1 = new Member(user) { Home = home };
        var member2 = new Member(user) { Home = home };

        _context.Users.Add(user);
        _context.Homes.Add(home);
        _context.Members.AddRange(member1, member2);
        _context.SaveChanges();

        // Act
        List<Member> members = _memberRepository.GetMembersByUserId(user.Id);

        // Assert
        Assert.IsNotNull(members);
        Assert.AreEqual(2, members.Count);
        Assert.IsTrue(members.All(m => m.User.Id == user.Id));
    }
    #endregion
}
