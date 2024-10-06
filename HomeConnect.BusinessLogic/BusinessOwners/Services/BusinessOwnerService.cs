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

    public Business CreateBusiness(CreateBusinessArgs args)
    {
        EnsureIsValidGuid(args.OwnerId);
        EnsureOwnerExists(Guid.Parse(args.OwnerId));
        EnsureOwnerDoesNotHaveBusiness(Guid.Parse(args.OwnerId));
        EnsureBusinessRutDoesNotExist(args.Rut);
        var owner = UserRepository.Get(Guid.Parse(args.OwnerId));
        var business = new Business(args.Rut, args.Name, owner);
        BusinessRepository.Add(business);
        return business;
    }

    private static void EnsureIsValidGuid(string id)
    {
        if (Guid.TryParse(id, out _) == false)
        {
            throw new ArgumentException("The business owner ID is not a valid GUID.");
        }
    }

    public Device CreateDevice(CreateDeviceArgs args)
    {
        EnsureBusinessExists(args.BusinessRut);
        var business = BusinessRepository.Get(args.BusinessRut);
        var device = new Device(args.Name, args.ModelNumber, args.Description, args.MainPhoto, args.SecondaryPhotos,
            args.Type, business);
        DeviceRepository.EnsureDeviceDoesNotExist(device);
        DeviceRepository.Add(device);
        return device;
    }

    public Camera CreateCamera(CreateCameraArgs args)
    {
        EnsureBusinessExists(args.BusinessRut);
        var business = BusinessRepository.Get(args.BusinessRut);
        var camera = new Camera(args.Name, args.ModelNumber, args.Description, args.MainPhoto, args.SecondaryPhotos,
            business, args.MotionDetection, args.PersonDetection, args.IsExterior, args.IsInterior);
        DeviceRepository.EnsureDeviceDoesNotExist(camera);
        DeviceRepository.Add(camera);
        return camera;
    }

    private void EnsureBusinessExists(string rut)
    {
        if (!BusinessRepository.Exists(rut))
        {
            throw new ArgumentException("That business does not exist.");
        }
    }

    private void EnsureOwnerExists(Guid ownerId)
    {
        if (!UserRepository.Exists(ownerId))
        {
            throw new ArgumentException("That business owner does not exist.");
        }
    }

    private void EnsureOwnerDoesNotHaveBusiness(Guid ownerId)
    {
        if (BusinessRepository.ExistsByOwnerId(ownerId))
        {
            throw new InvalidOperationException("Owner already has a business.");
        }
    }

    private void EnsureBusinessRutDoesNotExist(string businessRut)
    {
        if (BusinessRepository.Exists(businessRut))
        {
            throw new InvalidOperationException("There is already a business with this RUT.");
        }
    }
}
