using BusinessLogic.Devices.Models;

namespace HomeConnect.WebApi.Controllers.Notifications.Models;

public record NotificationData
{
    public string Event { get; set; } = null!;
    public OwnedDeviceDto Device { get; set; } = null!;
    public bool Read { get; set; }
    public string DateCreated { get; set; } = null!;
}
