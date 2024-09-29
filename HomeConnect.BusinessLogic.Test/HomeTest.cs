using BusinessLogic;
using FluentAssertions;

namespace HomeConnect.BusinessLogic.Test;

[TestClass]
public class HomeTest
{
    [TestMethod]
    public void Constructor_WhenArgumentsAreValid_CreatesInstance()
    {
        // Arrange
        var owner = new User();
        const string address = "Main St 123";
        const double latitude = 123.456;
        const double longitude = 456.789;
        const int maxMembers = 5;

        // Act
        var home = new Home(owner, address, latitude, longitude, maxMembers);

        // Assert
        home.Should().NotBeNull();
    }

    [TestMethod]
    [DataRow("Main St")]
    [DataRow("123")]
    [DataRow("123 Main St")]
    public void Constructor_WhenAddressIsNotRoadAndNumber_ThrowsArgumentException(string address)
    {
        // Arrange
        var owner = new User();
        const double latitude = 123.456;
        const double longitude = 456.789;
        const int maxMembers = 5;

        // Act
        var act = () => new Home(owner, address, latitude, longitude, maxMembers);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void AddMember_WhenMemberIsNotOwner_AddsMember()
    {
        // Arrange
        var owner = new User();
        var home = new Home(owner, "Main St 123", 123.456, 456.789, 5);
        var member = new User();

        // Act
        home.AddMember(member);

        // Assert
        home.Members.Should().ContainSingle(m => m.User == member);
    }

    [TestMethod]
    public void AddMember_WhenMemberIsOwner_ThrowsArgumentException()
    {
        // Arrange
        var owner = new User();
        var home = new Home(owner, "Main St 123", 123.456, 456.789, 5);

        // Act
        var act = () => home.AddMember(owner);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void AddMember_WhenMemberIsAlreadyMember_ThrowsArgumentException()
    {
        // Arrange
        var owner = new User();
        var home = new Home(owner, "Main St 123", 123.456, 456.789, 5);
        var member = new User();
        home.AddMember(member);

        // Act
        var act = () => home.AddMember(member);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void AddMember_WhenMaxMembersReached_ThrowsInvalidOperationException()
    {
        // Arrange
        var owner = new User();
        var home = new Home(owner, "Main St 123", 123.456, 456.789, 1);
        var member = new User();
        home.AddMember(member);

        // Act
        var act = () => home.AddMember(new User());

        // Assert
        act.Should().Throw<InvalidOperationException>();
    }
}
