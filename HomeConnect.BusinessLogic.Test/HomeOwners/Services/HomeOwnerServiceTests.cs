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
    private readonly User _user =
        new("John", "Doe", "test@example.com", "12345678@My",
            new Role());

    private readonly string _modelNumber = "123";

    private Mock<IDeviceRepository> _deviceRepositoryMock = null!;
    private HomeOwnerService _homeOwnerService = null!;
    private Mock<IHomeRepository> _homeRepositoryMock = null!;
    private Mock<IMemberRepository> _memberRepositoryMock = null!;
    private Mock<IOwnedDeviceRepository> _ownedDeviceRepositoryMock = null!;
    private Mock<IUserRepository> _userRepositoryMock = null!;
    private Mock<IRoomRepository> _roomRepositoryMock = null!;
    private List<Member> _testMembers = null!;

    [TestInitialize]
    public void Initialize()
    {
        _homeRepositoryMock = new Mock<IHomeRepository>(MockBehavior.Strict);
        _userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
        _ownedDeviceRepositoryMock = new Mock<IOwnedDeviceRepository>(MockBehavior.Strict);
        _deviceRepositoryMock = new Mock<IDeviceRepository>(MockBehavior.Strict);
        _memberRepositoryMock = new Mock<IMemberRepository>(MockBehavior.Strict);
        _roomRepositoryMock = new Mock<IRoomRepository>(MockBehavior.Strict);
        _homeOwnerService = new HomeOwnerService(_homeRepositoryMock.Object, _userRepositoryMock.Object,
            _deviceRepositoryMock.Object, _ownedDeviceRepositoryMock.Object, _memberRepositoryMock.Object,
            _roomRepositoryMock.Object);
        var home1 = new Home(_user, "Amarales 3420", 40.7128, -74.0060, 4);
        var home2 = new Home(_user, "Arteaga 1470", 34.0522, -118.2437, 6);
        _testMembers =
        [
            new Member(_user, []) { Home = home1 },
            new Member(_user, []) { Home = home2 }
        ];
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
        _userRepositoryMock.Setup(x => x.Exists(Guid.Parse(model.HomeOwnerId))).Returns(true);
        _userRepositoryMock.Setup(x => x.Get(Guid.Parse(model.HomeOwnerId))).Returns(_user);
        _userRepositoryMock.Setup(x => x.ExistsByEmail(_user.Email)).Returns(true);
        _homeRepositoryMock.Setup(x => x.Add(It.Is<Home>(x =>
            x.Address == model.Address && x.Latitude == model.Latitude && x.Longitude == model.Longitude &&
            x.MaxMembers == model.MaxMembers && x.Owner == _user))).Callback<Home>(x => x.Id = Guid.NewGuid());
        _homeRepositoryMock.Setup(x => x.GetByAddress(model.Address)).Returns((Home)null);

        // Act
        Guid result = _homeOwnerService.CreateHome(model);

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
        Func<Guid> act = () => _homeOwnerService.CreateHome(model);

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
        Func<Guid> act = () => _homeOwnerService.CreateHome(model);

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
        _userRepositoryMock.Setup(x => x.Exists(Guid.Parse(model.HomeOwnerId))).Returns(true);

        // Act
        Func<Guid> act = () => _homeOwnerService.CreateHome(model);

        // Assert
        act.Should().Throw<InvalidOperationException>().WithMessage("Address is already in use.");
    }

    #endregion

    #endregion

    #region AddMemberToHome

    #region Success

    [TestMethod]
    public void AddMemberToHome_WhenArgumentsAreValid_AddsMember()
    {
        // Arrange
        var invitedUser = new User("Jane", "Doe", "jane@doe.com", "12345678@My",
            new Role());
        var home = new Home(_user, "Main St 123", 1.0, 2.0, 5);
        var model = new AddMemberArgs
        {
            HomeId = home.Id.ToString(),
            UserEmail = invitedUser.Email,
            Permissions = [HomePermission.GetDevices, HomePermission.AddDevice, HomePermission.NameDevice]
        };
        _userRepositoryMock.Setup(x => x.ExistsByEmail(model.UserEmail)).Returns(true);
        _userRepositoryMock.Setup(x => x.GetByEmail(model.UserEmail)).Returns(invitedUser);
        _homeRepositoryMock.Setup(x => x.Exists(Guid.Parse(model.HomeId))).Returns(true);
        _homeRepositoryMock.Setup(x => x.Get(Guid.Parse(model.HomeId))).Returns(home);
        _memberRepositoryMock.Setup(x => x.Add(It.IsAny<Member>())).Verifiable();

        // Act
        Guid result = _homeOwnerService.AddMemberToHome(model);

        // Assert
        home.Members.Should().ContainSingle(x => x.User == invitedUser);
        result.Should().Be(home.Members.First().Id);
        home.Members.First().HomePermissions.Any(p => p.Value == HomePermission.GetDevices).Should().BeTrue();
        home.Members.First().HomePermissions.Any(p => p.Value == HomePermission.AddDevice).Should().BeTrue();
        home.Members.First().HomePermissions.Any(p => p.Value == HomePermission.NameDevice).Should().BeTrue();
        _memberRepositoryMock.Verify(x => x.Add(It.IsAny<Member>()), Times.Once);
    }

    #endregion

    #region Error

    [TestMethod]
    [DataRow("", "john.doe@gmail.com")]
    [DataRow("12345678-1234-1234-1234-123456789012", "")]
    public void AddMemberToHome_WhenArgumentsHaveEmptyFields_ThrowsException(string homeId, string homeOwnerEmail)
    {
        // Arrange
        var model = new AddMemberArgs
        {
            HomeId = homeId,
            UserEmail = homeOwnerEmail,
            Permissions = [SystemPermission.GetDevices, SystemPermission.AddDevice, SystemPermission.NameDevice]
        };

        // Act
        Func<Guid> act = () => _homeOwnerService.AddMemberToHome(model);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void AddMemberToHome_WhenHomeIdIsNotAGuid_ThrowsException()
    {
        // Arrange
        var model = new AddMemberArgs
        {
            HomeId = "invalid-guid",
            UserEmail = "a99feb27-7dac-41ec-8fd2-942533868689",
            Permissions = [SystemPermission.GetDevices, SystemPermission.AddDevice, SystemPermission.NameDevice]
        };

        // Act
        Func<Guid> act = () => _homeOwnerService.AddMemberToHome(model);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void AddMemberToHome_WhenHomeOwnerEmailDoesNotExist_ThrowsException()
    {
        // Arrange
        var model = new AddMemberArgs
        {
            HomeId = "a99feb27-7dac-41ec-8fd2-942533868689",
            UserEmail = "not@exists.com",
            Permissions = [SystemPermission.GetDevices]
        };
        _homeRepositoryMock.Setup(x => x.Exists(Guid.Parse(model.HomeId))).Returns(true);
        _homeRepositoryMock.Setup(x => x.Get(Guid.Parse(model.HomeId)))
            .Returns(new Home());
        _userRepositoryMock.Setup(x => x.ExistsByEmail(model.UserEmail))
            .Returns(false);

        // Act
        Func<Guid> act = () => _homeOwnerService.AddMemberToHome(model);

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
            UserEmail = invitedUser.Id.ToString(),
            Permissions = [SystemPermission.GetDevices]
        };
        _homeRepositoryMock.Setup(x => x.Exists(home.Id)).Returns(true);
        _homeRepositoryMock.Setup(x => x.Get(home.Id)).Returns(home);

        // Act
        Func<Guid> act = () => _homeOwnerService.AddMemberToHome(args);

        // Assert
        act.Should().Throw<InvalidOperationException>().WithMessage("The member is already added to the home.");
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
        var device = new Device("Sensor", "1", "A sensor",
            "https://example.com/image.png", [], "Sensor", new Business());
        var camera = new Camera("Camera", "2", "A camera", "https://example.com/image.png", [], new Business(), true,
            true, true, true);
        var addDeviceModel = new AddDevicesArgs
        {
            HomeId = home.Id.ToString(), DeviceIds = [device.Id.ToString(), camera.Id.ToString()]
        };
        _deviceRepositoryMock.Setup(x => x.Get(device.Id)).Returns(device);
        _deviceRepositoryMock.Setup(x => x.Get(camera.Id)).Returns(camera);
        _homeRepositoryMock.Setup(x => x.Exists(home.Id)).Returns(true);
        _homeRepositoryMock.Setup(x => x.Get(home.Id)).Returns(home);
        _ownedDeviceRepositoryMock.Setup(x => x.Add(It.IsAny<OwnedDevice>())).Verifiable();
        _ownedDeviceRepositoryMock.Setup(x => x.GetOwnedDevicesByHome(home)).Returns(new List<OwnedDevice>());

        // Act
        _homeOwnerService.AddDeviceToHome(addDeviceModel);

        // Assert
        _ownedDeviceRepositoryMock.Verify(x => x.Add(It.IsAny<OwnedDevice>()), Times.Exactly(2));
    }

    [TestMethod]
    public void AddDevicesToHome_WhenArgumentsAreValidAndDeviceTypeIsLamp_AddsDeviceWithStateFalse()
    {
        // Arrange
        var home = new Home(_user, "Main St 123", 1.0, 2.0, 5);
        var device = new Device("Lamp", _modelNumber, "A sensor",
            "https://example.com/image.png", [], "Lamp", new Business());
        var addDeviceModel = new AddDevicesArgs { HomeId = home.Id.ToString(), DeviceIds = [device.Id.ToString()] };
        _deviceRepositoryMock.Setup(x => x.Get(device.Id)).Returns(device);
        _homeRepositoryMock.Setup(x => x.Exists(home.Id)).Returns(true);
        _homeRepositoryMock.Setup(x => x.Get(home.Id)).Returns(home);
        _ownedDeviceRepositoryMock.Setup(x => x.Add(It.IsAny<LampOwnedDevice>())).Verifiable();
        _ownedDeviceRepositoryMock.Setup(x => x.GetOwnedDevicesByHome(home)).Returns(new List<OwnedDevice>());

        // Act
        _homeOwnerService.AddDeviceToHome(addDeviceModel);

        // Assert
        _ownedDeviceRepositoryMock.Verify(
            x => x.Add(It.Is<LampOwnedDevice>(lamp => lamp.State == false && lamp.Device == device)), Times.Once);
    }

    [TestMethod]
    public void AddDevicesToHome_WhenArgumentsAreValidAndDeviceTypeIsSensor_AddsDeviceWithIsOpenSetToFalse()
    {
        // Arrange
        var home = new Home(_user, "Main St 123", 1.0, 2.0, 5);
        var device = new Device("Sensor", _modelNumber, "A sensor",
            "https://example.com/image.png", [], "Sensor", new Business());
        var addDeviceModel = new AddDevicesArgs { HomeId = home.Id.ToString(), DeviceIds = [device.Id.ToString()] };
        _deviceRepositoryMock.Setup(x => x.Get(device.Id)).Returns(device);
        _homeRepositoryMock.Setup(x => x.Exists(home.Id)).Returns(true);
        _homeRepositoryMock.Setup(x => x.Get(home.Id)).Returns(home);
        _ownedDeviceRepositoryMock.Setup(x => x.Add(
            It.IsAny<SensorOwnedDevice>())).Verifiable();
        _ownedDeviceRepositoryMock.Setup(x => x.GetOwnedDevicesByHome(home)).Returns(new List<OwnedDevice>());

        // Act
        _homeOwnerService.AddDeviceToHome(addDeviceModel);

        // Assert
        _ownedDeviceRepositoryMock.Verify(x => x.Add(It.Is<SensorOwnedDevice>(
            sensor => sensor.IsOpen == false && sensor.Device == device)), Times.Once);
    }

    #endregion

    #region Error

    [TestMethod]
    public void AddDevicesToHome_WhenHomeIdIsNotAGuid_ThrowsException()
    {
        // Arrange
        var addDeviceModel = new AddDevicesArgs { HomeId = "invalid-guid", DeviceIds = ["1", "2"] };

        // Act
        Action act = () => _homeOwnerService.AddDeviceToHome(addDeviceModel);

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
        Action act = () => _homeOwnerService.AddDeviceToHome(addDeviceModel);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void AddDevicesToHome_WhenDevicesAreEmpty_ThrowsException()
    {
        // Arrange
        var home = new Home(_user, "Main St 123", 1.0, 2.0, 5);
        var addDeviceModel = new AddDevicesArgs { HomeId = home.Id.ToString(), DeviceIds = [] };
        _homeRepositoryMock.Setup(x => x.Exists(home.Id)).Returns(true);
        _homeRepositoryMock.Setup(x => x.Get(home.Id)).Returns(home);
        _ownedDeviceRepositoryMock.Setup(x => x.GetOwnedDevicesByHome(home)).Returns(new List<OwnedDevice>());

        // Act
        Action act = () => _homeOwnerService.AddDeviceToHome(addDeviceModel);

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
        var member = new Member(new User("Jane", "Doe", "test@example.com",
            "12345678@My", new Role()));
        home.AddMember(member);
        _homeRepositoryMock.Setup(x => x.Exists(home.Id)).Returns(true);
        _homeRepositoryMock.Setup(x => x.Get(home.Id)).Returns(home);

        // Act
        List<Member> result = _homeOwnerService.GetHomeMembers(home.Id.ToString());

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
        Func<List<Member>> act = () => _homeOwnerService.GetHomeMembers(homeId);

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
        var sensor = new Device("Sensor", "1", "A sensor",
            "https://example.com/image.png", [], "Sensor", new Business());
        var camera = new Camera("Camera", "2", "A camera", "https://example.com/image.png", [], new Business(), true,
            true, true, true);
        var ownedDevices =
            new List<OwnedDevice> { new(home, sensor), new(home, camera) };
        _homeRepositoryMock.Setup(x => x.Exists(home.Id)).Returns(true);
        _homeRepositoryMock.Setup(x => x.Get(home.Id)).Returns(home);
        _ownedDeviceRepositoryMock.Setup(x => x.GetOwnedDevicesByHome(home)).Returns(ownedDevices);

        // Act
        IEnumerable<OwnedDevice> result = _homeOwnerService.GetHomeDevices(home.Id.ToString());

        // Assert
        result.Should().BeEquivalentTo(ownedDevices);
    }

    [TestMethod]
    public void GetHomeDevices_WhenRoomIdIsProvided_ReturnsDevicesInThatRoom()
    {
        // Arrange
        var home = new Home(_user, "Main St 123", 1.0, 2.0, 5);
        var room = new Room { Id = Guid.NewGuid(), Name = "Living Room" };
        var sensor = new Device("Sensor", "1", "A sensor", "https://example.com/image.png", [], "Sensor",
            new Business());
        var camera = new Camera("Camera", "2", "A camera", "https://example.com/image.png", [], new Business(), true,
            true, true, true);
        var ownedDeviceInRoom = new OwnedDevice(home, sensor) { Room = room };
        var ownedDeviceNotInRoom = new OwnedDevice(home, camera);
        var ownedDevices = new List<OwnedDevice> { ownedDeviceInRoom, ownedDeviceNotInRoom };

        _homeRepositoryMock.Setup(x => x.Exists(home.Id)).Returns(true);
        _homeRepositoryMock.Setup(x => x.Get(home.Id)).Returns(home);
        _ownedDeviceRepositoryMock.Setup(x => x.GetOwnedDevicesByHome(home)).Returns(ownedDevices);

        // Act
        IEnumerable<OwnedDevice> result = _homeOwnerService.GetHomeDevices(home.Id.ToString(), room.Id.ToString());

        // Assert
        result.Should().ContainSingle(x => x.Room == room);
        result.Should().NotContain(x => x.Room == null);
    }

    #endregion

    #region Error

    [TestMethod]
    public void GetHomeDevices_WhenHomeIdIsNotAGuid_ThrowsException()
    {
        // Arrange
        var homeId = "invalid-guid";

        // Act
        Func<IEnumerable<OwnedDevice>> act = () => _homeOwnerService.GetHomeDevices(homeId);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    #endregion

    #endregion

    #region UpdateMemberNotifications

    #region Error

    [TestMethod]
    public void UpdateMemberNotifications_WhenMemberDoesNotExist_ThrowsException()
    {
        // Arrange
        var nonExistentMemberId = Guid.NewGuid();
        _memberRepositoryMock.Setup(x => x.Exists(nonExistentMemberId)).Returns(false);

        // Act
        Action act = () => _homeOwnerService.UpdateMemberNotifications(nonExistentMemberId, true);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("Member does not exist.");
    }

    [TestMethod]
    public void UpdateMemberNotifications_WhenShouldBeNotifiedIsNull_ThrowsException()
    {
        // Arrange
        var member = new Member(_user);
        Guid memberId = member.Id;
        _memberRepositoryMock.Setup(x => x.Exists(memberId)).Returns(true);
        _memberRepositoryMock.Setup(x => x.Get(memberId)).Returns(member);

        // Act
        Action act = () => _homeOwnerService.UpdateMemberNotifications(memberId, null);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("ShouldBeNotified must be provided.");
    }

    #endregion

    #region success

    [TestMethod]
    public void
        UpdateMemberNotifications_WhenMemberDoesNotHavePermissionAndRequestShouldBeNotifiedIsTrue_AddsPermission()
    {
        // Arrange
        var member = new Member(_user);
        Guid memberId = member.Id;
        _memberRepositoryMock.Setup(x => x.Exists(memberId)).Returns(true);
        _memberRepositoryMock.Setup(x => x.Get(memberId)).Returns(member);

        _memberRepositoryMock.Setup(e =>
            e.Update(It.Is<Member>(x =>
                x.User == _user && x.HomePermissions.Any(p => p.Value == HomePermission.GetNotifications))));

        // Act
        _homeOwnerService.UpdateMemberNotifications(memberId, true);

        // Assert
        _homeRepositoryMock.VerifyAll();
        member.HomePermissions.Any(p => p.Value == HomePermission.GetNotifications).Should().BeTrue();
    }

    [TestMethod]
    public void UpdateMemberNotifications_WhenMemberHavePermissionAndRequestShouldBeNotifiedIsFalse_RemovesPermission()
    {
        // Arrange
        var member = new Member(_user, [new HomePermission(HomePermission.GetNotifications)]);
        Guid memberId = member.Id;
        _memberRepositoryMock.Setup(x => x.Exists(memberId)).Returns(true);
        _memberRepositoryMock.Setup(x => x.Get(memberId)).Returns(member);
        var permissionList = new List<HomePermission>();

        _memberRepositoryMock.Setup(e =>
            e.Update(It.Is<Member>(x =>
                x.User == _user && x.HomePermissions.Count == 0)));

        // Act
        _homeOwnerService.UpdateMemberNotifications(memberId, false);

        // Assert
        _homeRepositoryMock.VerifyAll();
        member.HomePermissions.Should().BeEquivalentTo(permissionList);
    }

    #endregion

    #endregion

    #region GetHome

    #region error

    [TestMethod]
    public void GetHome_WhenHomeDoesNotExist_ThrowsException()
    {
        // Arrange
        var nonExistentHomeId = Guid.NewGuid();
        _homeRepositoryMock.Setup(x => x.Exists(nonExistentHomeId)).Returns(false);

        // Act
        Func<Home> act = () => _homeOwnerService.GetHome(nonExistentHomeId);

        // Assert
        act.Should().Throw<KeyNotFoundException>().WithMessage("Home does not exist.");
    }

    #endregion

    #region success

    [TestMethod]
    public void GetHome_WhenHomeExists_ReturnsHome()
    {
        // Arrange
        var home = new Home(_user, "Main St 123", 1.0, 2.0, 5);
        _homeRepositoryMock.Setup(x => x.Exists(home.Id)).Returns(true);
        _homeRepositoryMock.Setup(x => x.Get(home.Id)).Returns(home);

        // Act
        Home result = _homeOwnerService.GetHome(home.Id);

        // Assert
        result.Should().Be(home);
    }

    #endregion

    #endregion

    #region GetHomesByOwnerId

    [TestMethod]
    public void GetHomesByOwnerId_WhenCalled_ReturnsCorrectHomes()
    {
        // Arrange
        _homeRepositoryMock.Setup(repo => repo.GetHomesByUserId(_user.Id))
            .Returns(_testMembers.Select(m => m.Home).ToList());

        // Act
        List<Home> homes = _homeOwnerService.GetHomesByOwnerId(_user.Id);

        // Assert
        _homeRepositoryMock.Verify(repo => repo.GetHomesByUserId(_user.Id), Times.Once);
        homes.Should().BeEquivalentTo(_testMembers.Select(m => m.Home).ToList());
    }

    #endregion

    #region NameHome

    #region Success

    [TestMethod]
    public void NameHome_ShouldAssignNickName()
    {
        // Arrange
        var ownerId = Guid.NewGuid();
        var homeId = Guid.NewGuid();
        var newName = "New Home Name";
        var home = new Home { Id = homeId, NickName = "Old Name" };
        var args = new NameHomeArgs { OwnerId = ownerId, HomeId = homeId, NewName = newName };

        _homeRepositoryMock.Setup(repo => repo.Exists(homeId)).Returns(true);
        _homeRepositoryMock.Setup(repo =>
            repo.Get(homeId)).Returns(home);
        _homeRepositoryMock.Setup(repo => repo.Rename(It.IsAny<Home>(), It.IsAny<string>()))
            .Callback<Home, string>((h, n) => h.NickName = n)
            .Verifiable();

        // Act
        _homeOwnerService.NameHome(args);

        // Assert
        Assert.AreEqual(newName, home.NickName);
        _homeRepositoryMock.Verify(repo => repo.Rename(home, newName), Times.Once);
    }

    #endregion

    #region Error

    [TestMethod]
    public void NameHome_WhenOwnerIdIsEmpty_ThrowsException()
    {
        // Arrange
        var ownerId = Guid.Empty;
        var homeId = Guid.NewGuid();
        var newName = "New Home Name";
        var args = new NameHomeArgs { OwnerId = ownerId, HomeId = homeId, NewName = newName };

        // Act & Assert
        var exception =
            Assert.ThrowsException<ArgumentException>(() => _homeOwnerService.NameHome(args));
        Assert.AreEqual("Owner ID cannot be empty", exception.Message);
    }

    [TestMethod]
    public void NameHome_WhenHomeIdIsEmpty_ThrowsException()
    {
        // Arrange
        var ownerId = Guid.NewGuid();
        var homeId = Guid.Empty;
        var newName = "New Home Name";
        var args = new NameHomeArgs { OwnerId = ownerId, HomeId = homeId, NewName = newName };

        // Act & Assert
        var exception =
            Assert.ThrowsException<ArgumentException>(() => _homeOwnerService.NameHome(args));
        Assert.AreEqual("Home ID cannot be empty", exception.Message);
    }

    [TestMethod]
    public void NameHome_WhenNewNameIsEmpty_ThrowsException()
    {
        // Arrange
        var ownerId = Guid.NewGuid();
        var homeId = Guid.NewGuid();
        var newName = string.Empty;
        var args = new NameHomeArgs { OwnerId = ownerId, HomeId = homeId, NewName = newName };

        // Act & Assert
        var exception =
            Assert.ThrowsException<ArgumentException>(() => _homeOwnerService.NameHome(args));
        Assert.AreEqual("New name cannot be null or empty", exception.Message);
    }

    #endregion

    #endregion

    #region GetHomePermissions

    #region Success

    [TestMethod]
    public void GetHomePermissions_WhenCalled_ReturnsCorrectPermissions()
    {
        // Arrange
        var owner = new User("John", "Doe", "johndoe@gmail.com", "12345678@My",
            new Role());
        var home = new Home(owner, "Main St 123", 1.0, 2.0, 5);
        var member = new Member(_user, [new HomePermission(HomePermission.GetHome)]);
        home.AddMember(member);
        _homeRepositoryMock.Setup(repo => repo.Exists(home.Id))
            .Returns(true);
        _homeRepositoryMock.Setup(repo => repo.Get(home.Id))
            .Returns(home);

        // Act
        List<HomePermission> permissions = _homeOwnerService.GetHomePermissions(home.Id, _user.Id);

        // Assert
        _homeRepositoryMock.Verify(repo => repo.Get(home.Id), Times.Once);
        permissions.Should().BeEquivalentTo(member.HomePermissions);
    }

    [TestMethod]
    public void GetHomePermissions_WhenUserIsOwner_ReturnsAllPermissions()
    {
        // Arrange
        var home = new Home(_user, "Main St 123", 1.0, 2.0, 5);
        _homeRepositoryMock.Setup(repo => repo.Exists(home.Id))
            .Returns(true);
        _homeRepositoryMock.Setup(repo => repo.Get(home.Id))
            .Returns(home);

        // Act
        List<HomePermission> permissions = _homeOwnerService.GetHomePermissions(home.Id, _user.Id);

        // Assert
        permissions.Should().BeEquivalentTo(HomePermission.AllPermissions);
    }

    #endregion

    #region Error

    [TestMethod]
    public void GetHomePermissions_WhenUserIsNotMember_ThrowsException()
    {
        // Arrange
        var user = new User("Jane", "Doe", "jane@doe.com", "12345678@My",
            new Role());
        var home = new Home(user, "Main St 123", 1.0, 2.0, 5);
        _homeRepositoryMock.Setup(repo => repo.Exists(home.Id))
            .Returns(true);
        _homeRepositoryMock.Setup(repo => repo.Get(home.Id))
            .Returns(home);

        // Act
        var act = () => _homeOwnerService.GetHomePermissions(home.Id, _user.Id);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("You do not belong to this home.");
    }

    #endregion

    #endregion

    #region NameDevice

    #region Success

    [TestMethod]
    public void NameDevice_ShouldRenameDevice_WhenParametersAreValid()
    {
        // Arrange
        var home = new Home(_user, "Main St 123", 12.5, 12.5, 5);
        var member = new Member(_user, [new HomePermission(HomePermission.NameDevice)]);
        home.Members.Add(member);
        var ownedDevice = new OwnedDevice { HardwareId = Guid.NewGuid(), Name = "OldName", Home = home };
        var args = new NameDeviceArgs
        {
            OwnerId = _user.Id, HardwareId = ownedDevice.HardwareId.ToString(), NewName = "NewName"
        };

        _ownedDeviceRepositoryMock.Setup(repo => repo.GetByHardwareId(ownedDevice.HardwareId)).Returns(ownedDevice);
        _ownedDeviceRepositoryMock.Setup(repo => repo.Rename(It.IsAny<OwnedDevice>(), It.IsAny<string>()))
            .Callback<OwnedDevice, string>((device, newName) => device.Name = newName)
            .Verifiable();

        // Act
        _homeOwnerService.NameDevice(args);

        // Assert
        Assert.AreEqual("NewName", ownedDevice.Name);
        _ownedDeviceRepositoryMock.Verify(repo => repo.Rename(ownedDevice, "NewName"), Times.Once);
    }

    #endregion

    #region Error

    [TestMethod]
    public void NameDevice_ThrowsArgumentException_WhenOwnerIdIsEmpty()
    {
        // Arrange
        var args = new NameDeviceArgs
        {
            OwnerId = Guid.Empty, HardwareId = Guid.NewGuid().ToString(), NewName = "NewName"
        };

        // Act
        var act = () => _homeOwnerService.NameDevice(args);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void NameDevice_ThrowsArgumentException_WhenDeviceIdIsEmpty()
    {
        // Arrange
        var args = new NameDeviceArgs { OwnerId = Guid.NewGuid(), HardwareId = string.Empty, NewName = "NewName" };

        // Act
        var act = () => _homeOwnerService.NameDevice(args);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void NameDevice_ThrowsArgumentException_WhenNewNameIsEmpty()
    {
        // Arrange
        var args = new NameDeviceArgs
        {
            OwnerId = Guid.NewGuid(), HardwareId = Guid.NewGuid().ToString(), NewName = string.Empty
        };

        // Act
        var act = () => _homeOwnerService.NameDevice(args);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void NameDevice_ThrowsArgumentException_WhenDeviceDoesNotExist()
    {
        // Arrange
        var args = new NameDeviceArgs
        {
            OwnerId = Guid.NewGuid(), HardwareId = Guid.NewGuid().ToString(), NewName = "NewName"
        };
        _ownedDeviceRepositoryMock.Setup(repo => repo.GetByHardwareId(It.IsAny<Guid>())).Returns((OwnedDevice)null);

        // Act
        var act = () => _homeOwnerService.NameDevice(args);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void NameDevice_ThrowsArgumentException_WhenDeviceIdIsNotAGuid()
    {
        // Arrange
        var args = new NameDeviceArgs { OwnerId = Guid.NewGuid(), HardwareId = "invalid-guid", NewName = "NewName" };

        // Act
        var act = () => _homeOwnerService.NameDevice(args);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    #endregion

    #endregion

    #region GetOwnedDeviceByHardwareId

    #region Success

    [TestMethod]
    public void GetOwnedDeviceByHardwareId_WhenCalled_ReturnsCorrectDevice()
    {
        // Arrange
        var hardwareId = Guid.NewGuid();
        var ownedDevice = new OwnedDevice { HardwareId = hardwareId };
        _ownedDeviceRepositoryMock.Setup(repo => repo.Exists(hardwareId))
            .Returns(true);
        _ownedDeviceRepositoryMock.Setup(repo => repo.GetByHardwareId(hardwareId))
            .Returns(ownedDevice);

        // Act
        OwnedDevice result = _homeOwnerService.GetOwnedDeviceByHardwareId(hardwareId.ToString());

        // Assert
        _ownedDeviceRepositoryMock.Verify(repo => repo.GetByHardwareId(hardwareId), Times.Once);
        result.Should().Be(ownedDevice);
    }

    #endregion

    #region Error

    [TestMethod]
    public void GetOwnedDeviceByHardwareId_WhenHardwareIdIsNotAGuid_ThrowsException()
    {
        // Arrange
        var hardwareId = "invalid-guid";

        // Act
        var act = () => _homeOwnerService.GetOwnedDeviceByHardwareId(hardwareId);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void GetOwnedDeviceByHardwareId_WhenDeviceDoesNotExist_ThrowsException()
    {
        // Arrange
        var hardwareId = Guid.NewGuid();
        _ownedDeviceRepositoryMock.Setup(repo => repo.Exists(hardwareId))
            .Returns(false);

        // Act
        var act = () => _homeOwnerService.GetOwnedDeviceByHardwareId(hardwareId.ToString());

        // Assert
        act.Should().Throw<KeyNotFoundException>();
    }

    #endregion

    #endregion

    #region CreateRoom

    #region Success

    [TestMethod]
    public void CreateRoom_ShouldCreateRoomAndAddToRepository_WhenParametersAreValid()
    {
        // Arrange
        var homeId = Guid.NewGuid().ToString();
        var home = new Home { Id = Guid.Parse(homeId), Address = "Arteaga 1470", NickName = "My Home", Rooms = [] };
        var roomName = "Living Room";
        _homeRepositoryMock.Setup(repo => repo.Exists(home.Id)).Returns(true);
        _homeRepositoryMock.Setup(repo => repo.Get(It.IsAny<Guid>())).Returns(home);
        _roomRepositoryMock.Setup(repo => repo.Add(It.IsAny<Room>()));

        // Act
        var createdRoom = _homeOwnerService.CreateRoom(homeId, roomName);

        // Assert
        _roomRepositoryMock.Verify(repo => repo.Add(It.Is<Room>(r => r.Name == roomName)), Times.Once);
        createdRoom.Home.Should().Be(home);
        createdRoom.Should().NotBeNull();
    }

    #endregion

    #region Error

    [TestMethod]
    public void CreateRoom_ShouldThrowArgumentException_WhenHomeIdIsNullOrEmpty()
    {
        // Arrange
        var name = "Living Room";

        // Act
        Action act = () => _homeOwnerService.CreateRoom(null, name);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("Home ID cannot be null or empty.");
    }

    [TestMethod]
    public void CreateRoom_ShouldThrowArgumentException_WhenNameIsNullOrEmpty()
    {
        // Arrange
        var homeId = Guid.NewGuid().ToString();

        // Act
        Action act = () => _homeOwnerService.CreateRoom(homeId, null);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("Room name cannot be null or empty.");
    }

    [TestMethod]
    public void CreateRoom_ShouldThrowArgumentException_WhenHomeDoesNotExist()
    {
        // Arrange
        var homeId = Guid.NewGuid();
        var name = "Living Room";
        _homeRepositoryMock.Setup(repo => repo.Exists(homeId)).Returns(false);

        // Act
        Action act = () => _homeOwnerService.CreateRoom(homeId.ToString(), name);

        // Assert
        act.Should().Throw<KeyNotFoundException>().WithMessage("Home does not exist.");
    }

    #endregion

    #region AddOwnedDeviceToRoom

    [TestMethod]
    public void AddOwnedDeviceToRoom_ValidInput_DeviceAssociatedSuccessfully()
    {
        // Arrange
        var home = new Home(Mock.Of<User>(), "Arteaga 1470", 0, 0, 5);
        var room = new Room("Living Room", home);
        var device = new Device();
        var ownedDevice = new OwnedDevice(home, device);

        _roomRepositoryMock.Setup(repo => repo.Exists(room.Id)).Returns(true);
        _roomRepositoryMock.Setup(repo => repo.Get(room.Id)).Returns(room);
        _ownedDeviceRepositoryMock.Setup(repo => repo.GetOwnedDeviceById(device.Id)).Returns(ownedDevice);
        _roomRepositoryMock.Setup(repo => repo.Update(room)).Verifiable();

        // Act
        var result = _homeOwnerService.AddOwnedDeviceToRoom(room.Id.ToString(), device.Id.ToString());

        // Assert
        result.Should().Be(ownedDevice.HardwareId);
        _homeRepositoryMock.VerifyAll();
    }

    #region Error

    [TestMethod]
    public void AddOwnedDeviceToRoom_WhenRoomDoesNotExist_ThrowsArgumentException()
    {
        // Arrange
        var roomId = Guid.NewGuid().ToString();
        var deviceId = Guid.NewGuid().ToString();
        _roomRepositoryMock.Setup(repo => repo.Exists(It.IsAny<Guid>())).Returns(false);

        // Act
        Action act = () => _homeOwnerService.AddOwnedDeviceToRoom(roomId, deviceId);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("Invalid room ID.");
    }

    #endregion

    #endregion

    #endregion

    #region GetRoom

    #region Success

    [TestMethod]
    public void GetRoom_WhenCalled_ReturnsCorrectRoom()
    {
        // Arrange
        var home = new Home(_user, "Main St 123", 1.0, 2.0, 5);
        var room = new Room { Id = Guid.NewGuid(), Name = "Living Room", Home = home };
        _roomRepositoryMock.Setup(repo => repo.Get(room.Id)).Returns(room);
        _roomRepositoryMock.Setup(repo => repo.Exists(room.Id)).Returns(true);

        // Act
        Room result = _homeOwnerService.GetRoom(room.Id.ToString());

        // Assert
        _roomRepositoryMock.Verify(repo => repo.Exists(room.Id), Times.Once);
        _roomRepositoryMock.Verify(repo => repo.Get(room.Id), Times.Once);
        result.Should().Be(room);
    }

    #endregion

    #region Error

    [TestMethod]
    public void GetRoom_WhenRoomIdIsNotAGuid_ThrowsException()
    {
        // Arrange
        var roomId = "invalid-guid";

        // Act
        var act = () => _homeOwnerService.GetRoom(roomId);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void GetRoom_WhenRoomDoesNotExist_ThrowsException()
    {
        // Arrange
        var roomId = Guid.NewGuid();
        _roomRepositoryMock.Setup(repo => repo.Exists(roomId)).Returns(false);

        // Act
        var act = () => _homeOwnerService.GetRoom(roomId.ToString());

        // Assert
        act.Should().Throw<KeyNotFoundException>();
    }

    #endregion

    #endregion

    #region GetRoomsByHomeId

    [TestMethod]
    public void GetRoomsByHomeId_ShouldReturnRooms()
    {
        // Arrange
        var homeId = "123e4567-e89b-12d3-a456-426614174000";
        var rooms = new List<Room>
        {
            new Room { Id = Guid.NewGuid(), Name = "Room1" }, new Room { Id = Guid.NewGuid(), Name = "Room2" }
        };
        _roomRepositoryMock.Setup(repo => repo.GetRoomsByHomeId(It.IsAny<Guid>())).Returns(rooms);

        // Act
        var result = _homeOwnerService.GetRoomsByHomeId(homeId);

        // Assert
        result.Count.Should().Be(rooms.Count);
        result[0].Name.Should().Be(rooms[0].Name);
        result[1].Name.Should().Be(rooms[1].Name);
    }

    #endregion
}
