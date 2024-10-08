using System.Globalization;
using BusinessLogic.Devices.Entities;
using BusinessLogic.Notifications.Entities;
using BusinessLogic.Notifications.Services;
using BusinessLogic.Roles.Entities;
using BusinessLogic.Users.Entities;
using FluentAssertions;
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
    private Mock<HttpContext> _httpContextMock = null!;
    private NotificationController _notificationController = null!;
    private Mock<INotificationService> _notificationService = null!;

    [TestInitialize]
    public void Initialize()
    {
        _notificationService = new Mock<INotificationService>(MockBehavior.Strict);
        _httpContextMock = new Mock<HttpContext>(MockBehavior.Strict);
        _notificationController = new NotificationController(_notificationService.Object)
        {
            ControllerContext = new ControllerContext { HttpContext = _httpContextMock.Object }
        };
    }

    [TestMethod]
    public void GetNotifications_WhenValidRequest_ReturnsNotifications()
    {
        // Arrange
        var request = new GetNotificationsRequest
        {
            Device = Guid.NewGuid().ToString(), DateCreated = DateToString(DateTime.Now), Read = false
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
        var dateCreated = DateFromString(request.DateCreated);
        _httpContextMock.SetupGet(h => h.Items).Returns(items);
        _notificationService.Setup(n => n.GetNotifications(user.Id, request.Device, dateCreated, request.Read))
            .Returns([notification]);
        _notificationService.Setup(n => n.MarkNotificationsAsRead(new List<Notification> { notification })).Verifiable();

        // Act
        GetNotificationsResponse response = _notificationController.GetNotifications(request);

        // Assert
        _notificationService.VerifyAll();
        response.Should().NotBeNull();
        response.Notifications.Count.Should().Be(1);
        response.Notifications[0].Event.Should().Be(notification.Event);
        response.Notifications[0].DeviceId.Should().Be(notification.OwnedDevice.HardwareId.ToString());
        response.Notifications[0].Read.Should().Be(notification.Read);
        response.Notifications[0].DateCreated.Should().Be(notification.Date);
    }

    [TestMethod]
    public void GetNotifications_WhenInvalidFormatDateCreated_ThrowsArgumentException()
    {
        // Arrange
        var request = new GetNotificationsRequest
        {
            Device = Guid.NewGuid().ToString(), DateCreated = "2024/12/2", Read = false
        };

        // Act
        var action = () => _notificationController.GetNotifications(request);

        // Assert
        _notificationService.VerifyAll();
        action.Should().Throw<ArgumentException>().WithMessage("The date created filter is invalid");
    }

    private static string DateToString(DateTime date)
    {
        return date.ToString("dd-MM-yyyy");
    }

    private static DateTime DateFromString(string date)
    {
        return DateTime.ParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture);
    }
}
