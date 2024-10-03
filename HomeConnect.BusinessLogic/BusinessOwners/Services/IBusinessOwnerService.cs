using BusinessLogic.BusinessOwners.Entities;

namespace BusinessLogic.BusinessOwners.Services;

public interface IBusinessOwnerService
{
    public void CreateBusiness(string ownerEmail, string businessRut, string businessName);
    public void CreateDevice(string name, int modelNumber, string description, string mainPhoto, List<string> secondaryPhotos, string type, Business business);
}
