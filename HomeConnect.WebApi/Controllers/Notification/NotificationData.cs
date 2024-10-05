namespace HomeConnect.WebApi.Controllers.Notification;

public struct NotificationData
{
    public string Event { get; set; }
    public string DeviceId { get; set; }
    public bool Read { get; set; }
    public DateTime DateCreated { get; set; }
}
