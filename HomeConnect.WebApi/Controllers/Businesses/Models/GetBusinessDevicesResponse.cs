using BusinessLogic;
using BusinessLogic.Devices.Entities;

namespace HomeConnect.WebApi.Controllers.Businesses.Models;

public struct GetBusinessDevicesResponse
{
    public List<DeviceInfo> Devices { get; set; }
    public Pagination Pagination { get; set; }

    public static GetBusinessDevicesResponse FromDevices(PagedData<Device> devices)
    {
        return new GetBusinessDevicesResponse
        {
            Devices = devices.Data.Select(d => new DeviceInfo
            {
                Id = d.Id.ToString(),
                Name = d.Name,
                ModelNumber = d.ModelNumber,
                Description = d.Description,
                MainPhoto = d.MainPhoto,
                SecondaryPhotos = d.SecondaryPhotos,
                Type = d.Type.ToString(),
                BusinessName = d.Business.Name
            }).ToList(),
            Pagination = new Pagination
            {
                Page = devices.Page, PageSize = devices.PageSize, TotalPages = devices.TotalPages
            }
        };
    }
}
