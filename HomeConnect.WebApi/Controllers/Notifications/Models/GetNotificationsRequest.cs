namespace HomeConnect.WebApi.Controllers.Notification;

public record GetNotificationsRequest
{
    public string? Device { get; set; }
    public string? DateCreated { get; set; }
    public bool? Read { get; set; }
}
