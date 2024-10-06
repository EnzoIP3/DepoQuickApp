using BusinessLogic.HomeOwners.Entities;
using FluentAssertions;

namespace HomeConnect.BusinessLogic.Test.HomeOwners.Entities;

[TestClass]
public class HomeTest
{
    #region Constructor

    #region Success

    [TestMethod]
    public void Constructor_WhenArgumentsAreValid_CreatesInstance()
    {
        // Arrange
        var owner = new global::BusinessLogic.Users.Entities.User();
        const string address = "Main St 123";
        const double latitude = 50.456;
        const double longitude = 100.789;
        const int maxMembers = 5;

        // Act
        var home = new Home(owner, address, latitude, longitude, maxMembers);

        // Assert
        home.Should().NotBeNull();
    }

    #endregion

    #region Error

    [TestMethod]
    [DataRow("Main St")]
    [DataRow("123")]
    [DataRow("123 Main St")]
    public void Constructor_WhenAddressIsNotRoadAndNumber_ThrowsArgumentException(string address)
    {
        // Arrange
        var owner = new global::BusinessLogic.Users.Entities.User();
        const double latitude = 123.456;
        const double longitude = 456.789;
        const int maxMembers = 5;

        // Act
        var act = () => new Home(owner, address, latitude, longitude, maxMembers);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    [DataRow(-91)]
    [DataRow(91)]
    public void Constructor_WhenLatitudeIsInvalid_ThrowsArgumentException(double latitude)
    {
        // Arrange
        var owner = new global::BusinessLogic.Users.Entities.User();
        const string address = "Main St 123";
        const double longitude = 100.789;
        const int maxMembers = 5;

        // Act
        var act = () => new Home(owner, address, latitude, longitude, maxMembers);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    [DataRow(-181)]
    [DataRow(181)]
    public void Constructor_WhenLongitudeIsInvalid_ThrowsArgumentException(double longitude)
    {
        // Arrange
        var owner = new global::BusinessLogic.Users.Entities.User();
        const string address = "Main St 123";
        const double latitude = 50;
        const int maxMembers = 5;

        // Act
        var act = () => new Home(owner, address, latitude, longitude, maxMembers);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void Constructor_WhenLatitudeIsNull_ThrowsArgumentException()
    {
        // Arrange
        var owner = new global::BusinessLogic.Users.Entities.User();
        const string address = "Main St 123";
        const double longitude = 100.789;
        const int maxMembers = 5;

        // Act
        var act = () => new Home(owner, address, null, longitude, maxMembers);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void Constructor_WhenLongitudeIsNull_ThrowsArgumentException()
    {
        // Arrange
        var owner = new global::BusinessLogic.Users.Entities.User();
        const string address = "Main St 123";
        const double latitude = 50.456;
        const int maxMembers = 5;

        // Act
        var act = () => new Home(owner, address, latitude, null, maxMembers);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void Constructor_WhenMaxMembersIsNull_ThrowsArgumentException()
    {
        // Arrange
        var owner = new global::BusinessLogic.Users.Entities.User();
        const string address = "Main St 123";
        const double latitude = 50.456;
        const double longitude = 100.789;

        // Act
        var act = () => new Home(owner, address, latitude, longitude, null);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void Constructor_WhenMaxMembersIsLessThanOne_ThrowsArgumentException()
    {
        // Arrange
        var owner = new global::BusinessLogic.Users.Entities.User();
        const string address = "Main St 123";
        const double latitude = 50.456;
        const double longitude = 100.789;
        const int maxMembers = 0;

        // Act
        var act = () => new Home(owner, address, latitude, longitude, maxMembers);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    #endregion

    #endregion

    #region AddMember

    #region Success

    [TestMethod]
    public void AddMember_WhenMemberIsNotOwner_AddsMember()
    {
        // Arrange
        var owner = new global::BusinessLogic.Users.Entities.User();
        var home = new Home(owner, "Main St 123", 85.2, 100.789, 5);
        var member = new Member(new global::BusinessLogic.Users.Entities.User());

        // Act
        home.AddMember(member);

        // Assert
        home.Members.Should().ContainSingle(m => m == member);
    }

    #endregion

    #region Error

    [TestMethod]
    public void AddMember_WhenMemberIsOwner_ThrowsArgumentException()
    {
        // Arrange
        var owner = new global::BusinessLogic.Users.Entities.User();
        var ownerMember = new Member(owner);
        var home = new Home(owner, "Main St 123", 50.456, 100.789, 5);

        // Act
        var act = () => home.AddMember(ownerMember);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void AddMember_WhenMemberIsAlreadyMember_ThrowsArgumentException()
    {
        // Arrange
        var owner = new global::BusinessLogic.Users.Entities.User();
        var home = new Home(owner, "Main St 123", 50.456, 100.789, 5);
        var otherUser = new global::BusinessLogic.Users.Entities.User();
        var member = new Member(otherUser);
        home.AddMember(member);

        // Act
        var act = () => home.AddMember(member);

        // Assert
        act.Should().Throw<InvalidOperationException>();
    }

    [TestMethod]
    public void AddMember_WhenMaxMembersReached_ThrowsInvalidOperationException()
    {
        // Arrange
        var owner = new global::BusinessLogic.Users.Entities.User();
        var home = new Home(owner, "Main St 123", 50, 100.789, 1);
        var member = new Member(new global::BusinessLogic.Users.Entities.User());
        home.AddMember(member);

        // Act
        var act = () => home.AddMember(new Member(new global::BusinessLogic.Users.Entities.User()));

        // Assert
        act.Should().Throw<InvalidOperationException>();
    }

    #endregion

    #endregion
}
