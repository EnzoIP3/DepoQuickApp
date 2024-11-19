namespace HomeConnect.WebApi.Controllers.Devices.Models;

public struct ImportDevicesRequest
{
    public string ImporterName { get; set; }
    public Dictionary<string, string> Parameters { get; set; }
}
