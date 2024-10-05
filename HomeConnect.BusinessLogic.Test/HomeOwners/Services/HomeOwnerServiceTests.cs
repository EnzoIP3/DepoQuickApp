using BusinessLogic.BusinessOwners.Entities;
using BusinessLogic.Devices.Entities;
using BusinessLogic.Devices.Repositories;
using BusinessLogic.HomeOwners.Entities;
using BusinessLogic.HomeOwners.Models;
using BusinessLogic.HomeOwners.Repositories;
using BusinessLogic.HomeOwners.Services;
using BusinessLogic.Roles.Entities;
using BusinessLogic.Users.Entities;
using BusinessLogic.Users.Repositories;
using FluentAssertions;
using Moq;

namespace HomeConnect.BusinessLogic.Test.HomeOwners.Services;

[TestClass]
public class HomeOwnerServiceTests
{
    private Mock<IHomeRepository> _homeRepositoryMock = null!;
    private Mock<IUserRepository> _userRepositoryMock = null!;
    private Mock<IOwnedDeviceRepository> _ownedDeviceRepositoryMock = null!;
    private Mock<IDeviceRepository> _deviceRepositoryMock = null!;
    private HomeOwnerService _homeOwnerService = null!;

    private readonly global::BusinessLogic.Users.Entities.User _user =
        new global::BusinessLogic.Users.Entities.User("John", "Doe", "test@example.com", "12345678@My",
            new global::BusinessLogic.Roles.Entities.Role());

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
        var model = new CreateHomeArgs
        {
            HomeOwnerId = _user.Id.ToString(),
            Address = "Main St 123",
            Latitude = 1.0,
            Longitude = 2.0,
            MaxMembers = 5
        };
        var home = new Home(_user, model.Address, model.Latitude, model.Longitude, model.MaxMembers);
        _userRepositoryMock.Setup(x => x.Exists(Guid.Parse(model.HomeOwnerId))).Returns(true);
        _userRepositoryMock.Setup(x => x.Get(Guid.Parse(model.HomeOwnerId))).Returns(_user);
        _homeRepositoryMock.Setup(x => x.Add(It.Is<Home>(x =>
            x.Address == model.Address && x.Latitude == model.Latitude && x.Longitude == model.Longitude &&
            x.MaxMembers == model.MaxMembers && x.Owner == _user))).Callback<Home>(x => x.Id = Guid.NewGuid());
        _homeRepositoryMock.Setup(x => x.GetByAddress(model.Address)).Returns((Home)null);

        // Act
        var result = _homeOwnerService.CreateHome(model);

        // Assert
        _homeRepositoryMock.Verify(x => x.Add(It.IsAny<Home>()), Times.Once);
        result.Should().NotBeEmpty();
    }

    #endregion

    #region Error

    [TestMethod]
    [DataRow("", "Main St 123")]
    [DataRow("a99feb27-7dac-41ec-8fd2-942533868689", "")]
    public void CreateHome_WhenArgumentsHaveEmptyFields_ThrowsException(string homeOwnerId, string address)
    {
        // Arrange
        var model = new CreateHomeArgs
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

    [TestMethod]
    public void CreateHome_WhenHomeOwnerDoesNotExist_ThrowsException()
    {
        // Arrange
        var model = new CreateHomeArgs
        {
            HomeOwnerId = "a99feb27-7dac-41ec-8fd2-942533868689",
            Address = "Main St 123",
            Latitude = 1.0,
            Longitude = 2.0,
            MaxMembers = 5
        };
        _userRepositoryMock.Setup(x => x.Exists(Guid.Parse(model.HomeOwnerId)))
            .Returns(false);
        _homeRepositoryMock.Setup(x => x.GetByAddress(model.Address)).Returns((Home)null);

        // Act
        var act = () => _homeOwnerService.CreateHome(model);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void CreateHome_WhenAlreadyExistsHomeInAddress_ThrowsException()
    {
        // Arrange
        var model = new CreateHomeArgs
        {
            HomeOwnerId = _user.Id.ToString(),
            Address = "Main St 123",
            Latitude = 1.0,
            Longitude = 2.0,
            MaxMembers = 5
        };
        var home = new Home(_user, model.Address, model.Latitude, model.Longitude, model.MaxMembers);
        _userRepositoryMock.Setup(x => x.Get(Guid.Parse(model.HomeOwnerId))).Returns(_user);
        _homeRepositoryMock.Setup(x => x.GetByAddress(model.Address)).Returns(home);

        // Act
        var act = () => _homeOwnerService.CreateHome(model);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("Address is already in use");
    }

    #endregion

    #endregion

    #region AddMemberToHome

    #region Success

    [TestMethod]
    public void AddMemberToHome_WhenArgumentsAreValid_AddsMember()
    {
        // Arrange
        var invitedUser = new global::BusinessLogic.Users.Entities.User("Jane", "Doe", "jane@doe.com", "12345678@My",
            new global::BusinessLogic.Roles.Entities.Role());
        var home = new Home(_user, "Main St 123", 1.0, 2.0, 5);
        var model = new AddMemberArgs
        {
            HomeId = home.Id.ToString(),
            HomeOwnerId = invitedUser.Id.ToString(),
            CanAddDevices = true,
            CanListDevices = true
        };
        _userRepositoryMock.Setup(x => x.Exists(Guid.Parse(model.HomeOwnerId))).Returns(true);
        _userRepositoryMock.Setup(x => x.Get(Guid.Parse(model.HomeOwnerId))).Returns(invitedUser);
        _homeRepositoryMock.Setup(x => x.Get(Guid.Parse(model.HomeId))).Returns(home);

        // Act
        var result = _homeOwnerService.AddMemberToHome(model);

        // Assert
        home.Members.Should().ContainSingle(x => x.User == invitedUser);
        result.Should().Be(invitedUser.Id);
    }

    #endregion

    #region Error

    [TestMethod]
    [DataRow("", "a99feb27-7dac-41ec-8fd2-942533868689")]
    [DataRow("12345678-1234-1234-1234-123456789012", "")]
    public void AddMemberToHome_WhenArgumentsHaveEmptyFields_ThrowsException(string homeId, string homeOwnerId)
    {
        // Arrange
        var model = new AddMemberArgs()
        {
            HomeId = homeId, HomeOwnerId = homeOwnerId, CanAddDevices = true, CanListDevices = true
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
        var model = new AddMemberArgs()
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

    [TestMethod]
    public void AddMemberToHome_WhenHomeOwnerIdIsNotAGuid_ThrowsException()
    {
        // Arrange
        var model = new AddMemberArgs()
        {
            HomeId = "a99feb27-7dac-41ec-8fd2-942533868689",
            HomeOwnerId = "invalid-guid",
            CanAddDevices = true,
            CanListDevices = true
        };
        _homeRepositoryMock.Setup(x => x.Get(Guid.Parse(model.HomeId)))
            .Returns(new Home());

        // Act
        var act = () => _homeOwnerService.AddMemberToHome(model);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void AddMemberToHome_WhenHomeOwnerIdDoesNotExist_ThrowsException()
    {
        // Arrange
        var model = new AddMemberArgs()
        {
            HomeId = "a99feb27-7dac-41ec-8fd2-942533868689",
            HomeOwnerId = "a99feb27-7dac-41ec-8fd2-942533868689",
            CanAddDevices = true,
            CanListDevices = true
        };
        _homeRepositoryMock.Setup(x => x.Get(Guid.Parse(model.HomeId)))
            .Returns(new Home());
        _userRepositoryMock.Setup(x => x.Exists(Guid.Parse(model.HomeOwnerId)))
            .Returns(false);

        // Act
        var act = () => _homeOwnerService.AddMemberToHome(model);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void AddMemberToHome_WhenMemberIsAlreadyAdded_ThrowsException()
    {
        // Arrange
        var invitedUser = new User("name", "surname", "email1@email.com", "Password@100",
            new Role { Name = "HomeOwner", Permissions = [] });
        var home = new Home(_user, "Main St 123", 1.0, 2.0, 5);
        var member = new Member(invitedUser);
        home.AddMember(member);
        var args = new AddMemberArgs
        {
            HomeId = home.Id.ToString(),
            HomeOwnerId = invitedUser.Id.ToString(),
            CanAddDevices = true,
            CanListDevices = true
        };
        _homeRepositoryMock.Setup(x => x.Get(home.Id)).Returns(home);

        // Act
        var act = () => _homeOwnerService.AddMemberToHome(args);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("Member is already added to the home");
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
        var device = new Device("Sensor", 1, "A sensor",
            "https://example.com/image.png", [], "Sensor", new Business());
        var camera = new Camera("Camera", 2, "A camera", "https://example.com/image.png", [], new Business(), true,
            true, true, true);
        var addDeviceModel = new AddDevicesArgs
        {
            HomeId = home.Id.ToString(), DeviceIds = [device.Id.ToString(), camera.Id.ToString()]
        };
        _deviceRepositoryMock.Setup(x => x.Get(device.Id)).Returns(device);
        _deviceRepositoryMock.Setup(x => x.Get(camera.Id)).Returns(camera);
        _homeRepositoryMock.Setup(x => x.Get(home.Id)).Returns(home);
        _ownedDeviceRepositoryMock.Setup(x => x.Add(It.IsAny<OwnedDevice>())).Verifiable();
        _ownedDeviceRepositoryMock.Setup(x => x.GetOwnedDevicesByHome(home)).Returns(new List<OwnedDevice>());

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
        var addDeviceModel = new AddDevicesArgs { HomeId = "invalid-guid", DeviceIds = ["1", "2"] };

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
        var addDeviceModel = new AddDevicesArgs { HomeId = home.Id.ToString(), DeviceIds = ["invalid-guid"] };

        // Act
        var act = () => _homeOwnerService.AddDeviceToHome(addDeviceModel);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void AddDevicesToHome_WhenAtLeastOneDevicesIsAlreadyAdded_ThrowsException()
    {
        // Arrange
        var home = new Home(_user, "Main St 123", 1.0, 2.0, 5);
        var device = new Device("Sensor", 1, "A sensor", "https://example.com/image.png", [], "Sensor", new Business());
        Device camera = new Camera("Camera", 2, "A camera", "https://example.com/image.png", [], new Business(), true,
            true, true, true);
        var deviceIdList = new List<string> { device.Id.ToString(), camera.Id.ToString() };
        _homeRepositoryMock.Setup(x => x.Get(home.Id)).Returns(home);
        _ownedDeviceRepositoryMock.Setup(x => x.GetOwnedDevicesByHome(home))
            .Returns(new List<OwnedDevice> { new OwnedDevice(home, device) });

        // Act
        var act = () =>
            _homeOwnerService.AddDeviceToHome(new AddDevicesArgs
            {
                HomeId = home.Id.ToString(), DeviceIds = deviceIdList
            });

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage($"Devices with ids {device.Id.ToString()} are already added to the home");
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
        var member = new Member(new global::BusinessLogic.Users.Entities.User("Jane", "Doe", "test@example.com",
            "12345678@My", new global::BusinessLogic.Roles.Entities.Role()));
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
        var sensor = new Device("Sensor", 1, "A sensor",
            "https://example.com/image.png", [], "Sensor", new Business());
        var camera = new Camera("Camera", 2, "A camera", "https://example.com/image.png", [], new Business(), true,
            true, true, true);
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

    #region UpdateMemberNotifications

    #region error

    [TestMethod]
    public void UpdateMemberNotifications_WhenMemberDoesNotExist_ThrowsException()
    {
        // Arrange
        var nonExistentMemberId = Guid.NewGuid();
        _homeRepositoryMock.Setup(x => x.GetMemberById(nonExistentMemberId)).Returns((Member)null);

        // Act
        var act = () => _homeOwnerService.UpdateMemberNotifications(nonExistentMemberId, true);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("Member does not exist");
    }

    #endregion

    #region success

    [TestMethod]
    public void
        UpdateMemberNotifications_WhenMemberDoesNotHavePermissionAndRequestShouldBeNotifiedIsTrue_AddsPermission()
    {
        // Arrange
        var member = new Member(_user);
        var memberId = member.Id;
        _homeRepositoryMock.Setup(x => x.GetMemberById(memberId)).Returns(member);
        var permissionList = new List<HomePermission> { new HomePermission("shouldBeNotified") };

        _homeRepositoryMock.Setup(e =>
            e.UpdateMember(It.Is<Member>(x =>
                x.User == _user && x.HomePermissions.First().Value == "shouldBeNotified")));

        // Act
        _homeOwnerService.UpdateMemberNotifications(memberId, true);

        // Assert
        _homeRepositoryMock.VerifyAll();
        member.HomePermissions.Should().BeEquivalentTo(permissionList);
    }

    [TestMethod]
    public void UpdateMemberNotifications_WhenMemberHavePermissionAndRequestShouldBeNotifiedIsFalse_RemovesPermission()
    {
        // Arrange
        var member = new Member(_user, [new HomePermission("shouldBeNotified")]);
        var memberId = member.Id;
        _homeRepositoryMock.Setup(x => x.GetMemberById(memberId)).Returns(member);
        var permissionList = new List<HomePermission>();

        _homeRepositoryMock.Setup(e =>
            e.UpdateMember(It.Is<Member>(x =>
                x.User == _user && x.HomePermissions.Count == 0)));

        // Act
        _homeOwnerService.UpdateMemberNotifications(memberId, false);

        // Assert
        _homeRepositoryMock.VerifyAll();
        member.HomePermissions.Should().BeEquivalentTo(permissionList);
    }

    #endregion

    #endregion
}
