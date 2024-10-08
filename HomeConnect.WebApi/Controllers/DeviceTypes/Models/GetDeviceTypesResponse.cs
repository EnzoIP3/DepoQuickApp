namespace HomeConnect.WebApi.Controllers.DeviceTypes.Models;

public record GetDeviceTypesResponse
{
    public List<string> DeviceTypes { get; set; } = null!;
}
