using BusinessLogic.BusinessOwners.Entities;

namespace BusinessLogic.BusinessOwners.Services;

public interface IBusinessOwnerService
{
    public string CreateBusiness(string ownerEmail, string businessRut, string businessName);
    public Guid CreateDevice(string name, int modelNumber, string description, string mainPhoto, List<string> secondaryPhotos, string type, Business business);
    public Guid CreateCamera(string name, int modelNumber, string description, string mainPhoto, List<string> secondaryPhotos, Business business,
        bool motionDetection, bool personDetection, bool isExterior, bool isInterior);
}
