namespace BusinessLogic.Notifications.Models;

public record NotificationArgs
{
    public string HardwareId { get; set; } = null!;
    public DateTime Date { get; set; }
    public string Event { get; set; } = null!;
    public string? UserEmail { get; set; }

    public static NotificationArgs CreateMovementDetectedNotificationArgs(string hardwareId)
    {
        return new NotificationArgs { HardwareId = hardwareId, Date = DateTime.Now, Event = "Movement detected" };
    }

    public static NotificationArgs CreatePersonDetectedNotificationArgs(string hardwareId, string userEmail)
    {
        return new NotificationArgs
        {
            HardwareId = hardwareId,
            Date = DateTime.Now,
            Event = $"Person detected with email: {userEmail}"
        };
    }
}
