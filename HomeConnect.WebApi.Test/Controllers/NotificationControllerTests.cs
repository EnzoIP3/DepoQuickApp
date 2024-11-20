using System.Globalization;
using BusinessLogic.BusinessOwners.Entities;
using BusinessLogic.Devices.Entities;
using BusinessLogic.HomeOwners.Entities;
using BusinessLogic.Notifications.Entities;
using BusinessLogic.Notifications.Services;
using BusinessLogic.Roles.Entities;
using BusinessLogic.Users.Entities;
using FluentAssertions;
using HomeConnect.WebApi.Controllers.Notifications;
using HomeConnect.WebApi.Controllers.Notifications.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        var business = new Business("5.234.235-1", "Business", "https://image.com/logo.png", user);
        var device = new Device("Device", "123123", "a description", "https://image.com/photo.png", [], "Camera",
            business);
        var notification = new Notification
        {
            Event = "Event", OwnedDevice = new OwnedDevice(new Home(), device), Read = false, Date = DateTime.Now
        };
        var items = new Dictionary<object, object?> { { Item.UserLogged, user } };
        List<Notification> notifications = [notification];
        DateTime dateCreated = DateFromString(request.DateCreated);
        _httpContextMock.SetupGet(h => h.Items).Returns(items);
        _notificationService.Setup(n => n.GetNotifications(user.Id, request.Device, dateCreated, request.Read))
            .Returns([notification]);
        _notificationService.Setup(n => n.MarkNotificationsAsRead(notifications))
            .Verifiable();

        // Act
        GetNotificationsResponse response = _notificationController.GetNotifications(request);

        // Assert
        _notificationService.VerifyAll();
        response.Should().NotBeNull();
        response.Notifications.Count.Should().Be(1);
        response.Notifications[0].Event.Should().Be(notification.Event);
        response.Notifications[0].Device.Should().Be(notification.OwnedDevice.ToOwnedDeviceDto());
        response.Notifications[0].Read.Should().Be(notification.Read);
        response.Notifications[0].DateCreated.Should()
            .Be(notification.Date.ToString("dd MMMM yyyy HH:mm:ss", CultureInfo.InvariantCulture));
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
        Func<GetNotificationsResponse> action = () => _notificationController.GetNotifications(request);

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
