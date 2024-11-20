namespace HomeConnect.WebApi.Controllers.Notifications.Models;

public record NotificationData
{
    public string Event { get; set; } = null!;
    public string DeviceId { get; set; } = null!;
    public bool Read { get; set; }
    public string DateCreated { get; set; } = null!;
}
