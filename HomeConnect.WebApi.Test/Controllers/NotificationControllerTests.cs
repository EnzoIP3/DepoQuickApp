using BusinessLogic.Devices.Entities;
using BusinessLogic.Notifications.Entities;
using BusinessLogic.Notifications.Services;
using BusinessLogic.Roles.Entities;
using BusinessLogic.Users.Entities;
using HomeConnect.WebApi.Controllers.Notification;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Moq;

namespace HomeConnect.WebApi.Test.Controllers;

[TestClass]
public class NotificationControllerTests
{
    private AuthorizationFilterContext _context = null!;
    private Mock<HttpContext> _httpContextMock = null!;
    private NotificationController _notificationController = null!;
    private Mock<INotificationService> _notificationService = null!;

    [TestInitialize]
    public void Initialize()
    {
        _notificationService = new Mock<INotificationService>();
        _notificationController = new NotificationController(_notificationService.Object);
        _httpContextMock = new Mock<HttpContext>(MockBehavior.Strict);
        _context = new AuthorizationFilterContext(
            new ActionContext(
                _httpContextMock.Object,
                new RouteData(),
                new ActionDescriptor()),
            new List<IFilterMetadata>());
    }

    [TestMethod]
    public void GetNotifications_WhenValidRequest_ReturnsNotifications()
    {
        // Arrange
        var request = new GetNotificationsRequest
        {
            Device = Guid.NewGuid().ToString(), DateCreated = DateTime.Now, Read = false
        };
        var user = new User("John", "Doe", "email@email.com", "Password@100",
            new Role { Name = "HomeOwner", Permissions = [] });
        var notification = new Notification
        {
            Event = "Event",
            OwnedDevice = new OwnedDevice { HardwareId = Guid.NewGuid() },
            Read = false,
            Date = DateTime.Now
        };
        var items = new Dictionary<object, object?> { { Item.UserLogged, user } };
        _httpContextMock.SetupGet(h => h.Items).Returns(items);
        _notificationService.Setup(n => n.GetNotifications(user.Id, request.Device, request.DateCreated, request.Read))
            .Returns([notification]);

        // Act
        GetNotificationsResponse response = _notificationController.GetNotifications(request, _context);

        // Assert
        _notificationService.VerifyAll();
        Assert.IsNotNull(response);
        Assert.AreEqual(1, response.Notifications.Count);
        Assert.AreEqual(notification.Event, response.Notifications[0].Event);
        Assert.AreEqual(notification.OwnedDevice.HardwareId.ToString(), response.Notifications[0].DeviceId);
        Assert.AreEqual(notification.Read, response.Notifications[0].Read);
        Assert.AreEqual(notification.Date, response.Notifications[0].DateCreated);
    }
}
