using BusinessLogic;
using BusinessLogic.BusinessOwners.Entities;
using BusinessLogic.Devices.Entities;
using BusinessLogic.Devices.Models;
using BusinessLogic.Devices.Repositories;
using BusinessLogic.Devices.Services;
using BusinessLogic.HomeOwners.Entities;
using BusinessLogic.HomeOwners.Repositories;
using BusinessLogic.Notifications.Models;
using BusinessLogic.Notifications.Services;
using BusinessLogic.Roles.Entities;
using BusinessLogic.Users.Entities;
using FluentAssertions;
using Moq;

namespace HomeConnect.BusinessLogic.Test.Devices.Services;

[TestClass]
public class DeviceServiceTests
{
    private Mock<IDeviceRepository> _deviceRepository = null!;
    private List<Device> _devices = null!;
    private DeviceService _deviceService = null!;
    private Mock<IOwnedDeviceRepository> _ownedDeviceRepository = null!;
    private Mock<INotificationService> _notificationService = null!;
    private Mock<IHomeRepository> _homeRepository = null!;
    private PagedData<Device> _pagedDeviceList = null!;
    private GetDevicesArgs _parameters = null!;
    private Device otherDevice = null!;
    private User user1 = null!;
    private User user2 = null!;
    private Device validDevice = null!;

    [TestInitialize]
    public void TestInitialize()
    {
        _deviceRepository = new Mock<IDeviceRepository>(MockBehavior.Strict);
        _ownedDeviceRepository = new Mock<IOwnedDeviceRepository>(MockBehavior.Strict);
        _notificationService = new Mock<INotificationService>(MockBehavior.Strict);
        _homeRepository = new Mock<IHomeRepository>(MockBehavior.Strict);
        _deviceService = new DeviceService(_deviceRepository.Object, _ownedDeviceRepository.Object,
            _notificationService.Object, _homeRepository.Object);

        user1 = new User("name", "surname", "email1@email.com", "Password#100", new Role());
        user2 = new User("name", "surname", "email2@email.com", "Password#100", new Role());
        validDevice = new Device("Device1", 12345, "Device description1", "https://example1.com/image.png",
            [], "Sensor", new Business("Rut1", "Business", "https://example.com/image.png", user1));
        otherDevice = new Device("Device2", 12345, "Device description2", "https://example2.com/image.png",
            [], "Sensor", new Business("Rut2", "Business", "https://example.com/image.png", user2));

        _devices = [validDevice, otherDevice];

        _parameters = new GetDevicesArgs { Page = 1, PageSize = 10 };

        _pagedDeviceList = new PagedData<Device>
        {
            Data = _devices,
            Page = _parameters.Page ?? 1,
            PageSize = _parameters.PageSize ?? 10,
            TotalPages = 1
        };
    }

    [TestMethod]
    [DataRow("hardwareId")]
    [DataRow("")]
    public void TurnDevice_WhenHardwareIdIsInvalid_ThrowsArgumentException(string id)
    {
        // Act
        Func<bool> act = () => _deviceService.TurnDevice(id, true);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("Hardware ID is invalid.");
    }

    [TestMethod]
    public void TurnDevice_WhenHardwareIdIsValid_ReturnsConnected()
    {
        // Arrange
        var ownedDevice =
            new OwnedDevice(new Home(user1, "Street 3420", 50, 100, 5), validDevice) { Connected = true };
        var hardwareId = Guid.NewGuid().ToString();
        _ownedDeviceRepository.Setup(x => x.Exists(Guid.Parse(hardwareId))).Returns(true);
        _ownedDeviceRepository.Setup(x => x.GetByHardwareId(Guid.Parse(hardwareId)))
            .Returns(ownedDevice);
        _ownedDeviceRepository.Setup(x => x.Update(ownedDevice)).Verifiable();

        // Act
        var result = _deviceService.TurnDevice(hardwareId, false);

        // Assert
        result.Should().BeFalse();
        _ownedDeviceRepository.VerifyAll();
    }

    [TestMethod]
    public void TurnDevice_WhenOwnedDeviceDoesNotExist_ThrowsKeyNotFoundException()
    {
        // Arrange
        var hardwareId = Guid.NewGuid().ToString();
        _ownedDeviceRepository.Setup(x => x.Exists(Guid.Parse(hardwareId))).Returns(false);

        // Act
        Func<bool> act = () => _deviceService.TurnDevice(hardwareId, true);

        // Assert
        act.Should().Throw<KeyNotFoundException>().WithMessage("The device is not registered in this home.");
    }

    [TestMethod]
    public void GetDevices_WhenCalled_ReturnsDeviceList()
    {
        // Arrange
        _deviceRepository.Setup(x => x.GetPaged(It.Is<GetDevicesArgs>(args =>
            args.Page == _parameters.Page &&
            args.PageSize == _parameters.PageSize &&
            args.DeviceNameFilter == _parameters.DeviceNameFilter &&
            args.ModelNumberFilter == _parameters.ModelNumberFilter &&
            args.BusinessNameFilter == _parameters.BusinessNameFilter &&
            args.DeviceTypeFilter == _parameters.DeviceTypeFilter))).Returns(_pagedDeviceList);

        // Act
        PagedData<Device> result = _deviceService.GetDevices(_parameters);

        // Assert
        var expectedPagedDeviceList = new PagedData<Device>
        {
            Data = _devices,
            Page = _parameters.Page ?? 1,
            PageSize = _parameters.PageSize ?? 10,
            TotalPages = 1
        };

        result.Should().BeEquivalentTo(expectedPagedDeviceList,
            options => options.ComparingByMembers<PagedData<Device>>());
        _deviceRepository.Verify(x => x.GetPaged(
            It.Is<GetDevicesArgs>(args =>
                args.Page == _parameters.Page &&
                args.PageSize == _parameters.PageSize &&
                args.DeviceNameFilter == _parameters.DeviceNameFilter &&
                args.ModelNumberFilter == _parameters.ModelNumberFilter &&
                args.BusinessNameFilter == _parameters.BusinessNameFilter &&
                args.DeviceTypeFilter == _parameters.DeviceTypeFilter)), Times.Once);
    }

    [TestMethod]
    public void GetAllDeviceTypes_WhenCalled_ReturnsDeviceTypes()
    {
        // Arrange
        var expectedDeviceTypes = Enum.GetNames(typeof(DeviceType));

        // Act
        IEnumerable<string> result = _deviceService.GetAllDeviceTypes();

        // Assert
        result.Should().BeEquivalentTo(expectedDeviceTypes);
    }

    #region IsConnected

    #region Error

    [TestMethod]
    public void IsConnected_WhenHardwareIdIsInvalid_ThrowsArgumentException()
    {
        // Arrange
        var hardwareId = "hardwareId";

        // Act
        Func<bool> act = () => _deviceService.IsConnected(hardwareId);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("Hardware ID is invalid.");
    }

    [TestMethod]
    public void IsConnected_WhenOwnedDeviceDoesNotExist_ThrowsKeyNotFoundException()
    {
        // Arrange
        var hardwareId = Guid.NewGuid().ToString();
        _ownedDeviceRepository.Setup(x => x.Exists(Guid.Parse(hardwareId))).Returns(false);

        // Act
        Func<bool> act = () => _deviceService.IsConnected(hardwareId);

        // Assert
        act.Should().Throw<KeyNotFoundException>().WithMessage("The device is not registered in this home.");
    }

    #endregion

    [TestMethod]
    public void IsConnected_WhenDeviceIsConnected_ReturnsTrue()
    {
        // Arrange
        var ownedDevice =
            new OwnedDevice(new Home(user1, "Street 3420", 50, 100, 5), validDevice) { Connected = true };
        var hardwareId = Guid.NewGuid().ToString();
        _ownedDeviceRepository.Setup(x => x.Exists(Guid.Parse(hardwareId))).Returns(true);
        _ownedDeviceRepository.Setup(x => x.GetByHardwareId(Guid.Parse(hardwareId)))
            .Returns(ownedDevice);

        // Act
        var result = _deviceService.IsConnected(hardwareId);

        // Assert
        _ownedDeviceRepository.VerifyAll();
        result.Should().BeTrue();
    }

    #endregion

    #region TurnLamp

    #region Error

    [TestMethod]
    public void TurnLamp_WhenLampDoesNotExist_ThrowsKeyNotFoundException()
    {
        // Arrange
        var hardwareId = Guid.NewGuid().ToString();
        _ownedDeviceRepository.Setup(x => x.Exists(Guid.Parse(hardwareId))).Returns(false);
        var args = new NotificationArgs { HardwareId = hardwareId, Date = DateTime.Now, Event = "example" };

        // Act
        Action act = () => _deviceService.TurnLamp(hardwareId, true, args);

        // Assert
        act.Should().Throw<KeyNotFoundException>().WithMessage("The device is not registered in this home.");
    }

    [TestMethod]
    public void TurnLamp_WhenHardwareIdIsInvalid_ThrowsArgumentException()
    {
        // Arrange
        var hardwareId = "hardwareId";
        var args = new NotificationArgs { HardwareId = hardwareId, Date = DateTime.Now, Event = "example" };

        // Act
        Action act = () => _deviceService.TurnLamp(hardwareId, true, args);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("Hardware ID is invalid.");
    }

    #endregion

    #region Success

    [TestMethod]
    public void TurnLamp_WhenCalledWithValidHardwareId_SetsState()
    {
        // Arrange
        var hardwareId = Guid.NewGuid().ToString();
        _ownedDeviceRepository.Setup(x => x.Exists(Guid.Parse(hardwareId))).Returns(true);
        _ownedDeviceRepository.Setup(x => x.UpdateLampState(Guid.Parse(hardwareId), true)).Verifiable();
        _ownedDeviceRepository.Setup(x => x.GetLampState(Guid.Parse(hardwareId))).Returns(true);
        var args = new NotificationArgs { HardwareId = hardwareId, Date = DateTime.Now, Event = "example" };

        // Act
        _deviceService.TurnLamp(hardwareId, true, args);

        // Assert
        _ownedDeviceRepository.VerifyAll();
    }

    [TestMethod]
    public void TurnLamp_WhenCalledWithValidHardwareIdAndCurrentStateIsEqualToTheNewState_DoesNotCreateANotification()
    {
        // Arrange
        var hardwareId = Guid.NewGuid().ToString();
        _ownedDeviceRepository.Setup(x => x.Exists(Guid.Parse(hardwareId))).Returns(true);
        _ownedDeviceRepository.Setup(x => x.UpdateLampState(Guid.Parse(hardwareId), true)).Verifiable();
        _ownedDeviceRepository.Setup(x => x.GetLampState(Guid.Parse(hardwareId))).Returns(true);
        var args = new NotificationArgs { HardwareId = hardwareId, Date = DateTime.Now, Event = "example" };

        // Act
        _deviceService.TurnLamp(hardwareId, true, args);

        // Assert
        _ownedDeviceRepository.VerifyAll();
    }

    [TestMethod]
    public void TurnLamp_WhenCalledWithValidHardwareIdAndCurrentStateIsDifferentThanTheNewState_CreatesANotification()
    {
        // Arrange
        var hardwareId = Guid.NewGuid().ToString();
        var date = DateTime.Now;
        _ownedDeviceRepository.Setup(x => x.Exists(Guid.Parse(hardwareId))).Returns(true);
        _ownedDeviceRepository.Setup(x => x.UpdateLampState(Guid.Parse(hardwareId), true)).Verifiable();
        _ownedDeviceRepository.Setup(x => x.GetLampState(Guid.Parse(hardwareId))).Returns(false);
        var args = new NotificationArgs { HardwareId = hardwareId, Date = DateTime.Now, Event = "example" };
        _notificationService.Setup(x => x.Notify(args, _deviceService));

        // Act
        _deviceService.TurnLamp(hardwareId, true, args);

        // Assert
        _notificationService.Verify(x => x.Notify(It.Is<NotificationArgs>(y =>
            y.HardwareId == hardwareId &&
            y.Date.Year == date.Year &&
            y.Date.Month == date.Month &&
            y.Date.Day == date.Day &&
            y.Date.Hour == date.Hour &&
            y.Date.Minute == date.Minute &&
            y.Event == args.Event), _deviceService), Times.Once);
        _ownedDeviceRepository.VerifyAll();
    }

    #endregion
    #endregion

    #region UpdateSensorState
    #region Error

    [TestMethod]
    public void UpdateSensorState_WhenLampDoesNotExist_ThrowsKeyNotFoundException()
    {
        // Arrange
        var hardwareId = Guid.NewGuid().ToString();
        _ownedDeviceRepository.Setup(x => x.Exists(Guid.Parse(hardwareId))).Returns(false);
        var args = new NotificationArgs { HardwareId = hardwareId, Date = DateTime.Now, Event = "example" };

        // Act
        Action act = () => _deviceService.UpdateSensorState(hardwareId, true, args);

        // Assert
        act.Should().Throw<KeyNotFoundException>().WithMessage("The device is not registered in this home.");
    }

    [TestMethod]
    public void UpdateSensorState_WhenHardwareIdIsInvalid_ThrowsArgumentException()
    {
        // Arrange
        var hardwareId = "hardwareId";
        var args = new NotificationArgs { HardwareId = hardwareId, Date = DateTime.Now, Event = "example" };

        // Act
        Action act = () => _deviceService.UpdateSensorState(hardwareId, true, args);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("Hardware ID is invalid.");
    }

    #endregion

    #region Success

    [TestMethod]
    public void UpdateSensorState_WhenCalledWithValidHardwareId_SetsState()
    {
        // Arrange
        var hardwareId = Guid.NewGuid().ToString();
        const bool state = false;
        var args = new NotificationArgs { HardwareId = hardwareId, Date = DateTime.Now, Event = "example" };
        _ownedDeviceRepository.Setup(x => x.Exists(Guid.Parse(hardwareId))).Returns(true);
        _ownedDeviceRepository.Setup(x => x.UpdateSensorState(Guid.Parse(hardwareId), state)).Verifiable();
        _ownedDeviceRepository.Setup(x => x.GetSensorState(Guid.Parse(hardwareId))).Returns(state);

        // Act
        _deviceService.UpdateSensorState(hardwareId, state, args);

        // Assert
        _ownedDeviceRepository.VerifyAll();
    }

    [TestMethod]
    public void UpdateSensorState_WhenCalledWithValidHardwareIdAndCurrentStateIsEqualToTheNewState_DoesNotCreateANotification()
    {
        // Arrange
        var hardwareId = Guid.NewGuid().ToString();
        const bool state = false;
        var args = new NotificationArgs { HardwareId = hardwareId, Date = DateTime.Now, Event = "example" };
        _ownedDeviceRepository.Setup(x => x.Exists(Guid.Parse(hardwareId))).Returns(true);
        _ownedDeviceRepository.Setup(x => x.UpdateSensorState(Guid.Parse(hardwareId), state)).Verifiable();
        _ownedDeviceRepository.Setup(x => x.GetSensorState(Guid.Parse(hardwareId))).Returns(state);

        // Act
        _deviceService.UpdateSensorState(hardwareId, state, args);

        // Assert
        _ownedDeviceRepository.VerifyAll();
    }

    [TestMethod]
    public void UpdateSensorState_WhenCalledWithValidHardwareIdAndCurrentStateIsDifferentThanTheNewState_CreatesANotification()
    {
        // Arrange
        var hardwareId = Guid.NewGuid().ToString();
        const bool state = false;
        var date = DateTime.Now;
        var args = new NotificationArgs { HardwareId = hardwareId, Date = DateTime.Now, Event = "example" };
        _ownedDeviceRepository.Setup(x => x.Exists(Guid.Parse(hardwareId))).Returns(true);
        _ownedDeviceRepository.Setup(x => x.UpdateSensorState(Guid.Parse(hardwareId), state)).Verifiable();
        _ownedDeviceRepository.Setup(x => x.GetSensorState(Guid.Parse(hardwareId))).Returns(!state);
        _notificationService.Setup(x => x.Notify(args, _deviceService));

        // Act
        _deviceService.UpdateSensorState(hardwareId, state, args);

        // Assert
        _notificationService.Verify(x => x.Notify(It.Is<NotificationArgs>(y =>
            y.HardwareId == hardwareId &&
            y.Date.Year == date.Year &&
            y.Date.Month == date.Month &&
            y.Date.Day == date.Day &&
            y.Date.Hour == date.Hour &&
            y.Date.Minute == date.Minute &&
            y.Event == args.Event), _deviceService), Times.Once);
        _ownedDeviceRepository.VerifyAll();
    }
    #endregion
    #endregion

    #region MoveDevice
    #region Success
    [TestMethod]
    public void MoveDevice_WhenCalled_MovesDeviceSuccessfully()
    {
        // Arrange
        var sourceRoomId = Guid.NewGuid();
        var targetRoomId = Guid.NewGuid();
        var ownedDeviceId = Guid.NewGuid();
        var ownedDevice = new OwnedDevice { HardwareId = ownedDeviceId, Room = new Room { Id = sourceRoomId } };
        var sourceRoom = new Room { Id = sourceRoomId, OwnedDevices = new List<OwnedDevice> { ownedDevice } };
        var targetRoom = new Room { Id = targetRoomId, OwnedDevices = new List<OwnedDevice>() };

        _homeRepository.Setup(r => r.ExistsRoom(sourceRoomId)).Returns(true);
        _homeRepository.Setup(r => r.ExistsRoom(targetRoomId)).Returns(true);
        _homeRepository.Setup(r => r.GetRoomById(sourceRoomId)).Returns(sourceRoom);
        _homeRepository.Setup(r => r.GetRoomById(targetRoomId)).Returns(targetRoom);
        _homeRepository.Setup(r => r.UpdateRoom(It.IsAny<Room>())).Verifiable();
        _ownedDeviceRepository.Setup(r => r.UpdateOwnedDevice(It.IsAny<OwnedDevice>())).Verifiable();

        // Act
        _deviceService.MoveDevice(sourceRoomId.ToString(), targetRoomId.ToString(), ownedDeviceId.ToString());

        // Assert
        sourceRoom.OwnedDevices.Should().NotContain(ownedDevice);
        targetRoom.OwnedDevices.Should().Contain(ownedDevice);
        ownedDevice.Room.Id.Should().Be(targetRoomId);
        _homeRepository.VerifyAll();
        _deviceRepository.VerifyAll();
    }
    #endregion
    #region Error
    [TestMethod]
    public void MoveDevice_WhenSourceRoomIdIsInvalid_ThrowsArgumentException()
    {
        // Arrange
        var sourceRoomId = Guid.NewGuid();
        var targetRoomId = Guid.NewGuid();
        var ownedDeviceId = Guid.NewGuid();

        _homeRepository.Setup(r => r.ExistsRoom(sourceRoomId)).Returns(false);

        // Act
        Action act = () => _deviceService.MoveDevice(sourceRoomId.ToString(), targetRoomId.ToString(), ownedDeviceId.ToString());

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("Invalid source room ID.");
    }

    [TestMethod]
    public void MoveDevice_WhenTargetRoomIdIsInvalid_ThrowsArgumentException()
    {
        // Arrange
        var sourceRoomId = Guid.NewGuid();
        var targetRoomId = Guid.NewGuid();
        var ownedDeviceId = Guid.NewGuid();

        _homeRepository.Setup(r => r.ExistsRoom(sourceRoomId)).Returns(true);
        _homeRepository.Setup(r => r.ExistsRoom(targetRoomId)).Returns(false);

        // Act
        Action act = () => _deviceService.MoveDevice(sourceRoomId.ToString(), targetRoomId.ToString(), ownedDeviceId.ToString());

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("Invalid target room ID.");
    }

    [TestMethod]
    public void MoveDevice_WhenDeviceNotFoundInSourceRoom_ThrowsArgumentException()
    {
        // Arrange
        var sourceRoomId = Guid.NewGuid();
        var targetRoomId = Guid.NewGuid();
        var ownedDeviceId = Guid.NewGuid();
        var sourceRoom = new Room { Id = sourceRoomId, OwnedDevices = new List<OwnedDevice>() };
        var targetRoom = new Room { Id = targetRoomId, OwnedDevices = new List<OwnedDevice>() };

        _homeRepository.Setup(r => r.ExistsRoom(sourceRoomId)).Returns(true);
        _homeRepository.Setup(r => r.ExistsRoom(targetRoomId)).Returns(true);
        _homeRepository.Setup(r => r.GetRoomById(sourceRoomId)).Returns(sourceRoom);
        _homeRepository.Setup(r => r.GetRoomById(targetRoomId)).Returns(targetRoom);

        // Act
        Action act = () => _deviceService.MoveDevice(sourceRoomId.ToString(), targetRoomId.ToString(), ownedDeviceId.ToString());

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("Device not found in source room.");
    }
    #endregion
    #endregion
}
