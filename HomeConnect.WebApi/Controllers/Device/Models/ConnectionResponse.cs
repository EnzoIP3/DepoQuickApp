namespace HomeConnect.WebApi.Controllers.Device.Models;

public struct ConnectionResponse
{
    public bool ConnectionState { get; set; }
    public string HardwareId { get; set; }
}
