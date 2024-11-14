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
            CanAddDevices = true,
            CanListDevices = false
        };
        var items = new Dictionary<object, object?> { { Item.UserLogged, _user } };
        _httpContextMock.Setup(h => h.Items).Returns(items);
        var args = new AddMemberArgs
        {
            HomeId = _home.Id.ToString(),
            UserEmail = _user.Email,
            CanAddDevices = request.CanAddDevices,
            CanListDevices = request.CanListDevices
        };
        _homeOwnerService.Setup(x => x.AddMemberToHome(args)).Returns(_user.Id);

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
        var device1 = new OwnedDevice(_home,
            new Device
            {
                Name = "Device1",
                Type = DeviceType.Sensor,
                ModelNumber = "1",
                MainPhoto = "https://www.example.com/photo1.jpg",
                Business = new Business { Name = "Name1" }
            });
        var device2 = new OwnedDevice(_home,
            new Device
            {
                Name = "Device2",
                Type = DeviceType.Camera,
                ModelNumber = "2",
                MainPhoto = "https://www.example.com/photo2.jpg",
                Business = new Business { Name = "Name2" }
            });
        var items = new Dictionary<object, object?> { { Item.UserLogged, _user } };
        _httpContextMock.Setup(h => h.Items).Returns(items);
        _homeOwnerService.Setup(x => x.GetHomeDevices(_home.Id.ToString()))
            .Returns(new List<OwnedDevice> { device1, device2 });

        var expectedResponse = new GetDevicesResponse
        {
            Devices =
            [
                new ListDeviceInfo
                {
                    HardwareId = device1.HardwareId.ToString(),
                    Name = device1.Device.Name,
                    BusinessName = device1.Device.Business.Name,
                    Type = device1.Device.Type.ToString(),
                    ModelNumber = device1.Device.ModelNumber,
                    MainPhoto = device1.Device.MainPhoto,
                    SecondaryPhotos = device1.Device.SecondaryPhotos
                },
                new ListDeviceInfo
                {
                    HardwareId = device2.HardwareId.ToString(),
                    Name = device2.Device.Name,
                    BusinessName = device2.Device.Business.Name,
                    Type = device2.Device.Type.ToString(),
                    ModelNumber = device2.Device.ModelNumber,
                    MainPhoto = device2.Device.MainPhoto,
                    SecondaryPhotos = device2.Device.SecondaryPhotos
                }

            ]
        };

        // Act
        GetDevicesResponse response = _controller.GetDevices(_home.Id.ToString());

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
        var lamp1 = new LampOwnedDevice(_home,
            new Device
            {
                Name = "Device1",
                Type = DeviceType.Lamp,
                ModelNumber = "1",
                MainPhoto = "https://www.example.com/photo1.jpg",
                Business = new Business { Name = "Name1" }
            });
        var lamp2 = new LampOwnedDevice(_home,
            new Device
            {
                Name = "Device2",
                Type = DeviceType.Lamp,
                ModelNumber = "2",
                MainPhoto = "https://www.example.com/photo2.jpg",
                Business = new Business { Name = "Name2" }
            });
        var items = new Dictionary<object, object?> { { Item.UserLogged, _user } };
        _httpContextMock.Setup(h => h.Items).Returns(items);
        _homeOwnerService.Setup(x => x.GetHomeDevices(_home.Id.ToString()))
            .Returns(new List<OwnedDevice> { lamp1, lamp2 });

        var expectedResponse = new GetDevicesResponse
        {
            Devices =
            [
                new ListDeviceInfo
                {
                    HardwareId = lamp1.HardwareId.ToString(),
                    Name = lamp1.Device.Name,
                    BusinessName = lamp1.Device.Business.Name,
                    Type = lamp1.Device.Type.ToString(),
                    ModelNumber = lamp1.Device.ModelNumber,
                    MainPhoto = lamp1.Device.MainPhoto,
                    SecondaryPhotos = lamp1.Device.SecondaryPhotos,
                    State = false
                },
                new ListDeviceInfo
                {
                    HardwareId = lamp2.HardwareId.ToString(),
                    Name = lamp2.Device.Name,
                    BusinessName = lamp2.Device.Business.Name,
                    Type = lamp2.Device.Type.ToString(),
                    ModelNumber = lamp2.Device.ModelNumber,
                    MainPhoto = lamp2.Device.MainPhoto,
                    SecondaryPhotos = lamp2.Device.SecondaryPhotos,
                    State = false
                }

            ]
        };

        // Act
        GetDevicesResponse response = _controller.GetDevices(_home.Id.ToString());

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
        var sensor1 = new SensorOwnedDevice(_home,
            new Device
            {
                Name = "Device1",
                Type = DeviceType.Sensor,
                ModelNumber = "1",
                MainPhoto = "https://www.example.com/photo1.jpg",
                Business = new Business { Name = "Name1" }
            });
        var sensor2 = new SensorOwnedDevice(_home,
            new Device
            {
                Name = "Device2",
                Type = DeviceType.Sensor,
                ModelNumber = "2",
                MainPhoto = "https://www.example.com/photo2.jpg",
                Business = new Business { Name = "Name2" }
            });
        var items = new Dictionary<object, object?> { { Item.UserLogged, _user } };
        _httpContextMock.Setup(h => h.Items).Returns(items);
        _homeOwnerService.Setup(x => x.GetHomeDevices(_home.Id.ToString()))
            .Returns(new List<OwnedDevice> { sensor1, sensor2 });

        var expectedResponse = new GetDevicesResponse
        {
            Devices =
            [
                new ListDeviceInfo
                {
                    HardwareId = sensor1.HardwareId.ToString(),
                    Name = sensor1.Device.Name,
                    BusinessName = sensor1.Device.Business.Name,
                    Type = sensor1.Device.Type.ToString(),
                    ModelNumber = sensor1.Device.ModelNumber,
                    MainPhoto = sensor1.Device.MainPhoto,
                    SecondaryPhotos = sensor1.Device.SecondaryPhotos,
                    IsOpen = false
                },
                new ListDeviceInfo
                {
                    HardwareId = sensor2.HardwareId.ToString(),
                    Name = sensor2.Device.Name,
                    BusinessName = sensor2.Device.Business.Name,
                    Type = sensor2.Device.Type.ToString(),
                    ModelNumber = sensor2.Device.ModelNumber,
                    MainPhoto = sensor2.Device.MainPhoto,
                    SecondaryPhotos = sensor2.Device.SecondaryPhotos,
                    IsOpen = false
                }

            ]
        };

        // Act
        GetDevicesResponse response = _controller.GetDevices(_home.Id.ToString());

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
                    CanAddDevices = member.HasPermission(new HomePermission(HomePermission.AddDevice)),
                    CanListDevices = member.HasPermission(new HomePermission(HomePermission.GetDevices)),
                    ShouldBeNotified = member.HasPermission(new HomePermission(HomePermission.GetNotifications))
                },

                new ListMemberInfo
                {
                    Id = otherMember.Id.ToString(),
                    Name = otherMember.User.Name,
                    Surname = otherMember.User.Surname,
                    Photo = otherMember.User.ProfilePicture ?? string.Empty,
                    CanAddDevices = otherMember.HasPermission(new HomePermission(HomePermission.AddDevice)),
                    CanListDevices = otherMember.HasPermission(new HomePermission(HomePermission.GetDevices)),
                    ShouldBeNotified =
                        otherMember.HasPermission(new HomePermission(HomePermission.GetNotifications))
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
}
