namespace HomeConnect.WebApi.Controllers.Notification;

public record GetNotificationsRequest
{
    public string? Device { get; set; } = null!;
    public string? DateCreated { get; set; }
    public bool? Read { get; set; }
}
