namespace HomeConnect.WebApi.Controllers.Notification;

public struct GetNotificationsRequest
{
    public string Device { get; set; }
    public DateTime DateCreated { get; set; }
    public bool Read { get; set; }
}
