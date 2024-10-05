using BusinessLogic.BusinessOwners.Models;

namespace BusinessLogic.BusinessOwners.Services;

public interface IBusinessOwnerService
{
    public string CreateBusiness(CreateBusinessArgs businessArgs);
    public Guid CreateDevice(CreateDeviceArgs args);
    public Guid CreateCamera(CreateCameraArgs args);
}
