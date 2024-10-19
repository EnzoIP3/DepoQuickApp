using BusinessLogic.BusinessOwners.Entities;
using BusinessLogic.BusinessOwners.Models;
using BusinessLogic.BusinessOwners.Services;
using BusinessLogic.Devices.Entities;
using BusinessLogic.Users.Entities;
using FluentAssertions;
using HomeConnect.WebApi.Controllers.Lamps;
using HomeConnect.WebApi.Controllers.Lamps.Models;
using Microsoft.AspNetCore.Http;
using Moq;

namespace HomeConnect.WebApi.Test.Controllers;

[TestClass]
public class LampControllerTests
{
    private Mock<IBusinessOwnerService> _businessOwnerServiceMock = null!;
    private Mock<HttpContext> _httpContextMock = null!;
    private LampController _lampController = null!;

    [TestInitialize]
    public void TestInitialize()
    {
        _httpContextMock = new Mock<HttpContext>();
        _businessOwnerServiceMock = new Mock<IBusinessOwnerService>();
        _lampController =
            new LampController(_businessOwnerServiceMock.Object)
                { ControllerContext = { HttpContext = _httpContextMock.Object } };
    }

    #region CreateDevices

    [TestMethod]
    public void CreateLamp_WhenCalledWithValidRequest_ReturnsCreatedResponse()
    {
        // Arrange
        var user = new User();
        var lamp = new Device("name", 123, "description", "http://example.com/photo.png", [],
            DeviceType.Lamp.ToString(),
            new Business());
        var lampArgs = new CreateDeviceArgs
        {
            Owner = user,
            Description = lamp.Description,
            MainPhoto = lamp.MainPhoto,
            ModelNumber = lamp.ModelNumber,
            Name = lamp.Name,
            SecondaryPhotos = lamp.SecondaryPhotos,
            Type = lamp.Type.ToString()
        };
        var lampRequest = new CreateLampRequest
        {
            Description = lamp.Description,
            MainPhoto = lamp.MainPhoto,
            ModelNumber = lamp.ModelNumber,
            Name = lamp.Name,
            SecondaryPhotos = lamp.SecondaryPhotos
        };
        _businessOwnerServiceMock.Setup(x => x.CreateDevice(lampArgs)).Returns(lamp);
        var items = new Dictionary<object, object?> { { Item.UserLogged, user } };
        _httpContextMock.Setup(h => h.Items).Returns(items);

        // Act
        CreateLampResponse response = _lampController.CreateLamp(lampRequest);

        // Assert
        _businessOwnerServiceMock.VerifyAll();
        response.Should().NotBeNull();
        response.Id.Should().Be(lamp.Id);
    }
    #endregion
}
