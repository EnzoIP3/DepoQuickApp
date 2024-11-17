using BusinessLogic.BusinessOwners.Entities;
using BusinessLogic.BusinessOwners.Models;
using BusinessLogic.Devices.Entities;
using BusinessLogic.Users.Entities;

namespace BusinessLogic.BusinessOwners.Services;

public interface IBusinessOwnerService
{
    public Business CreateBusiness(CreateBusinessArgs businessArgs);
    public Device CreateDevice(CreateDeviceArgs args);
    public Camera CreateCamera(CreateCameraArgs args);
    public void UpdateValidator(UpdateValidatorArgs args);
    public PagedData<Business> GetBusinesses(string ownerFilter, int currentPage, int pageSize);
    public PagedData<Device> GetDevices(GetBusinessDevicesArgs args);
}
