using BusinessLogic;
using BusinessLogic.Devices.Entities;
using HomeConnect.WebApi.Controllers.Homes.Models;

namespace HomeConnect.WebApi.Controllers.Devices.Models;

public sealed record GetDevicesResponse
{
    public List<ListDeviceInfo> Devices { get; set; } = [];
    public Pagination Pagination { get; set; } = new();

    public static GetDevicesResponse FromDevices(PagedData<Device> devices)
    {
        return new GetDevicesResponse
        {
            Devices = devices.Data.Select(d => new ListDeviceInfo
            {
                Id = d.Id.ToString(),
                Name = d.Name,
                BusinessName = d.Business.Name,
                Type = d.Type.ToString(),
                ModelNumber = d.ModelNumber,
                MainPhoto = d.MainPhoto,
                SecondaryPhotos = d.SecondaryPhotos,
                Description = d.Description
            }).ToList(),
            Pagination = new Pagination
            {
                Page = devices.Page,
                PageSize = devices.PageSize,
                TotalPages = devices.TotalPages
            }
        };
    }
}
