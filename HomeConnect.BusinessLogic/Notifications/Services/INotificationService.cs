using HomeConnect.WebApi.Controllers.Sensor;

namespace BusinessLogic.Notifications.Services;

public interface INotificationService
{
    void Notify(NotificationArgs args);
}
