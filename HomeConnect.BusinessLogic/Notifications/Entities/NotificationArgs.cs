namespace HomeConnect.WebApi.Controllers.Sensor;

public struct NotificationArgs
{
    public string HardwareId { get; set; }
    public DateTime Date { get; set; }
    public string Event { get; set; }
}
