namespace HomeConnect.WebApi.Controllers.Notifications.Models;

public record GetNotificationsRequest
{
    public string? Device { get; set; }
    public string? DateCreated { get; set; }
    public bool? Read { get; set; }
}
