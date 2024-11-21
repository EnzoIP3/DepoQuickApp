namespace BusinessLogic.Notifications.Models;

public record GetNotificationsArgs
{
    public Guid UserId { get; set; }
    public string? DeviceFilter { get; set; }
    public DateTime? DateFilter { get; set; }
    public bool? ReadFilter { get; set; }
}
