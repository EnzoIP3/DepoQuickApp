namespace HomeConnect.WebApi.Controllers.Devices.Models;

public sealed record ImportDevicesResponse
{
    public List<string> ImportedDevices { get; set; } = [];
}
