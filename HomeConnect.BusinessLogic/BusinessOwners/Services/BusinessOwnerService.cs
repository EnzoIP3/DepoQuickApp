using BusinessLogic.BusinessOwners.Entities;
using BusinessLogic.BusinessOwners.Repositories;
using BusinessLogic.Devices.Entities;
using BusinessLogic.Devices.Repositories;
using BusinessLogic.Roles.Repositories;
using BusinessLogic.Users.Entities;
using BusinessLogic.Users.Repositories;

namespace BusinessLogic.BusinessOwners.Services;

public class BusinessOwnerService : IBusinessOwnerService
{
    private IDeviceRepository DeviceRepository { get; init; }
    private IUserRepository UserRepository { get; init; }
    private IBusinessRepository BusinessRepository { get; init; }

    public BusinessOwnerService(IUserRepository userRepository, IBusinessRepository businessRepository,
        IDeviceRepository deviceRepository)
    {
        UserRepository = userRepository;
        BusinessRepository = businessRepository;
        DeviceRepository = deviceRepository;
    }

    public string CreateBusiness(string ownerEmail, string businessRut, string businessName)
    {
        EnsureOwnerExists(ownerEmail);
        EnsureOwnerDoesNotHaveBusiness(ownerEmail);
        EnsureBusinessRutDoesNotExist(businessRut);

        var owner = UserRepository.Get(ownerEmail);
        var business = new Business(businessRut, businessName, owner);
        BusinessRepository.Add(business);
        return business.Rut;
    }

    public Guid CreateDevice(string name, int modelNumber, string description, string mainPhoto, List<string> secondaryPhotos, string type, Business business)
    {
        var device = new Device(name, modelNumber, description, mainPhoto, secondaryPhotos, type, business);
        DeviceRepository.EnsureDeviceDoesNotExist(device);
        DeviceRepository.Add(device);
        return device.Id;
    }

    public Guid CreateCamera(string name, int modelNumber, string description, string mainPhoto, List<string> secondaryPhotos,
        Business business, bool motionDetection, bool personDetection, bool isExterior, bool isInterior)
    {
        var camera = new Camera(name, modelNumber, description, mainPhoto, secondaryPhotos, business, motionDetection, personDetection, isExterior, isInterior);
        DeviceRepository.EnsureDeviceDoesNotExist(camera);
        DeviceRepository.Add(camera);
        return camera.Id;
    }

    private void EnsureOwnerExists(string ownerEmail)
    {
        if (!UserRepository.Exists(ownerEmail))
        {
            throw new ArgumentException("Owner does not exist");
        }
    }

    private void EnsureOwnerDoesNotHaveBusiness(string ownerEmail)
    {
        var existingBusiness = BusinessRepository.GetBusinessByOwner(ownerEmail);
        if (existingBusiness != null)
        {
            throw new ArgumentException("Owner already has a business");
        }
    }

    private void EnsureBusinessRutDoesNotExist(string businessRut)
    {
        var existingBusinessByRut = BusinessRepository.GetBusinessByRut(businessRut);
        if (existingBusinessByRut != null)
        {
            throw new InvalidOperationException("RUT already exists");
        }
    }
}
