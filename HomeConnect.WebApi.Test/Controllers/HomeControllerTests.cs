using BusinessLogic.BusinessOwners.Entities;
using BusinessLogic.Devices.Entities;
using BusinessLogic.HomeOwners.Entities;
using BusinessLogic.HomeOwners.Models;
using BusinessLogic.HomeOwners.Services;
using BusinessLogic.Roles.Entities;
using BusinessLogic.Users.Entities;
using FluentAssertions;
using HomeConnect.WebApi.Controllers.HomeOwners.Models;
using HomeConnect.WebApi.Controllers.Homes;
using HomeConnect.WebApi.Controllers.Homes.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace HomeConnect.WebApi.Test.Controllers;

[TestClass]
public class HomeControllerTests
{
    private static readonly User _user = new("John", "Doe", "email@email.com", "Password@100",
        new Role { Name = "HomeOwner", Permissions = [] });

    private readonly Home _home = new(_user, "Road 123", 50.456, 100.789, 3);

    private readonly User _otherUser = new("Jane", "Doe", "email2@email.com", "Password@100",
        new Role { Name = "HomeOwner", Permissions = [] });

    private HomeController _controller = null!;
    private Mock<IHomeOwnerService> _homeOwnerService = null!;
    private Mock<HttpContext> _httpContextMock = null!;

    [TestInitialize]
    public void Initialize()
    {
        _httpContextMock = new Mock<HttpContext>(MockBehavior.Strict);
        _homeOwnerService = new Mock<IHomeOwnerService>(MockBehavior.Strict);
        _controller = new HomeController(_homeOwnerService.Object)
        {
            ControllerContext = new ControllerContext { HttpContext = _httpContextMock.Object }
        };
    }

    #region CreateHome

    [TestMethod]
    public void CreateHome_WhenCalledWithValidRequest_ReturnsCreatedResponse()
    {
        // Arrange
        var request = new CreateHomeRequest
        {
            Address = "Road 123",
            Latitude = 50.456,
            Longitude = 100.789,
            MaxMembers = 3,
            Name = "Lo de Maxi"
        };
        var items = new Dictionary<object, object?> { { Item.UserLogged, _user } };
        _httpContextMock.Setup(h => h.Items).Returns(items);
        var home = new Home(_user, request.Address, request.Latitude, request.Longitude, request.MaxMembers,
            request.Name);
        var args = new CreateHomeArgs
        {
            HomeOwnerId = _user.Id.ToString(),
            Address = request.Address,
            Latitude = request.Latitude,
            Longitude = request.Longitude,
            MaxMembers = request.MaxMembers,
            Name = request.Name
        };
        _homeOwnerService.Setup(x => x.CreateHome(args)).Returns(home.Id);

        // Act
        CreateHomeResponse response = _controller.CreateHome(request);

        // Assert
        _homeOwnerService.VerifyAll();
        response.Should().NotBeNull();
        response.Id.Should().Be(home.Id.ToString());
    }

    #endregion

    #region AddMember

    [TestMethod]
    public void AddMember_WhenCalledWithValidRequest_ReturnsCreatedResponse()
    {
        // Arrange
        var request = new AddMemberRequest
        {
            Email = _user.Email,
            Permissions = [SystemPermission.AddDevice, SystemPermission.GetDevices],
        };
        var items = new Dictionary<object, object?> { { Item.UserLogged, _user } };
        _httpContextMock.Setup(h => h.Items).Returns(items);
        _homeOwnerService.Setup(x => x.AddMemberToHome(It.IsAny<AddMemberArgs>())).Returns(_user.Id);

        // Act
        AddMemberResponse response = _controller.AddMember(_home.Id.ToString(), request);

        // Assert
        _homeOwnerService.VerifyAll();
        response.Should().NotBeNull();
        response.HomeId.Should().Be(_home.Id.ToString());
        response.MemberId.Should().Be(_user.Id.ToString());
    }

    #endregion

    #region AddDevices

    [TestMethod]
    public void AddDevices_WhenCalledWithValidRequest_ReturnsOkResponse()
    {
        // Arrange
        var businessOwner = new User("Business", "Owner", "bo@email.com", "Password@100",
            new Role { Name = "BusinessOwner", Permissions = [] });
        var business = new Business("123456789123", "business", "https://example.com/image.png", businessOwner);
        var sensor = new Device("sensor", "123", "a camera", "https://www.example.com/photo1.jpg", [],
            "sensor", business);
        var camera = new Camera("camera", "123", "a camera", "https://www.example.com/photo1.jpg", [],
            business, true, true, false, true);

        var request = new AddDevicesRequest { DeviceIds = [sensor.Id.ToString(), camera.Id.ToString()] };
        var items = new Dictionary<object, object?> { { Item.UserLogged, _user } };
        _httpContextMock.Setup(h => h.Items).Returns(items);
        var args = new AddDevicesArgs { HomeId = _home.Id.ToString(), DeviceIds = request.DeviceIds };
        _homeOwnerService.Setup(x => x.AddDeviceToHome(args));

        // Act
        AddDevicesResponse response = _controller.AddDevices(_home.Id.ToString(), request);

        // Assert
        _homeOwnerService.VerifyAll();
        response.Should().NotBeNull();
        response.DeviceIds.Should().BeEquivalentTo(request.DeviceIds);
        response.HomeId.Should().Be(_home.Id.ToString());
    }

    #endregion

    #region GetDevices

    [TestMethod]
    public void GetDevices_WhenCalledWithValidRequest_ReturnsDevices()
    {
        // Arrange
        var room = new Room { Id = Guid.NewGuid(), Name = "Living Room" };
        var device1 = new OwnedDevice(_home,
            new Device
            {
                Name = "Device1",
                Type = DeviceType.Sensor,
                ModelNumber = "1",
                MainPhoto = "https://www.example.com/photo1.jpg",
                Business = new Business { Name = "Name1" }
            });
        device1.Room = room;
        var device2 = new OwnedDevice(_home,
            new Device
            {
                Name = "Device2",
                Type = DeviceType.Camera,
                ModelNumber = "2",
                MainPhoto = "https://www.example.com/photo2.jpg",
                Business = new Business { Name = "Name2" }
            });
        device2.Room = room;
        var items = new Dictionary<object, object?> { { Item.UserLogged, _user } };
        _httpContextMock.Setup(h => h.Items).Returns(items);
        _homeOwnerService.Setup(x => x.GetHomeDevices(_home.Id.ToString(), null))
            .Returns(new List<OwnedDevice> { device1, device2 });

        var expectedResponse = new GetHomeDevicesResponse
        {
            Devices =
            [
                new ListOwnedDeviceInfo
                {
                    HardwareId = device1.HardwareId.ToString(),
                    Name = device1.Name,
                    BusinessName = device1.Device.Business.Name,
                    Type = device1.Device.Type.ToString(),
                    ModelNumber = device1.Device.ModelNumber,
                    MainPhoto = device1.Device.MainPhoto,
                    SecondaryPhotos = device1.Device.SecondaryPhotos,
                    Description = device1.Device.Description,
                    RoomId = device1.Room.Id.ToString()
                },
                new ListOwnedDeviceInfo
                {
                    HardwareId = device2.HardwareId.ToString(),
                    Name = device2.Name,
                    BusinessName = device2.Device.Business.Name,
                    Type = device2.Device.Type.ToString(),
                    ModelNumber = device2.Device.ModelNumber,
                    MainPhoto = device2.Device.MainPhoto,
                    SecondaryPhotos = device2.Device.SecondaryPhotos,
                    Description = device2.Device.Description,
                    RoomId = device2.Room.Id.ToString()
                }

            ]
        };

        // Act
        GetHomeDevicesResponse response = _controller.GetDevices(_home.Id.ToString());

        // Assert
        _homeOwnerService.VerifyAll();
        response.Should().NotBeNull();
        response.Devices.Should().NotBeNullOrEmpty();
        response.Devices.Should().HaveCount(2);
        response.Devices.Should().BeEquivalentTo(expectedResponse.Devices);
    }

    [TestMethod]
    public void GetDevices_WhenCalledWithAHomeWithALamp_ShouldHaveTheStateOfTheLamp()
    {
        // Arrange
        var room = new Room { Id = Guid.NewGuid(), Name = "Living Room" };
        var lamp1 = new LampOwnedDevice(_home,
            new Device
            {
                Name = "Device1",
                Type = DeviceType.Lamp,
                ModelNumber = "1",
                MainPhoto = "https://www.example.com/photo1.jpg",
                Business = new Business { Name = "Name1" },
            });
        lamp1.Room = room;

        var lamp2 = new LampOwnedDevice(_home,
            new Device
            {
                Name = "Device2",
                Type = DeviceType.Lamp,
                ModelNumber = "2",
                MainPhoto = "https://www.example.com/photo2.jpg",
                Business = new Business { Name = "Name2" }
            });
        lamp2.Room = room;

        var items = new Dictionary<object, object?> { { Item.UserLogged, _user } };
        _httpContextMock.Setup(h => h.Items).Returns(items);
        _homeOwnerService.Setup(x => x.GetHomeDevices(_home.Id.ToString(), null))
            .Returns(new List<OwnedDevice> { lamp1, lamp2 });

        var expectedResponse = new GetHomeDevicesResponse
        {
            Devices =
            [
                new ListOwnedDeviceInfo
                {
                    HardwareId = lamp1.HardwareId.ToString(),
                    Name = lamp1.Name,
                    BusinessName = lamp1.Device.Business.Name,
                    Type = lamp1.Device.Type.ToString(),
                    ModelNumber = lamp1.Device.ModelNumber,
                    MainPhoto = lamp1.Device.MainPhoto,
                    SecondaryPhotos = lamp1.Device.SecondaryPhotos,
                    State = false,
                    Description = lamp1.Device.Description,
                    RoomId = lamp1.Room.Id.ToString()
                },
                new ListOwnedDeviceInfo
                {
                    HardwareId = lamp2.HardwareId.ToString(),
                    Name = lamp2.Name,
                    BusinessName = lamp2.Device.Business.Name,
                    Type = lamp2.Device.Type.ToString(),
                    ModelNumber = lamp2.Device.ModelNumber,
                    MainPhoto = lamp2.Device.MainPhoto,
                    SecondaryPhotos = lamp2.Device.SecondaryPhotos,
                    State = false,
                    Description = lamp2.Device.Description,
                    RoomId = lamp2.Room.Id.ToString()
                }

            ]
        };

        // Act
        GetHomeDevicesResponse response = _controller.GetDevices(_home.Id.ToString());

        // Assert
        _homeOwnerService.VerifyAll();
        response.Should().NotBeNull();
        response.Devices.Should().NotBeNullOrEmpty();
        response.Devices.Should().HaveCount(2);
        response.Devices.Should().BeEquivalentTo(expectedResponse.Devices);
    }

    [TestMethod]
    public void GetDevices_WhenCalledWithAHomeWithASensor_ShouldHaveTheStateOfTheSensor()
    {
        // Arrange
        var room = new Room { Id = Guid.NewGuid(), Name = "Living Room" };
        var sensor1 = new SensorOwnedDevice(_home,
            new Device
            {
                Name = "Device1",
                Type = DeviceType.Sensor,
                ModelNumber = "1",
                MainPhoto = "https://www.example.com/photo1.jpg",
                Business = new Business { Name = "Name1" }
            });

        sensor1.Room = room;

        var sensor2 = new SensorOwnedDevice(_home,
            new Device
            {
                Name = "Device2",
                Type = DeviceType.Sensor,
                ModelNumber = "2",
                MainPhoto = "https://www.example.com/photo2.jpg",
                Business = new Business { Name = "Name2" }
            });

        sensor2.Room = room;

        var items = new Dictionary<object, object?> { { Item.UserLogged, _user } };
        _httpContextMock.Setup(h => h.Items).Returns(items);
        _homeOwnerService.Setup(x => x.GetHomeDevices(_home.Id.ToString(), null))
            .Returns(new List<OwnedDevice> { sensor1, sensor2 });

        var expectedResponse = new GetHomeDevicesResponse
        {
            Devices =
            [
                new ListOwnedDeviceInfo
                {
                    HardwareId = sensor1.HardwareId.ToString(),
                    Name = sensor1.Name,
                    BusinessName = sensor1.Device.Business.Name,
                    Type = sensor1.Device.Type.ToString(),
                    ModelNumber = sensor1.Device.ModelNumber,
                    MainPhoto = sensor1.Device.MainPhoto,
                    SecondaryPhotos = sensor1.Device.SecondaryPhotos,
                    IsOpen = false,
                    Description = sensor1.Device.Description,
                    RoomId = sensor1.Room.Id.ToString()
                },
                new ListOwnedDeviceInfo
                {
                    HardwareId = sensor2.HardwareId.ToString(),
                    Name = sensor2.Name,
                    BusinessName = sensor2.Device.Business.Name,
                    Type = sensor2.Device.Type.ToString(),
                    ModelNumber = sensor2.Device.ModelNumber,
                    MainPhoto = sensor2.Device.MainPhoto,
                    SecondaryPhotos = sensor2.Device.SecondaryPhotos,
                    IsOpen = false,
                    Description = sensor2.Device.Description,
                    RoomId = sensor2.Room.Id.ToString()
                }

            ]
        };

        // Act
        GetHomeDevicesResponse response = _controller.GetDevices(_home.Id.ToString());

        // Assert
        _homeOwnerService.VerifyAll();
        response.Should().NotBeNull();
        response.Devices.Should().NotBeNullOrEmpty();
        response.Devices.Should().HaveCount(2);
        response.Devices.Should().BeEquivalentTo(expectedResponse.Devices);
    }

    #endregion

    #region GetMembers

    [TestMethod]
    public void GetMembers_WhenCalledWithValidRequest_ReturnsMembers()
    {
        // Arrange
        var owner = new User("owner", "owner", "email@email.com", "Password@100",
            new Role { Name = "HomeOwner", Permissions = [] });
        var home = new Home(owner, "Road 123", 50.456, 100.789, 3);
        var member = new Member(_user,
        [
            new HomePermission(HomePermission.AddDevice),
            new HomePermission(HomePermission.GetDevices)
        ]);
        var otherMember = new Member(_otherUser,
            [new HomePermission(HomePermission.AddDevice), new HomePermission(HomePermission.GetDevices)]);
        home.AddMember(member);
        home.AddMember(otherMember);
        var items = new Dictionary<object, object?> { { Item.UserLogged, _user } };
        _httpContextMock.Setup(h => h.Items).Returns(items);
        _homeOwnerService.Setup(x => x.GetHomeMembers(home.Id.ToString()))
            .Returns([member, otherMember]);

        GetMembersResponse expectedResponse = CreateGetMembersResponse(member, otherMember);

        // Act
        GetMembersResponse response = _controller.GetMembers(home.Id.ToString());

        // Assert
        _homeOwnerService.VerifyAll();
        response.Should().NotBeNull();
        response.Members.Should().NotBeNullOrEmpty();
        response.Members.Should().HaveCount(2);
        response.Members.Should().BeEquivalentTo(expectedResponse.Members);
    }

    private static GetMembersResponse CreateGetMembersResponse(Member member, Member otherMember)
    {
        return new GetMembersResponse
        {
            Members =
            [
                new ListMemberInfo
                {
                    Id = member.Id.ToString(),
                    Name = member.User.Name,
                    Surname = member.User.Surname,
                    Photo = member.User.ProfilePicture ?? string.Empty,
                    Permissions = member.HomePermissions.Select(hp => hp.Value).ToList()
                },

                new ListMemberInfo
                {
                    Id = otherMember.Id.ToString(),
                    Name = otherMember.User.Name,
                    Surname = otherMember.User.Surname,
                    Photo = otherMember.User.ProfilePicture ?? string.Empty,
                    Permissions = otherMember.HomePermissions.Select(hp => hp.Value).ToList()
                }

            ]
        };
    }

    #endregion

    #region GetHomes

    [TestMethod]
    public void GetHomes_WhenCalledWithValidRequest_ReturnsHomes()
    {
        // Arrange
        var home1 = new Home
        {
            Id = Guid.NewGuid(),
            Address = "Amarales 3420",
            Latitude = 40.7128,
            Longitude = -74.0060,
            MaxMembers = 4
        };
        var home2 = new Home
        {
            Id = Guid.NewGuid(),
            Address = "Arteaga 1470",
            Latitude = 34.0522,
            Longitude = -118.2437,
            MaxMembers = 6
        };
        var items = new Dictionary<object, object?> { { Item.UserLogged, _user } };
        _httpContextMock.Setup(h => h.Items).Returns(items);
        _homeOwnerService.Setup(x => x.GetHomesByOwnerId(_user.Id))
            .Returns([home1, home2]);

        var expectedResponse = new GetHomesResponse
        {
            Homes =
            [
                new ListHomeInfo
                {
                    Id = home1.Id.ToString(),
                    Address = home1.Address,
                    Latitude = home1.Latitude,
                    Longitude = home1.Longitude,
                    MaxMembers = home1.MaxMembers
                },
                new ListHomeInfo
                {
                    Id = home2.Id.ToString(),
                    Address = home2.Address,
                    Latitude = home2.Latitude,
                    Longitude = home2.Longitude,
                    MaxMembers = home2.MaxMembers
                }

            ]
        };

        // Act
        GetHomesResponse response = _controller.GetHomes();

        // Assert
        _homeOwnerService.VerifyAll();
        response.Should().NotBeNull();
        response.Homes.Should().NotBeNullOrEmpty();
        response.Homes.Should().HaveCount(2);
        response.Homes.Should().BeEquivalentTo(expectedResponse.Homes);
    }

    #endregion

    #region NameHome

    #region Success

    [TestMethod]
    public void NameHome_WithValidRequest_ReturnsHomeId()
    {
        // Arrange
        var request = new NameHomeRequest { NewName = "New Home Name" };
        var homeId = Guid.NewGuid();
        var items = new Dictionary<object, object?> { { Item.UserLogged, _user } };
        var args = new NameHomeArgs { HomeId = homeId, NewName = request.NewName, OwnerId = _user.Id };
        _httpContextMock.Setup(h => h.Items).Returns(items);
        _homeOwnerService.Setup(x => x.NameHome(args));

        // Act
        NameHomeResponse response = _controller.NameHome(homeId.ToString(), request);

        // Assert
        _homeOwnerService.Verify(x => x.NameHome(args), Times.Once);
        response.Should().NotBeNull();
        response.HomeId.Should().Be(homeId.ToString());
    }

    #endregion

    #region Error

    [TestMethod]
    public void NameHome_WithInvalidNewName_ThrowsArgumentException()
    {
        // Arrange
        var request = new NameHomeRequest { NewName = string.Empty };
        var items = new Dictionary<object, object?> { { Item.UserLogged, _user } };
        _httpContextMock.Setup(h => h.Items).Returns(items);

        // Act
        var act = () => _controller.NameHome(Guid.NewGuid().ToString(), request);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void NameHome_WithInvalidHomeId_ThrowsArgumentException()
    {
        // Arrange
        var request = new NameHomeRequest { NewName = "New Home Name" };
        var items = new Dictionary<object, object?> { { Item.UserLogged, _user } };
        _httpContextMock.Setup(h => h.Items).Returns(items);

        // Act
        var act = () => _controller.NameHome(string.Empty, request);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    #endregion

    #endregion

    #region GetHome

    #region Success

    [TestMethod]
    public void GetHome_WithValidRequest_ReturnsHome()
    {
        // Arrange
        var homeId = _home.Id;
        var items = new Dictionary<object, object?> { { Item.UserLogged, _user } };
        _httpContextMock.Setup(h => h.Items).Returns(items);
        _homeOwnerService.Setup(x => x.GetHome(homeId)).Returns(_home);

        // Act
        GetHomeResponse response = _controller.GetHome(homeId.ToString());

        // Assert
        _homeOwnerService.VerifyAll();
        response.Id.Should().Be(_home.Id.ToString());
        response.Owner.Id.Should().Be(_home.Owner.Id.ToString());
        response.Address.Should().Be(_home.Address);
        response.Latitude.Should().Be(_home.Latitude);
        response.Longitude.Should().Be(_home.Longitude);
        response.MaxMembers.Should().Be(_home.MaxMembers);
    }

    #endregion

    #endregion

    #region GetHomePermissions

    #region Success

    [TestMethod]
    public void GetHomePermissions_WithValidRequest_ReturnsHomePermissions()
    {
        // Arrange
        var homeId = _home.Id;
        var items = new Dictionary<object, object?> { { Item.UserLogged, _user } };
        _httpContextMock.Setup(h => h.Items).Returns(items);
        _homeOwnerService.Setup(x => x.GetHomePermissions(homeId, _user.Id))
            .Returns([new HomePermission(HomePermission.AddDevice), new HomePermission(HomePermission.GetDevices)]);

        // Act
        GetHomePermissionsResponse response = _controller.GetHomePermissions(homeId.ToString());

        // Assert
        _homeOwnerService.VerifyAll();
        response.HomeId.Should().Be(homeId.ToString());
        response.HomePermissions.Should().NotBeNullOrEmpty();
        response.HomePermissions.Should().HaveCount(2);
        response.HomePermissions.Should().BeEquivalentTo(new[] { HomePermission.AddDevice, HomePermission.GetDevices });
    }

    #endregion

    #endregion

    #region CreateRoom

    [TestMethod]
    public void CreateRoom_WhenCalledWithValidRequest_ReturnsCreatedResponse()
    {
        // Arrange
        var homeId = "123e4567-e89b-12d3-a456-426614174000";
        var name = "Living Room";
        var room = new Room { Id = Guid.NewGuid(), Name = name };

        _homeOwnerService.Setup(x => x.CreateRoom(It.IsAny<string>(), name)).Returns(room);

        var request = new CreateRoomRequest { Name = name };

        // Act
        var response = _controller.CreateRoom(homeId, request);

        // Assert
        _homeOwnerService.VerifyAll();
        response.Should().NotBeNull();
        response.RoomId.Should().Be(room.Id.ToString());
    }

    #endregion

    #region GetRooms

    [TestMethod]
    public void GetRooms_WhenCalledWithValidRequest_ReturnsRooms()
    {
        // Arrange
        var room1 = new Room
        {
            Id = Guid.NewGuid(),
            Name = "Living Room",
            Home = _home,
            OwnedDevices =
            [
                new OwnedDevice { HardwareId = Guid.NewGuid() },
                new OwnedDevice { HardwareId = Guid.NewGuid() }
            ]
        };
        var room2 = new Room
        {
            Id = Guid.NewGuid(),
            Name = "Bedroom",
            Home = _home,
            OwnedDevices =
            [
                new OwnedDevice { HardwareId = Guid.NewGuid() }
            ]
        };
        var items = new Dictionary<object, object?> { { Item.UserLogged, _user } };
        _httpContextMock.Setup(h => h.Items).Returns(items);
        _homeOwnerService.Setup(x => x.GetRoomsByHomeId(_home.Id.ToString()))
            .Returns([room1, room2]);

        var expectedResponse = new GetRoomsResponse
        {
            Rooms =
            [
                new ListRoomInfo
                {
                    Id = room1.Id.ToString(),
                    Name = room1.Name,
                    HomeId = room1.Home.Id.ToString(),
                    OwnedDevicesId = room1.OwnedDevices.Select(od => od.HardwareId.ToString()).ToList()
                },
                new ListRoomInfo
                {
                    Id = room2.Id.ToString(),
                    Name = room2.Name,
                    HomeId = room2.Home.Id.ToString(),
                    OwnedDevicesId = room2.OwnedDevices.Select(od => od.HardwareId.ToString()).ToList()
                }

            ]
        };

        // Act
        GetRoomsResponse response = _controller.GetRooms(_home.Id.ToString());

        // Assert
        _homeOwnerService.VerifyAll();
        response.Should().NotBeNull();
        response.Rooms.Should().NotBeNullOrEmpty();
        response.Rooms.Should().HaveCount(2);
        response.Rooms.Should().BeEquivalentTo(expectedResponse.Rooms);
    }

    #endregion
}
