namespace HomeConnect.WebApi.Controllers.Businesses.Models;

public struct GetDevicesResponse
{
    public List<DeviceInfo> Devices { get; set; }
    public Pagination Pagination { get; set; }
}
