namespace HomeConnect.WebApi.Controllers.Home.Models;

public struct AddDevicesResponse
{
    public string HomeId { get; set; }
    public List<string> DeviceIds { get; set; }
}
