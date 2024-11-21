using BusinessLogic.Notifications.Models;
using BusinessLogic.Users.Entities;

namespace HomeConnect.WebApi.Controllers.Notifications.Models;

public record GetNotificationsRequest
{
    public string? Device { get; set; }
    public string? DateCreated { get; set; }
    public bool? Read { get; set; }

    public GetNotificationsArgs ToArgs(User user, DateTime? dateCreated)
    {
        return new GetNotificationsArgs
        {
            UserId = user.Id, DeviceFilter = Device, DateFilter = dateCreated, ReadFilter = Read
        };
    }
}
