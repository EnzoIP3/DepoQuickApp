using BusinessLogic;
using FluentAssertions;
using Moq;

namespace HomeConnect.BusinessLogic.Test;

[TestClass]
public class HomeOwnerServiceTests
{
    private Mock<IHomeRepository> _homeRepositoryMock = null!;
    private Mock<IUserRepository> _userRepositoryMock = null!;
    private Mock<IOwnedDeviceRepository> _ownedDeviceRepositoryMock = null!;
    private Mock<IDeviceRepository> _deviceRepositoryMock = null!;
    private HomeOwnerService _homeOwnerService = null!;
    private readonly User _user = new User("John", "Doe", "test@example.com", "12345678@My", new Role());

    [TestInitialize]
    public void Initialize()
    {
        _homeRepositoryMock = new Mock<IHomeRepository>(MockBehavior.Strict);
        _userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
        _ownedDeviceRepositoryMock = new Mock<IOwnedDeviceRepository>(MockBehavior.Strict);
        _deviceRepositoryMock = new Mock<IDeviceRepository>(MockBehavior.Strict);
        _homeOwnerService = new HomeOwnerService(_homeRepositoryMock.Object, _userRepositoryMock.Object,
            _deviceRepositoryMock.Object, _ownedDeviceRepositoryMock.Object);
    }

    #region CreateHome

    #region Success

    [TestMethod]
    public void CreateHome_WhenArgumentsAreValid_AddsHome()
    {
        // Arrange
        var model = new CreateHomeModel
        {
            HomeOwnerId = _user.Id.ToString(),
            Address = "Main St 123",
            Latitude = 1.0,
            Longitude = 2.0,
            MaxMembers = 5
        };
        _userRepositoryMock.Setup(x => x.Get(Guid.Parse(model.HomeOwnerId))).Returns(_user);
        _homeRepositoryMock.Setup(x => x.Add(It.IsAny<Home>())).Verifiable();

        // Act
        _homeOwnerService.CreateHome(model);

        // Assert
        _homeRepositoryMock.Verify(x => x.Add(It.IsAny<Home>()), Times.Once);
    }

    #endregion

    #region Error

    [TestMethod]
    [DataRow("", "Main St 123")]
    [DataRow("a99feb27-7dac-41ec-8fd2-942533868689", "")]
    public void CreateHome_WhenArgumentsHaveEmptyFields_ThrowsException(string homeOwnerId, string address)
    {
        // Arrange
        var model = new CreateHomeModel
        {
            HomeOwnerId = homeOwnerId,
            Address = address,
            Latitude = 1.0,
            Longitude = 2.0,
            MaxMembers = 5
        };

        // Act
        var act = () => _homeOwnerService.CreateHome(model);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    #endregion

    #endregion

    #region AddMemberToHome

    #region Success

    [TestMethod]
    public void AddMemberToHome_WhenArgumentsAreValid_AddsMember()
    {
        // Arrange
        var invitedUser = new User("Jane", "Doe", "jane@doe.com", "12345678@My", new Role());
        var home = new Home(_user, "Main St 123", 1.0, 2.0, 5);
        var model = new AddMemberModel
        {
            HomeId = home.Id.ToString(),
            HomeOwnerId = invitedUser.Id.ToString(),
            CanAddDevices = true,
            CanListDevices = true
        };
        _userRepositoryMock.Setup(x => x.Get(Guid.Parse(model.HomeOwnerId))).Returns(invitedUser);
        _homeRepositoryMock.Setup(x => x.Get(Guid.Parse(model.HomeId))).Returns(home);

        // Act
        _homeOwnerService.AddMemberToHome(model);

        // Assert
        home.Members.Should().ContainSingle(x => x.User == invitedUser);
    }

    #endregion

    #region Error

    [TestMethod]
    [DataRow("", "a99feb27-7dac-41ec-8fd2-942533868689")]
    [DataRow("12345678-1234-1234-1234-123456789012", "")]
    public void AddMemberToHome_WhenArgumentsHaveEmptyFields_ThrowsException(string homeId, string homeOwnerId)
    {
        // Arrange
        var model = new AddMemberModel
        {
            HomeId = homeId,
            HomeOwnerId = homeOwnerId,
            CanAddDevices = true,
            CanListDevices = true
        };

        // Act
        var act = () => _homeOwnerService.AddMemberToHome(model);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void AddMemberToHome_WhenHomeIdIsNotAGuid_ThrowsException()
    {
        // Arrange
        var model = new AddMemberModel
        {
            HomeId = "invalid-guid",
            HomeOwnerId = "a99feb27-7dac-41ec-8fd2-942533868689",
            CanAddDevices = true,
            CanListDevices = true
        };

        // Act
        var act = () => _homeOwnerService.AddMemberToHome(model);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    #endregion

    #endregion

    #region AddDevicesToHome

    #region Success

    [TestMethod]
    public void AddDevicesToHome_WhenArgumentsAreValid_AddsDevice()
    {
        // Arrange
        var home = new Home(_user, "Main St 123", 1.0, 2.0, 5);
        var device = new Device("Sensor", 1, "A sensor", "https://example.com/image.png", [], "Sensor");
        var camera = new Camera("Camera", 2, "A camera", "https://example.com/image.png", [], true, true, true, true);
        var addDeviceModel = new AddDeviceModel
        {
            HomeId = home.Id.ToString(),
            DeviceIds = [device.Id.ToString(), camera.Id.ToString()]
        };
        _deviceRepositoryMock.Setup(x => x.Get(device.Id)).Returns(device);
        _deviceRepositoryMock.Setup(x => x.Get(camera.Id)).Returns(camera);
        _homeRepositoryMock.Setup(x => x.Get(home.Id)).Returns(home);
        _ownedDeviceRepositoryMock.Setup(x => x.Add(It.IsAny<OwnedDevice>())).Verifiable();

        // Act
        _homeOwnerService.AddDeviceToHome(addDeviceModel);

        // Assert
        _ownedDeviceRepositoryMock.Verify(x => x.Add(It.IsAny<OwnedDevice>()), Times.Exactly(2));
    }

    #endregion

    #region Error

    [TestMethod]
    public void AddDevicesToHome_WhenHomeIdIsNotAGuid_ThrowsException()
    {
        // Arrange
        var addDeviceModel = new AddDeviceModel { HomeId = "invalid-guid", DeviceIds = ["1", "2"] };

        // Act
        var act = () => _homeOwnerService.AddDeviceToHome(addDeviceModel);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void AddDevicesToHome_WhenDeviceIdIsNotAGuid_ThrowsException()
    {
        // Arrange
        var home = new Home(_user, "Main St 123", 1.0, 2.0, 5);
        var addDeviceModel = new AddDeviceModel { HomeId = home.Id.ToString(), DeviceIds = ["invalid-guid"] };

        // Act
        var act = () => _homeOwnerService.AddDeviceToHome(addDeviceModel);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    #endregion

    #endregion

    #region GetHomeMembers

    #region Success

    [TestMethod]
    public void GetHomeMembers_WhenArgumentsAreValid_ReturnsMembers()
    {
        // Arrange
        var home = new Home(_user, "Main St 123", 1.0, 2.0, 5);
        var member = new Member(new User("Jane", "Doe", "test@example.com", "12345678@My", new Role()));
        home.AddMember(member);
        _homeRepositoryMock.Setup(x => x.Get(home.Id)).Returns(home);

        // Act
        var result = _homeOwnerService.GetHomeMembers(home.Id.ToString());

        // Assert
        result.Should().ContainSingle(x => x.User == member.User);
    }

    #endregion

    #region Error

    [TestMethod]
    public void GetHomeMembers_WhenHomeIdIsNotAGuid_ThrowsException()
    {
        // Arrange
        var homeId = "invalid-guid";

        // Act
        var act = () => _homeOwnerService.GetHomeMembers(homeId);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    #endregion

    #endregion

    #region GetHomeDevices

    #region Success

    [TestMethod]
    public void GetHomeDevices_WhenArgumentsAreValid_ReturnsDevices()
    {
        // Arrange
        var home = new Home(_user, "Main St 123", 1.0, 2.0, 5);
        var sensor = new Device("Sensor", 1, "A sensor", "https://example.com/image.png", [], "Sensor");
        var camera = new Camera("Camera", 2, "A camera", "https://example.com/image.png", [], true, true, true, true);
        var ownedDevices =
            new List<OwnedDevice>() { new OwnedDevice(home, sensor), new OwnedDevice(home, camera) };
        _homeRepositoryMock.Setup(x => x.Get(home.Id)).Returns(home);
        _ownedDeviceRepositoryMock.Setup(x => x.GetOwnedDevicesByHome(home)).Returns(ownedDevices);

        // Act
        var result = _homeOwnerService.GetHomeDevices(home.Id.ToString());

        // Assert
        result.Should().BeEquivalentTo(ownedDevices);
    }

    #endregion

    #region Error

    [TestMethod]
    public void GetHomeDevices_WhenHomeIdIsNotAGuid_ThrowsException()
    {
        // Arrange
        var homeId = "invalid-guid";

        // Act
        var act = () => _homeOwnerService.GetHomeDevices(homeId);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    #endregion

    #endregion
}
