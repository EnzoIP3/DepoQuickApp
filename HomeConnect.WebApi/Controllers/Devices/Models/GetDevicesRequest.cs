using BusinessLogic.Devices.Models;

namespace HomeConnect.WebApi.Controllers.Devices.Models;

public record GetDevicesRequest
{
    public string? Name { get; set; }
    public string? ModelNumber { get; set; }
    public string? BusinessName { get; set; }
    public string? Type { get; set; }
    public int? Page { get; set; }
    public int? PageSize { get; set; }

    public GetDevicesArgs ToGetDevicesArgs()
    {
        return new GetDevicesArgs
        {
            BusinessNameFilter = BusinessName,
            DeviceTypeFilter = Type,
            Page = Page,
            PageSize = PageSize,
            DeviceNameFilter = Name,
            ModelNumberFilter = ModelNumber
        };
    }
}
