namespace HomeConnect.WebApi.Controllers.Notification;

public record NotificationData
{
    public string Event { get; set; } = null!;
    public string DeviceId { get; set; } = null!;
    public bool Read { get; set; }
    public DateTime DateCreated { get; set; }
}
