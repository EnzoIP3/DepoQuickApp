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
    public IDeviceRepository DeviceRepository { get; init; }
    public IUserRepository UserRepository { get; init; }
    public IBusinessRepository BusinessRepository { get; init; }
    public IRoleRepository RoleRepository { get; init; }

    public BusinessOwnerService(IUserRepository userRepository, IBusinessRepository businessRepository,
        IRoleRepository roleRepository, IDeviceRepository deviceRepository)
    {
        UserRepository = userRepository;
        BusinessRepository = businessRepository;
        RoleRepository = roleRepository;
        DeviceRepository = deviceRepository;
    }

    public string CreateBusiness(string ownerEmail, string businessRut, string businessName)
    {
        var owner = VerifyOwnerExists(ownerEmail);
        EnsureOwnerDoesNotHaveBusiness(ownerEmail);
        EnsureBusinessRutDoesNotExist(businessRut);

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

    private User VerifyOwnerExists(string ownerEmail)
    {
        var owner = UserRepository.GetUser(ownerEmail);
        if (owner == null)
        {
            throw new ArgumentException("Owner does not exist");
        }

        return owner;
    }

    private void EnsureOwnerDoesNotHaveBusiness(string ownerEmail)
    {
        var existingBusiness = BusinessRepository.GetBusinessByOwner(ownerEmail);
        if (existingBusiness != null)
        {
            throw new InvalidOperationException("Owner already has a business");
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
