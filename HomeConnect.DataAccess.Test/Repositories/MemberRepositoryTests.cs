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
    public void Add_WithValidArguments_ShouldAddMember()
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
}
