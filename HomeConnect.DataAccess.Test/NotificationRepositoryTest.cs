using BusinessLogic;
using FluentAssertions;

namespace HomeConnect.DataAccess.Test;

[TestClass]
public class NotificationRepositoryTest
{
    private readonly Context _context = DbContextBuilder.BuildTestDbContext();
    private NotificationRepository _notificationRepository = null!;

    [TestInitialize]
    public void Initialize()
    {
        _context.Database.EnsureCreated();
        _notificationRepository = new NotificationRepository(_context);
        _context.SaveChanges();
    }

    [TestCleanup]
    public void Cleanup()
    {
        _context.Database.EnsureDeleted();
    }

    [TestMethod]
    public void Add_WhenNotificationDoesNotExist_ShouldAddNotification()
    {
        // Arrange
        var notification = new Notification("Notification", "Notification description", "https://example.com/image.png",
            "Notification message");

        // Act
        _notificationRepository.Add(notification);

        // Assert
        _context.Notifications.Should().Contain(notification);
    }
}
