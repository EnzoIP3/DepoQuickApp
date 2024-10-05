using BusinessLogic.BusinessOwners.Entities;
using BusinessLogic.BusinessOwners.Models;
using BusinessLogic.BusinessOwners.Repositories;
using BusinessLogic.Devices.Entities;
using BusinessLogic.Devices.Repositories;
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

    public string CreateBusiness(CreateBusinessArgs args)
    {
        EnsureOwnerExists(args.OwnerId);
        EnsureOwnerDoesNotHaveBusiness(args.OwnerId);
        EnsureBusinessRutDoesNotExist(args.Rut);
        var owner = UserRepository.Get(args.OwnerId);
        var business = new Business(args.Rut, args.Name, owner);
        BusinessRepository.Add(business);
        return business.Rut;
    }

    public Guid CreateDevice(CreateDeviceArgs args)
    {
        var business = BusinessRepository.GetBusinessByRut(args.BusinessRut);
        var device = new Device(args.Name, args.ModelNumber, args.Description, args.MainPhoto, args.SecondaryPhotos,
            args.Type, business!);
        DeviceRepository.EnsureDeviceDoesNotExist(device);
        DeviceRepository.Add(device);
        return device.Id;
    }

    public Guid CreateCamera(CreateCameraArgs args)
    {
        var business = BusinessRepository.GetBusinessByRut(args.BusinessRut);
        var camera = new Camera(args.Name, args.ModelNumber, args.Description, args.MainPhoto, args.SecondaryPhotos,
            business!, args.MotionDetection, args.PersonDetection, args.IsExterior, args.IsInterior);
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
