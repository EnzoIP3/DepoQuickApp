namespace BusinessLogic.Notifications.Models;

public record NotificationArgs
{
    public string HardwareId { get; set; } = null!;
    public DateTime Date { get; set; }
    public string Event { get; set; } = null!;
}
