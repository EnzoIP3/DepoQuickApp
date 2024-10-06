using BusinessLogic.BusinessOwners.Entities;
using BusinessLogic.Devices.Entities;
using BusinessLogic.HomeOwners.Entities;
using BusinessLogic.HomeOwners.Models;
using BusinessLogic.HomeOwners.Services;
using BusinessLogic.Roles.Entities;
using BusinessLogic.Users.Entities;
using FluentAssertions;
using HomeConnect.WebApi.Controllers.Home;
using HomeConnect.WebApi.Controllers.Home.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Moq;

namespace HomeConnect.WebApi.Test.Controllers;

[TestClass]
public class HomeControllerTests
{
    private HomeController _controller = null!;
    private Mock<HttpContext> _httpContextMock = null!;
    private Mock<IHomeOwnerService> _homeOwnerService = null!;
    private AuthorizationFilterContext _context = null!;

    private static readonly User _user = new User("John", "Doe", "email@email.com", "Password@100",
        new Role { Name = "HomeOwner", Permissions = [] });

    private readonly User _otherUser = new User("Jane", "Doe", "email2@email.com", "Password@100",
        new Role { Name = "HomeOwner", Permissions = [] });

    private readonly Home _home = new Home(_user, "Road 123", 50.456, 100.789, 3);

    [TestInitialize]
    public void Initialize()
    {
        _httpContextMock = new Mock<HttpContext>(MockBehavior.Strict);
        _homeOwnerService = new Mock<IHomeOwnerService>(MockBehavior.Strict);
        _controller = new HomeController(_homeOwnerService.Object);
        _controller.ControllerContext = new ControllerContext { HttpContext = _httpContextMock.Object };
        _context = new AuthorizationFilterContext(
            new ActionContext(
                _httpContextMock.Object,
                new RouteData(),
                new ActionDescriptor()),
            new List<IFilterMetadata>());
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
            MaxMembers = 3
        };
        var items = new Dictionary<object, object?> { { Item.UserLogged, _user } };
        _httpContextMock.Setup(h => h.Items).Returns(items);
        var home = new Home(_user, request.Address, request.Latitude, request.Longitude, request.MaxMembers);
        var args = new CreateHomeArgs
        {
            HomeOwnerId = _user.Id.ToString(),
            Address = request.Address,
            Latitude = request.Latitude,
            Longitude = request.Longitude,
            MaxMembers = request.MaxMembers
        };
        _homeOwnerService.Setup(x => x.CreateHome(args)).Returns(home.Id);

        // Act
        var response = _controller.CreateHome(request);

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
            HomeId = _home.Id.ToString(),
            HomeOwnerId = _user.Id.ToString(),
            CanAddDevices = true,
            CanListDevices = false
        };
        var items = new Dictionary<object, object?> { { Item.UserLogged, _user } };
        _httpContextMock.Setup(h => h.Items).Returns(items);
        var args = new AddMemberArgs
        {
            HomeId = _home.Id.ToString(),
            HomeOwnerId = _user.Id.ToString(),
            CanAddDevices = request.CanAddDevices,
            CanListDevices = request.CanListDevices
        };
        _homeOwnerService.Setup(x => x.AddMemberToHome(args)).Returns(_user.Id);

        // Act
        var response = _controller.AddMember(_home.Id.ToString(), request);

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
        var sensor = new Device("sensor", 123, "a camera", "https://www.example.com/photo1.jpg", [],
            "sensor", business);
        var camera = new Camera("camera", 123, "a camera", "https://www.example.com/photo1.jpg", [],
            business, true, true, false, true);

        var request = new AddDevicesRequest { DeviceIds = [sensor.Id.ToString(), camera.Id.ToString()] };
        var items = new Dictionary<object, object?> { { Item.UserLogged, _user } };
        _httpContextMock.Setup(h => h.Items).Returns(items);
        var args = new AddDevicesArgs { HomeId = _home.Id.ToString(), DeviceIds = request.DeviceIds };
        _homeOwnerService.Setup(x => x.AddDeviceToHome(args));

        // Act
        var response = _controller.AddDevices(_home.Id.ToString(), request);

        // Assert
        _homeOwnerService.VerifyAll();
        response.Should().NotBeNull();
        response.DeviceIds.Should().BeEquivalentTo(request.DeviceIds);
        response.HomeId.Should().Be(_home.Id.ToString());
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
            new HomePermission("canAddDevices"),
            new HomePermission("canListDevices"),
            new HomePermission("shouldBeNotified")
        ]);
        var otherMember = new Member(_otherUser, [new HomePermission("canAddDevices")]);
        home.AddMember(member);
        home.AddMember(otherMember);
        var items = new Dictionary<object, object?> { { Item.UserLogged, _user } };
        _httpContextMock.Setup(h => h.Items).Returns(items);
        _homeOwnerService.Setup(x => x.GetHomeMembers(home.Id.ToString()))
            .Returns([member, otherMember]);

        var expectedResponse = CreateGetMembersResponse(member, otherMember);

        // Act
        var response = _controller.GetMembers(home.Id.ToString(), _context);

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
                    Id = member.User.Id.ToString(),
                    Name = member.User.Name,
                    Surname = member.User.Surname,
                    Photo = member.User.ProfilePicture ?? string.Empty,
                    CanAddDevices = member.HasPermission(new HomePermission("canAddDevices")),
                    CanListDevices = member.HasPermission(new HomePermission("canListDevices")),
                    ShouldBeNotified = member.HasPermission(new HomePermission("shouldBeNotified"))
                },

                new ListMemberInfo
                {
                    Id = otherMember.User.Id.ToString(),
                    Name = otherMember.User.Name,
                    Surname = otherMember.User.Surname,
                    Photo = otherMember.User.ProfilePicture ?? string.Empty,
                    CanAddDevices = otherMember.HasPermission(new HomePermission("canAddDevices")),
                    CanListDevices = otherMember.HasPermission(new HomePermission("canListDevices")),
                    ShouldBeNotified = otherMember.HasPermission(new HomePermission("shouldBeNotified"))
                }

            ]
        };
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
                ModelNumber = 1,
                MainPhoto = "https://www.example.com/photo1.jpg"
            });
        var device2 = new OwnedDevice(_home,
            new Device
            {
                Name = "Device2",
                Type = DeviceType.Camera,
                ModelNumber = 2,
                MainPhoto = "https://www.example.com/photo2.jpg"
            });
        var items = new Dictionary<object, object?> { { Item.UserLogged, _user } };
        _httpContextMock.Setup(h => h.Items).Returns(items);
        _homeOwnerService.Setup(x => x.GetHomeDevices(_home.Id.ToString()))
            .Returns(new List<OwnedDevice> { device1, device2 });

        var expectedResponse = new GetDevicesResponse
        {
            Device =
            [
                new ListDeviceInfo
                {
                    Name = device1.Device.Name,
                    Type = device1.Device.Type.ToString(),
                    ModelNumber = device1.Device.ModelNumber,
                    Photo = device1.Device.MainPhoto,
                    IsConnected = device1.Connected
                },
                new ListDeviceInfo
                {
                    Name = device2.Device.Name,
                    Type = device2.Device.Type.ToString(),
                    ModelNumber = device2.Device.ModelNumber,
                    Photo = device2.Device.MainPhoto,
                    IsConnected = device2.Connected
                }

            ]
        };

        // Act
        var response = _controller.GetDevices(_home.Id.ToString(), _context);

        // Assert
        _homeOwnerService.VerifyAll();
        response.Should().NotBeNull();
        response.Device.Should().NotBeNullOrEmpty();
        response.Device.Should().HaveCount(2);
        response.Device.Should().BeEquivalentTo(expectedResponse.Device);
    }

    #endregion
}
